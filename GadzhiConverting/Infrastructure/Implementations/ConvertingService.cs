using ChannelAdam.ServiceModel;
using ConvertingModels.Models.Interfaces.FilesConvert;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extentions.Functional;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces.Converters;
using GadzhiConverting.Models.Implementations.FilesConvert;
using GadzhiConverting.Models.Interfaces.FilesConvert;
using GadzhiDTOServer.Contracts.FilesConvert;
using GadzhiDTOServer.TransferModels.FilesConvert;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Extentions.Functional.Result;
using System.Linq;
using System.Diagnostics;
using GadzhiCommon.Extentions.StringAdditional;
using static GadzhiCommon.Infrastructure.Implementations.ExecuteAndCatchErrors;
using static GadzhiCommon.Extentions.Functional.ExecuteTaskHandler;

namespace GadzhiConverting.Infrastructure.Implementations
{
    /// <summary>
    /// Запуск процесса конвертирования
    /// </summary>
    public class ConvertingService : IConvertingService
    {
        /// <summary>
        ///Контейнер зависимостей
        /// </summary>
        private readonly IConvertingFileData _convertingFileData;

        /// <summary>
        /// Параметры приложения
        /// </summary>
        private readonly IProjectSettings _projectSettings;

        /// <summary>
        /// Сервис для добавления и получения данных о конвертируемых пакетах в серверной части
        /// </summary>     
        private readonly IServiceConsumer<IFileConvertingServerService> _fileConvertingServerService;

        /// <summary>
        /// Конвертер из трансферной модели в серверную
        /// </summary>     
        private readonly IConverterServerFilesDataFromDTO _converterServerFilesDataFromDto;

        /// <summary>
        /// Конвертер из серверной модели в трансферную
        /// </summary>
        private readonly IConverterServerFilesDataToDTO _converterServerFilesDataToDto;

        /// <summary>
        /// Класс для отображения изменений и логгирования
        /// </summary>
        private readonly IMessagingService _messagingService;

        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        public ConvertingService(IConvertingFileData convertingFileData,
                                 IProjectSettings projectSettings,
                                 IServiceConsumer<IFileConvertingServerService> fileConvertingServerService,
                                 IConverterServerFilesDataFromDTO converterServerFilesDataFromDto,
                                 IConverterServerFilesDataToDTO converterServerFilesDataToDto,
                                 IMessagingService messagingService,
                                 IFileSystemOperations fileSystemOperations)
        {
            _convertingFileData = convertingFileData ?? throw new ArgumentNullException(nameof(convertingFileData));
            _projectSettings = projectSettings ?? throw new ArgumentNullException(nameof(projectSettings));
            _fileConvertingServerService = fileConvertingServerService ?? throw new ArgumentNullException(nameof(fileConvertingServerService));
            _converterServerFilesDataFromDto = converterServerFilesDataFromDto ?? throw new ArgumentNullException(nameof(converterServerFilesDataFromDto));
            _converterServerFilesDataToDto = converterServerFilesDataToDto ?? throw new ArgumentNullException(nameof(converterServerFilesDataToDto));
            _messagingService = messagingService ?? throw new ArgumentNullException(nameof(messagingService));
            _fileSystemOperations = fileSystemOperations ?? throw new ArgumentNullException(nameof(fileSystemOperations));

            _convertingUpdaterSubsriptions = new CompositeDisposable();
            _idPackage = null;
        }

        /// <summary>
        /// Идентефикатор конвертируемого пакета
        /// </summary>
        private Guid? _idPackage;

        /// <summary>
        /// Запуск процесса конвертирования
        /// </summary>
        private readonly CompositeDisposable _convertingUpdaterSubsriptions;

        /// <summary>
        /// Запущен ли процесс конвертации
        /// </summary>
        private bool IsConverting { get; set; }

        /// <summary>
        /// Запустить процесс конвертирования
        /// </summary>      
        public void StartConverting()
        {
            _messagingService.ShowAndLogMessage("Запуск процесса конвертирования...");
            KillPreviousRunProcesses();

            var subcribe = Observable.Interval(TimeSpan.FromSeconds(_projectSettings.IntervalSecondsToServer)).
                           Where(_ => !IsConverting).
                           Subscribe(async _ => await ExecuteAndHandleErrorAsync(ConvertingFirstInQueuePackage,
                                                                      beforeMethod: () => IsConverting = true,
                                                                      finallyMethod: () => IsConverting = false));
            _convertingUpdaterSubsriptions.Add(subcribe);
        }

