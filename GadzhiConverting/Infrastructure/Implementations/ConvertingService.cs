using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces.Converters;
using GadzhiConverting.Models.Interfaces.FilesConvert;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;
using GadzhiCommon.Extensions.Functional.Result;
using GadzhiCommon.Extensions.StringAdditional;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Infrastructure.Implementations.Reflection;
using GadzhiCommon.Infrastructure.Interfaces.Logger;
using GadzhiCommon.Models.Enums;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Models.Implementations.FilesConvert;
using GadzhiConvertingLibrary.Infrastructure.Implementations.Services;
using GadzhiConvertingLibrary.Infrastructure.Interfaces.Services;
using static GadzhiCommon.Infrastructure.Implementations.ExecuteAndCatchErrors;
using static GadzhiCommon.Extensions.Functional.Result.ExecuteResultHandler;

namespace GadzhiConverting.Infrastructure.Implementations
{
    /// <summary>
    /// Запуск процесса конвертирования
    /// </summary>
    public class ConvertingService : IConvertingService
    {
        public ConvertingService(IConvertingFileData convertingFileData, IProjectSettings projectSettings,
                                 IWcfServerServicesFactory wcfServerServicesFactory,
                                 IConverterServerPackageDataFromDto converterServerPackageDataFromDto,
                                 IConverterServerPackageDataToDto converterServerPackageDataToDto,
                                 IMessagingService messagingService, IFileSystemOperations fileSystemOperations)
        {
            _convertingFileData = convertingFileData ?? throw new ArgumentNullException(nameof(convertingFileData));
            _projectSettings = projectSettings ?? throw new ArgumentNullException(nameof(projectSettings));
            _convertingServerServiceFactory = wcfServerServicesFactory?.ConvertingServerServiceFactory ?? throw new ArgumentNullException(nameof(wcfServerServicesFactory));
            _converterServerPackageDataFromDto = converterServerPackageDataFromDto ?? throw new ArgumentNullException(nameof(converterServerPackageDataFromDto));
            _converterServerPackageDataToDto = converterServerPackageDataToDto ?? throw new ArgumentNullException(nameof(converterServerPackageDataToDto));
            _messagingService = messagingService ?? throw new ArgumentNullException(nameof(messagingService));
            _fileSystemOperations = fileSystemOperations ?? throw new ArgumentNullException(nameof(fileSystemOperations));
         
            _convertingUpdaterSubscriptions = new CompositeDisposable();
            _idPackage = null;
        }

        /// <summary>
        /// Журнал системных сообщений
        /// </summary>
        private static readonly ILoggerService _loggerService = LoggerFactory.GetFileLogger();

        /// <summary>
        /// Контейнер зависимостей
        /// </summary>
        private readonly IConvertingFileData _convertingFileData;

        /// <summary>
        /// Параметры приложения
        /// </summary>
        private readonly IProjectSettings _projectSettings;

        /// <summary>
        /// Фабрика для создания сервисов WCF
        /// </summary>
        private readonly ConvertingServerServiceFactory _convertingServerServiceFactory;

        /// <summary>
        /// Конвертер из трансферной модели в серверную
        /// </summary>     
        private readonly IConverterServerPackageDataFromDto _converterServerPackageDataFromDto;

        /// <summary>
        /// Конвертер из серверной модели в трансферную
        /// </summary>
        private readonly IConverterServerPackageDataToDto _converterServerPackageDataToDto;

        /// <summary>
        /// Класс для отображения изменений и логгирования
        /// </summary>
        private readonly IMessagingService _messagingService;

        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        /// <summary>
        /// Идентификатор конвертируемого пакета
        /// </summary>
        private Guid? _idPackage;

        /// <summary>
        /// Запуск процесса конвертирования
        /// </summary>
        private readonly CompositeDisposable _convertingUpdaterSubscriptions;

        /// <summary>
        /// Запущен ли процесс конвертации
        /// </summary>
        private bool IsConverting { get; set; }

        /// <summary>
        /// Запустить процесс конвертирования
        /// </summary>
        [Logger]
        public void StartConverting()
        {
            if (!CheckSignatures()) return;

            _messagingService.ShowMessage("Запуск процесса конвертирования...");
            ConvertingTimeActions.KillPreviousRunProcesses();

            SubscribeToDataBase();
        }

        /// <summary>
        /// Проверить наличие базы подписей
        /// </summary>
        private bool CheckSignatures() =>
            _projectSettings.ConvertingResources.
            Map(resources => resources.SignatureNames.OkStatus && resources.SignatureNames.Value.Count > 0).
            WhereBad(hasSignatures => hasSignatures,
                badFunc: hasSignatures => hasSignatures.
                         Void(_ => _messagingService.ShowAndLogError(new ErrorCommon(ErrorConvertingType.SignatureNotFound,
                                                                                     "База подписей не загружена. Отмена запуска"))));

