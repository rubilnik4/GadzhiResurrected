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
using static GadzhiCommon.Infrastructure.Implementations.ExecuteAndCatchErrors;
using static GadzhiCommon.Extentions.Functional.ExecuteTaskHandler;
using GadzhiCommon.Extentions.Functional.Result;
using System.Linq;

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
        private readonly IConverterServerFilesDataFromDTO _converterServerFilesDataFromDTO;

        /// <summary>
        /// Конвертер из серверной модели в трансферную
        /// </summary>
        private readonly IConverterServerFilesDataToDTO _converterServerFilesDataToDTO;

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
                                 IConverterServerFilesDataFromDTO converterServerFilesDataFromDTO,
                                 IConverterServerFilesDataToDTO converterServerFilesDataToDTO,
                                 IMessagingService messagingService,
                                 IFileSystemOperations fileSystemOperations)
        {
            _convertingFileData = convertingFileData;
            _projectSettings = projectSettings;
            _fileConvertingServerService = fileConvertingServerService;
            _converterServerFilesDataFromDTO = converterServerFilesDataFromDTO;
            _converterServerFilesDataToDTO = converterServerFilesDataToDTO;
            _messagingService = messagingService;
            _fileSystemOperations = fileSystemOperations;

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

            var subcribe = Observable.Interval(TimeSpan.FromSeconds(_projectSettings.IntervalSecondsToServer)).
                           Where(_ => !IsConverting).
                           Subscribe(async _ => await ExecuteAndHandleErrorAsync(ConvertingFirstInQueuePackage,
                                                                      applicationBeforeMethod: () => IsConverting = true,
                                                                      applicationFinallyMethod: () => IsConverting = false));
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
                var filesDataServer = await _converterServerFilesDataFromDTO.ConvertToFilesDataServerAndSaveFile(filesDataRequest);
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
                                    MapAsync(fileDataConverted => ReplyPackageIsComplete(fileDataConverted)),
                badFunc: fileData => Task.FromResult(ReplyPackageIsInvalid(filesDataServer))).
            VoidAsync(async fileData => await SendResponse(fileData));

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
            filesDataServer.
            WhereOk(fileData => fileData.IsCompleted,
                okFunc: fileData => fileData.SetStatusProcessingProject(StatusProcessingProject.ConvertingComplete));

        /// <summary>
        /// Отправить промежуточный отчет
        /// </summary>
        private async Task<IFilesDataServer> SendIntermediateResponse(IFilesDataServer filesDataServer) =>
            await _converterServerFilesDataToDTO.ConvertFilesToIntermediateResponse(filesDataServer).
            MapAsync(feliDataRequest => _fileConvertingServerService.Operations.UpdateFromIntermediateResponse(feliDataRequest)).
            MapAsync(statusProcessingProject => filesDataServer.SetStatusProcessingProject(statusProcessingProject));

        /// <summary>
        /// Отправить окончательный ответ
        /// </summary>
        private async Task SendResponse(IFilesDataServer filesDataServer)
        {
            if (filesDataServer.StatusProcessingProject == StatusProcessingProject.ConvertingComplete ||
                filesDataServer.StatusProcessingProject == StatusProcessingProject.Error)
            {
                _messagingService.ShowAndLogMessage($"Отправка данных в базу...");

                var filesDataResponse = await _converterServerFilesDataToDTO.ConvertFilesToResponse(filesDataServer);
                await _fileConvertingServerService.Operations.UpdateFromResponse(filesDataResponse);

                _messagingService.ShowAndLogMessage($"Конвертация пакета закончена");
            }
            else if (filesDataServer.StatusProcessingProject == StatusProcessingProject.Abort) //в случае если пользователь отменил конвертацию
            {
                _messagingService.ShowAndLogMessage($"Конвертация пакета прервана");
            }
        }

        /// <summary>
        /// Отмена конвертирования
        /// </summary>      
        private async Task AbortConverting()
        {
            _messagingService.ShowAndLogMessage($"Отмена выполнения конвертирования");
            if (_idPackage.HasValue)
            {
                await _fileConvertingServerService?.Operations.AbortConvertingById(_idPackage.Value);
            }
        }

        /// <summary>
        /// Конвертировать пакет
        /// </summary>
        private async Task<IFilesDataServer> ConvertingFilesData(IFilesDataServer filesDataServer) =>
            await filesDataServer.FileDatasServer.
            Where(fileData => !filesDataServer.IsCompleted && !fileData.IsCompleted).
            FirstOrDefault().
            Map(fileData => new ResultValue<IFileDataServer>(fileData)).
            ResultValueOk(fileData => ConvertingByCountLimit(fileData).
                                      MapAsync(fileDataConverted => filesDataServer.ChangeFileDataServer(fileDataConverted)).
                                      BindAsync(filesDataChanged => SendIntermediateResponse(filesDataChanged)).
                                      BindAsync(filesDataSent => ConvertingFilesData(filesDataSent))).          
            Value;
       
        /// <summary>
        /// Конвертировать файл до превышения лимита
        /// </summary>       
        private async Task<IFileDataServer> ConvertingByCountLimit(IFileDataServer fileDataServer) =>
            await fileDataServer.WhereOkAsync(fileData => !fileData.IsCompleted,
                okFunc: fileData =>
                        ExecuteBindResultValueAsync(() => _convertingFileData.Converting(fileData)).
                        ResultValueBad(fileDataTask => new Task<IFileDataServer>(() => fileData.SetAttemtingCount(fileData.AttemptingConvertCount))).
                        ResultValueBad(fileDataTask => fileDataTask.VoidAsync(async fileDataVoid => await ConvertingByCountLimit(fileDataVoid))).
                        Value);

        /// <summary>
        /// Сообщить об отсутсвии пакетов на конвертирование
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
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _convertingUpdaterSubsriptions?.Dispose();
                }
                AbortConverting().ConfigureAwait(false);
                _fileConvertingServerService?.Dispose();

                disposedValue = true;
            }
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