        /// <summary>
        /// Получить пакет на конвертирование и запустить процесс
        /// </summary>        
        public async Task ConvertingFirstInQueuePackage()
        {
            _messagingService.ShowAndLogMessage("Запрос пакета в базе...");

            FilesDataRequestServer filesDataRequest = await _fileConvertingServerService.Operations.
                                                             GetFirstInQueuePackage(_projectSettings.NetworkName);
            if (filesDataRequest != null)
            {
                var filesDataServer = await _converterServerFilesDataFromDto.ConvertToFilesDataServerAndSaveFile(filesDataRequest);
                _idPackage = filesDataServer.Id;
                await ConvertingPackage(filesDataServer);
            }
            else
            {
                await ActionsInEmptyQueue();
            }
        }

        /// <summary>
        /// Конвертировать пакет
        /// </summary>
        private async Task ConvertingPackage(IFilesDataServer filesDataServer) =>
            await filesDataServer.
            WhereContinueAsync(fileData => fileData.IsValid,
                okFunc: fileData => fileData.
                                    Void(_ => _messagingService.ShowAndLogMessage($"Конвертация пакета {fileData.Id}")).
                                    Map(_ => ConvertingFilesData(fileData)).
                                    MapAsync(ReplyPackageIsComplete),
                badFunc: fileData => Task.FromResult(ReplyPackageIsInvalid(filesDataServer))).
            VoidAsync(SendResponse);

        /// <summary>
        /// Сообщить о пустом/некорректном пакете
        /// </summary>
        private IFilesDataServer ReplyPackageIsInvalid(IFilesDataServer filesDataServer)
        {
            if (!filesDataServer.IsValidByFileDatas)
            {
                _messagingService.ShowAndLogError(new ErrorCommon(FileConvertErrorType.FileNotFound, "Файлы для конвертации не обнаружены"));
            }
            if (!filesDataServer.IsValidByAttemptingCount)
            {
                _messagingService.ShowAndLogError(new ErrorCommon(FileConvertErrorType.AttemptingCount, "Превышено количество попыток конвертирования пакета"));
            }
            return filesDataServer.SetErrorToAllFiles().
                   SetStatusProcessingProject(StatusProcessingProject.Error);
        }

        /// <summary>
        /// Сообщить об отконвертированном пакете, если процесс не был прерван
        /// </summary>
        private IFilesDataServer ReplyPackageIsComplete(IFilesDataServer filesDataServer) =>
            filesDataServer.SetStatusProcessingProject(StatusProcessingProject.ConvertingComplete);

        /// <summary>
        /// Отправить промежуточный отчет
        /// </summary>
        private async Task<IFilesDataServer> SendIntermediateResponse(IFilesDataServer filesDataServer) =>
            await _converterServerFilesDataToDto.ConvertFilesToIntermediateResponse(filesDataServer).
            Map(Task.FromResult).
            BindAsync(feliDataRequest => _fileConvertingServerService.Operations.UpdateFromIntermediateResponse(feliDataRequest)).
            MapAsync(filesDataServer.SetStatusProcessingProject);

        /// <summary>
        /// Отправить окончательный ответ
        /// </summary>
        private async Task SendResponse(IFilesDataServer filesDataServer)
        {
            if (filesDataServer.StatusProcessingProject == StatusProcessingProject.ConvertingComplete ||
                filesDataServer.StatusProcessingProject == StatusProcessingProject.Error)
            {
                _messagingService.ShowAndLogMessage("Отправка данных в базу...");

                FilesDataResponseServer filesDataResponse = await _converterServerFilesDataToDto.ConvertFilesToResponse(filesDataServer);
                await _fileConvertingServerService.Operations.UpdateFromResponse(filesDataResponse);

                _messagingService.ShowAndLogMessage("Конвертация пакета закончена");
            }
            else if (filesDataServer.StatusProcessingProject == StatusProcessingProject.Abort) //в случае если пользователь отменил конвертацию
            {
                _messagingService.ShowAndLogMessage("Конвертация пакета прервана");
            }
            _idPackage = null;
        }

        /// <summary>
        /// Отмена конвертирования
        /// </summary>      
        private async Task AbortConverting()
        {
            _messagingService.ShowAndLogMessage("Отмена выполнения конвертирования");
            if (_idPackage.HasValue)
            {
                await _fileConvertingServerService.Operations.AbortConvertingById(_idPackage.Value);
            }
        }

