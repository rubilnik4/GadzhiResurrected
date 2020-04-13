using ChannelAdam.ServiceModel;
using GadzhiCommon.Enums.FilesConvert;
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

        /// <summary>
        /// Запуск процесса конвертирования
        /// </summary>
        private readonly CompositeDisposable _convertingUpdaterSubsriptions;

        /// <summary>
        /// Идентефикатор конвертируемого пакета
        /// </summary>
        private Guid? _idPackage;

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
        /// Запущен ли процесс конвертации
        /// </summary>
        private bool IsConverting { get; set; }

        /// <summary>
        /// Запустить процесс конвертирования
        /// </summary>      
        public void StartConverting()
        {
            _messagingService.ShowAndLogMessage("Запуск процесса конвертирования...");

            _convertingUpdaterSubsriptions.Add(
                Observable.Interval(TimeSpan.FromSeconds(_projectSettings.IntervalSecondsToServer)).
                           Where(_ => !IsConverting).
                           Subscribe(async _ =>
                                     await ExecuteAndHandleErrorAsync(ConvertingFirstInQueuePackage,
                                                                      applicationBeforeMethod: () => IsConverting = true,
                                                                      applicationFinallyMethod: () => IsConverting = false)));
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
                IFilesDataServer filesDataServer = await _converterServerFilesDataFromDTO.ConvertToFilesDataServerAndSaveFile(filesDataRequest);
                _idPackage = filesDataServer.Id;

                await ConvertingPackage(filesDataServer);
            }
            else
            {
                await CheckAndDeleteUnusedPackagesOnDataBase();
                await DeleteAllUnusedCovertedDataOnDisk();

                await QueueIsEmpty();
            }
        }

        /// <summary>
        /// Конвертировать пакет
        /// </summary>
        private async Task ConvertingPackage(IFilesDataServer filesDataServer)
        {
            if (filesDataServer.IsValid)
            {
                filesDataServer = await ConvertingFilesData(filesDataServer);
            }
            else
            {
                ReplyPackageIsInvalid(filesDataServer);
            }

            await SendResponse(filesDataServer);
        }

        #region Response
        /// <summary>
        /// Сообщить о пустом/некорректном пакете
        /// </summary>
        private void ReplyPackageIsInvalid(IFilesDataServer filesDataServer)
        {
            if (!filesDataServer.IsValidByFileDatas)
            {
                _messagingService.ShowAndLogError(new ErrorCommon(FileConvertErrorType.FileNotFound,
                                                          "Файлы для конвертации не обнаружены"));
            }
            else if (!filesDataServer.IsValidByAttemptingCount)
            {
                _messagingService.ShowAndLogError(new ErrorCommon(FileConvertErrorType.AttemptingCount,
                                                          "Превышено количество попыток конвертирования пакета"));
            }
            filesDataServer.SetErrorToAllUncompletedFiles();
            filesDataServer.StatusProcessingProject = StatusProcessingProject.Error;
        }

        /// <summary>
        /// Сообщить об отконвертированном пакете, если процесс не был прерван
        /// </summary>
        private void ReplyPackageIsComplete(IFilesDataServer filesDataServer)
        {
            if (!filesDataServer.IsCompleted)//если пользователь не прервал процесс
            {
                filesDataServer.StatusProcessingProject = StatusProcessingProject.ConvertingComplete;
            }
        }

        /// <summary>
        /// Отправить промежуточный отчет
        /// </summary>
        private async Task SendIntermediateResponse(IFilesDataServer filesDataServer)
        {
            FilesDataIntermediateResponseServer filesDataIntermediateResponse =
                _converterServerFilesDataToDTO.ConvertFilesToIntermediateResponse(filesDataServer);

            filesDataServer.StatusProcessingProject = await _fileConvertingServerService.Operations.
                                                      UpdateFromIntermediateResponse(filesDataIntermediateResponse);
        }

        /// <summary>
        /// Отправить окончательный ответ
        /// </summary>
        private async Task SendResponse(IFilesDataServer filesDataServer)
        {
            if (filesDataServer.StatusProcessingProject == StatusProcessingProject.ConvertingComplete ||
                filesDataServer.StatusProcessingProject == StatusProcessingProject.Error)
            {
                _messagingService.ShowAndLogMessage($"Отправка данных в базу...");

                FilesDataResponseServer filesDataResponse = await _converterServerFilesDataToDTO.ConvertFilesToResponse(filesDataServer);
                await _fileConvertingServerService.Operations.UpdateFromResponse(filesDataResponse);

                _messagingService.ShowAndLogMessage($"Конвертация пакета закончена");
            }
            else if (filesDataServer.StatusProcessingProject == StatusProcessingProject.Abort) //в случае если пользователь отменил конвертацию
            {
                _messagingService.ShowAndLogMessage($"Конвертация пакета прервана");
            }
        }

        private async Task AbortConverting(bool isDispose)
        {
            _messagingService.ShowAndLogMessage($"Отмена выполнения конвертирования");

            if (_idPackage.HasValue)
            {
                await _fileConvertingServerService?.Operations.AbortConvertingById(_idPackage.Value);
            }
            if (isDispose)
            {
                _fileConvertingServerService?.Dispose();
            }
        }
        #endregion

        /// <summary>
        /// Конвертировать пакет
        /// </summary>
        private async Task<IFilesDataServer> ConvertingFilesData(IFilesDataServer filesDataServer)
        {
            _messagingService.ShowAndLogMessage($"Конвертация пакета {filesDataServer.Id}");

            foreach (var fileData in filesDataServer.FileDatasServerConverting)
            {
                if (!filesDataServer.IsCompleted) //если пользователь не прервал процесс
                {
                    while (!filesDataServer.IsCompleted && !fileData.IsCompleted && fileData.IsValidByAttemptingCount)
                    {
                        await ExecuteAndHandleErrorAsync(() => _convertingFileData.Converting(fileData),
                                                         () => fileData.AttemptingConvertCount += 1);
                    }

                    await SendIntermediateResponse(filesDataServer);
                }
            }
            ReplyPackageIsComplete(filesDataServer);

            return filesDataServer;
        }

        /// <summary>
        /// Сообщить об отсутсвии пакетов на конвертирование
        /// </summary>
        private async Task QueueIsEmpty()
        {
            await Task.Delay(500);
            _messagingService.ShowAndLogMessage("Очередь пакетов пуста...");
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
                AbortConverting(true).ConfigureAwait(false);

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