        /// <summary>
        /// Подписаться на обновление пакетов из базы данных
        /// </summary>
        private void SubscribeToDataBase() =>
            _convertingUpdaterSubscriptions.
            Add(Observable.
                Interval(TimeSpan.FromSeconds(ProjectSettings.IntervalSecondsToServer)).
                Where(_ => !IsConverting).
                Select(_ => Observable.FromAsync(() => ExecuteAndHandleErrorAsync(ConvertingFirstInQueuePackage,
                                                                                  beforeMethod: StartConvertingFile,
                                                                                  finallyMethod: StopConvertingFile))).
                Concat().
                Subscribe());

        /// <summary>
        /// Отметки при запуске конвертирования файла
        /// </summary>
        private void StartConvertingFile() =>
            IsConverting = true;
        

        /// <summary>
        /// Отметки при завершении конвертирования файла
        /// </summary>
        private void StopConvertingFile() =>
            IsConverting = false;
        

        /// <summary>
        /// Получить пакет на конвертирование и запустить процесс
        /// </summary>
        [Logger]
        private async Task ConvertingFirstInQueuePackage()
        {
            _messagingService.ShowMessage("Запрос пакета в базе...");

            var packageResult = await _convertingServerServiceFactory.
                                UsingServiceRetry(service => service.Operations.GetFirstInQueuePackage(ProjectSettings.NetworkName));
            if (packageResult.OkStatus && !packageResult.Value.IsEmptyPackage())
            {
                var filesDataServer = await _converterServerPackageDataFromDto.ToFilesDataServerAndSaveFile(packageResult.Value);
                _idPackage = filesDataServer.Id;
                await ConvertingPackage(filesDataServer);
            }
            else if (packageResult.OkStatus && packageResult.Value.IsEmptyPackage())
            {
                await ActionsInEmptyQueue();
            }
        }

        /// <summary>
        /// Конвертировать пакет
        /// </summary>
        private async Task ConvertingPackage(IPackageServer packageServer) =>
            await packageServer.
            WhereContinueAsyncBind(package => package.IsValid,
                okFunc: package => package.
                                   Void(_ => _messagingService.ShowMessage($"Конвертация пакета {package.Id}")).
                                   Void(_ => _loggerService.LogByObject(LoggerLevel.Info, LoggerAction.Operation,
                                                                        ReflectionInfo.GetMethodBase(this), packageServer.Id.ToString())).
                                   Void(_ => ConvertingTimeActions.KillPreviousRunProcesses()).
                                   Map(_ => ConvertingFilesData(package)).
                                   MapAsync(ReplyPackageIsComplete),
                badFunc: package => Task.FromResult(ReplyPackageIsInvalid(package))).
            VoidAsync(_ => _convertingFileData.CloseApplication()).
            VoidBindAsync(SendResponse);

        /// <summary>
        /// Сообщить о пустом/некорректном пакете
        /// </summary>
        private IPackageServer ReplyPackageIsInvalid(IPackageServer packageServer)
        {
            var error = packageServer switch
            {
                _ when !packageServer.IsFilesDataValid => new ErrorCommon(ErrorConvertingType.FileNotFound, "Файлы для конвертации не обнаружены"),
                _ when !packageServer.IsValidByAttemptingCount => new ErrorCommon(ErrorConvertingType.AttemptingCount, "Превышено количество попыток конвертирования пакета"),
                _ => throw new ArgumentOutOfRangeException(nameof(packageServer))
            };

            _messagingService.ShowAndLogError(error);

            return packageServer.SetErrorToAllFiles(error).
                   SetStatusProcessingProject(StatusProcessingProject.Error);
        }

        /// <summary>
        /// Сообщить об обработанном пакете, если процесс не был прерван
        /// </summary>
        private IPackageServer ReplyPackageIsComplete(IPackageServer packageServer) =>
            packageServer.SetStatusProcessingProject(StatusProcessingProject.ConvertingComplete).
            Void(_ => _loggerService.LogByObject(LoggerLevel.Info, LoggerAction.Reply,
                                                 ReflectionInfo.GetMethodBase(this), packageServer.Id.ToString()));

        /// <summary>
        /// Отправить промежуточный отчет
        /// </summary>
        private async Task<IResultValue<IPackageServer>> SendIntermediateResponse(IPackageServer packageServer, IFileDataServer fileDataServer) =>
            await _converterServerPackageDataToDto.FileDataToResponse(fileDataServer).
            Void(_ => _messagingService.ShowMessage("Отправка данных в базу...")).
            MapBindAsync(fileDataRequest => _convertingServerServiceFactory.
                                            UsingServiceRetry(service => service.Operations.UpdateFromIntermediateResponse(packageServer.Id, fileDataRequest))).
            ResultVoidBadAsync(errors => _loggerService.ErrorsLog(errors)).
            ResultVoidBadAsync(errors => _messagingService.ShowErrors(errors)).
            ResultValueOkAsync(packageServer.SetStatusProcessingProject);