        /// <summary>
        /// Конвертировать пакет
        /// </summary>
        private async Task<IFilesDataServer> ConvertingFilesData(IFilesDataServer filesDataServer) =>
            await filesDataServer.FileDatasServer.
                FirstOrDefault(fileData => !filesDataServer.IsCompleted && !fileData.IsCompleted).
                Map(fileData => new ResultValue<IFileDataServer>(fileData, new ErrorCommon(FileConvertErrorType.ArgumentNullReference, nameof(IFileDataServer)))).
                ResultOkBad(
                    okFunc: fileData => ConvertingByCountLimit(fileData).
                                        MapAsync(filesDataServer.ChangeFileDataServer).
                                        BindAsync(SendIntermediateResponse).
                                        BindAsync(ConvertingFilesData),
                    badFunc: _ => Task.FromResult(filesDataServer)).
                Value;

        /// <summary>
        /// Конвертировать файл до превышения лимита
        /// </summary>       
        private async Task<IFileDataServer> ConvertingByCountLimit(IFileDataServer fileDataServer) =>
            await fileDataServer.WhereOkAsync(fileData => !fileData.IsCompleted,
                okFunc: fileData =>
                        ExecuteBindResultValueAsync(() => _convertingFileData.Converting(fileData)).
                        ResultValueBad(fileDataTask => Task.FromResult(fileData.SetAttemtingCount(fileData.AttemptingConvertCount + 1))).
                        ResultValueBad(fileDataTask => fileDataTask.VoidAsync(async fileDataVoid => await ConvertingByCountLimit(fileDataVoid))).
                        Value);

        /// <summary>
        /// Сообщить об отсутствии пакетов на конвертирование
        /// </summary>
        private async Task QueueIsEmpty()
        {
            await Task.Delay(500);
            _messagingService.ShowAndLogMessage("Очередь пакетов пуста...");
        }

        /// <summary>
        /// Системные действия во время пустой очереди
        /// </summary>       
        private async Task ActionsInEmptyQueue()
        {
            await CheckAndDeleteUnusedPackagesOnDataBase();
            await DeleteAllUnusedCovertedDataOnDisk();
            await QueueIsEmpty();
        }

        /// <summary>
        /// Удалить все предыдущие запущенные процессы
        /// </summary>
        private static void KillPreviousRunProcesses()
        {
            var processes = Process.GetProcesses().
                                        Where(process => process.ProcessName.ContainsIgnoreCase("ustation") ||
                                                         process.ProcessName.ContainsIgnoreCase("winword"));
            foreach (var process in processes)
            {
                process.Kill();
            }
        }

        /// <summary>
        /// Проверить и удалить ненужные пакеты в базе
        /// </summary>
        private async Task CheckAndDeleteUnusedPackagesOnDataBase()
        {
            DateTime dateTimeNow = DateTime.Now;
            TimeSpan timeElapsed = new TimeSpan((dateTimeNow - Properties.Settings.Default.UnusedDataCheck).Ticks);
            if (timeElapsed.TotalHours > _projectSettings.IntervalHouresToDeleteUnusedPackages)
            {
                _messagingService.ShowAndLogMessage("Очистка неиспользуемых пакетов...");
                await _fileConvertingServerService.Operations.DeleteAllUnusedPackagesUntilDate(dateTimeNow);

                Properties.Settings.Default.UnusedDataCheck = new TimeSpan(dateTimeNow.Ticks);
                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// Очистить папку с отконвертированными файлами на жестком диске
        /// </summary>
        private async Task DeleteAllUnusedCovertedDataOnDisk()
        {
            DateTime dateTimeNow = DateTime.Now;
            TimeSpan timeElapsed = new TimeSpan((dateTimeNow - Properties.Settings.Default.ConvertingDataFolderCheck).Ticks);
            if (timeElapsed.TotalHours > _projectSettings.IntervalHouresToDeleteUnusedPackages)
            {
                _messagingService.ShowAndLogMessage("Очистка пространства на жестком диске...");
                await Task.Run(() => _fileSystemOperations.DeleteAllDataInDirectory(_projectSettings.ConvertingDirectory));

                Properties.Settings.Default.ConvertingDataFolderCheck = new TimeSpan(dateTimeNow.Ticks);
                Properties.Settings.Default.Save();
            }
        }

        #region IDisposable Support
        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            if (disposing)
            {
                _convertingUpdaterSubsriptions?.Dispose();
            }
            AbortConverting().ConfigureAwait(false);
            _fileConvertingServerService?.Dispose();

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