        /// <summary>
        /// Отправить окончательный ответ
        /// </summary>
        private async Task SendResponse(IPackageServer packageServer)
        {
            switch (packageServer.StatusProcessingProject)
            {
                case StatusProcessingProject.ConvertingComplete:
                case StatusProcessingProject.Error:
                    {
                        _messagingService.ShowMessage("Отправка данных пакета в базу...");

                        var packageDataShortResponse = _converterServerPackageDataToDto.FilesDataToShortResponse(packageServer);
                        var result = await _convertingServerServiceFactory.UsingServiceRetry(service => service.Operations.UpdateFromResponse(packageDataShortResponse));
                        if (result.OkStatus)
                        {
                            _messagingService.ShowMessage("Конвертация пакета закончена");
                        }
                        else
                        {
                            _loggerService.ErrorsLog(result.Errors);
                            _messagingService.ShowErrors(result.Errors);
                        }
                       
                        _loggerService.LogByObject(LoggerLevel.Info, LoggerAction.Upload, ReflectionInfo.GetMethodBase(this), packageServer.Id.ToString());
                        break;
                    }
                case StatusProcessingProject.Abort:
                    _messagingService.ShowMessage("Конвертация пакета прервана");
                    _loggerService.InfoLog("Abort converting by user");
                    break;
            }

            _idPackage = null;
        }

        /// <summary>
        /// Отмена конвертирования
        /// </summary>
        [Logger]
        private async Task AbortConverting()
        {
            _messagingService.ShowMessage("Отмена выполнения конвертирования");
            if (_idPackage.HasValue)
            {
                await _convertingServerServiceFactory.UsingServiceRetry(service => service.Operations.AbortConvertingById(_idPackage.Value));
            }
        }

        /// <summary>
        /// Конвертировать пакет
        /// </summary>
        private async Task<IPackageServer> ConvertingFilesData(IPackageServer packageServer) =>
            await packageServer.FilesDataServer.
            FirstOrDefault(fileData => !packageServer.IsCompleted && !fileData.IsCompleted).
            Map(fileData => new ResultValue<IFileDataServer>(fileData, new ErrorCommon(ErrorConvertingType.ArgumentNullReference, nameof(IFileDataServer)))).
            ResultOkBad(
                okFunc: fileData => ConvertingByCountLimit(fileData, packageServer.ConvertingSettings).
                                    Map(fileDataConvert => packageServer.ChangeFileDataServer(fileDataConvert).
                                                           Map(packageChanged => SendIntermediateResponse(packageChanged, fileDataConvert))).
                                    ResultValueOkAsync(ConvertingFilesData).
                                    MapAsync(result => result.OkStatus ? result.Value : packageServer),
                badFunc: _ => Task.FromResult(packageServer)).
           Value;

        /// <summary>
        /// Конвертировать файл до превышения лимита
        /// </summary>       
        private IFileDataServer ConvertingByCountLimit(IFileDataServer fileDataServer, IConvertingSettings convertingSettings) =>
            fileDataServer.
            Void(_ => _loggerService.LogByObject(LoggerLevel.Info, LoggerAction.Operation, ReflectionInfo.GetMethodBase(this), fileDataServer.FileNameServer)).
            WhereOk(fileData => !fileData.IsCompleted,
                okFunc: fileData =>
                        ExecuteBindResultValue(() => _convertingFileData.Converting(fileData, convertingSettings)).
                        ResultValueBad(errors => fileData.IsValidByAttemptingCount
                                                 ? fileData.SetAttemptingCount(fileData.AttemptingConvertCount + 1).
                                                   Void(_ => _convertingFileData.CloseApplication()).
                                                   Map(fileDataUncompleted => ConvertingByCountLimit(fileDataUncompleted, convertingSettings))
                                                 : new FileDataServer(fileData, StatusProcessing.ConvertingComplete, errors)).
                        Value);

        /// <summary>
        /// Сообщить об отсутствии пакетов на конвертирование
        /// </summary>
        [Logger]
        private async Task QueueIsEmpty()
        {
            await Task.Delay(500);
            _messagingService.ShowMessage("Очередь пакетов пуста...");
        }

        /// <summary>
        /// Системные действия во время пустой очереди
        /// </summary>       
        private async Task ActionsInEmptyQueue()
        {
            await ConvertingTimeActions.SignaturesOnDataBase(_projectSettings, _messagingService, _loggerService);
            await ConvertingTimeActions.CheckAndDeleteUnusedPackagesOnDataBase(_convertingServerServiceFactory, _messagingService, _loggerService);
            await ConvertingTimeActions.DeleteAllUnusedDataOnDisk(_fileSystemOperations, _messagingService, _loggerService);
            await QueueIsEmpty();
        }

        #region IDisposable Support
        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            if (disposing)
            {
                _convertingUpdaterSubscriptions?.Dispose();
            }
            AbortConverting().ConfigureAwait(false);
            _convertingServerServiceFactory?.Dispose();

            _disposedValue = true;
        }

        ~ConvertingService()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
