using ChannelAdam.ServiceModel;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces.Converters;
using GadzhiConverting.Models.Implementations.FilesConvert;
using GadzhiDTOServer.Contracts.FilesConvert;
using GadzhiDTOServer.TransferModels.FilesConvert;
using System;
using System.Threading.Tasks;

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
        private readonly IMessageAndLoggingService _messageAndLoggingService;

        /// <summary>
        /// Класс обертка для отлова ошибок
        /// </summary> 
        private readonly IExecuteAndCatchErrors _executeAndCatchErrors;

        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        /// <summary>
        /// Идентефикатор конвертируемого пакета
        /// </summary>
        private Guid? _idPackage;

        public ConvertingService(IConvertingFileData convertingFileData,
                                 IProjectSettings projectSettings,
                                 IServiceConsumer<IFileConvertingServerService> fileConvertingServerService,
                                 IConverterServerFilesDataFromDTO converterServerFilesDataFromDTO,
                                 IConverterServerFilesDataToDTO converterServerFilesDataToDTO,
                                 IMessageAndLoggingService messageAndLoggingService,
                                 IExecuteAndCatchErrors executeAndCatchErrors,
                                 IFileSystemOperations fileSystemOperations)
        {
            _convertingFileData = convertingFileData;
            _projectSettings = projectSettings;
            _fileConvertingServerService = fileConvertingServerService;
            _converterServerFilesDataFromDTO = converterServerFilesDataFromDTO;
            _converterServerFilesDataToDTO = converterServerFilesDataToDTO;
            _messageAndLoggingService = messageAndLoggingService;
            _executeAndCatchErrors = executeAndCatchErrors;
            _fileSystemOperations = fileSystemOperations;

            _idPackage = null;
        }

        /// <summary>
        /// Получить пакет на конвертирование и запустить процесс
        /// </summary>        
        public async Task ConvertingFirstInQueuePackage()
        {
            _messageAndLoggingService.ShowMessage("Запрос пакета в базе...");

            FilesDataRequestServer filesDataRequest = await _fileConvertingServerService.Operations.
                                                             GetFirstInQueuePackage(_projectSettings.NetworkName);
            if (filesDataRequest != null)
            {
                FilesDataServer filesDataServer = await _converterServerFilesDataFromDTO.ConvertToFilesDataServerAndSaveFile(filesDataRequest);
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
        private async Task ConvertingPackage(FilesDataServer filesDataServer)
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
        private void ReplyPackageIsInvalid(FilesDataServer filesDataServer)
        {
            if (!filesDataServer.IsValidByFileDatas)
            {
                _messageAndLoggingService.ShowError(FileConvertErrorType.FileNotFound,
                                                    "Файлы для конвертации не обнаружены");
            }
            else if (!filesDataServer.IsValidByAttemptingCount)
            {
                _messageAndLoggingService.ShowError(FileConvertErrorType.AttemptingCount,
                                                    "Превышено количество попыток конвертирования пакета");
            }
            filesDataServer.SetErrorToAllUncompletedFiles();
            filesDataServer.StatusProcessingProject = StatusProcessingProject.Error;
        }

        /// <summary>
        /// Сообщить об отконвертированном пакете, если процесс не был прерван
        /// </summary>
        private void ReplyPackageIsComplete(FilesDataServer filesDataServer)
        {
            if (!filesDataServer.IsCompleted)//если пользователь не прервал процесс
            {
                filesDataServer.StatusProcessingProject = StatusProcessingProject.ConvertingComplete;
            }
        }

        /// <summary>
        /// Отправить промежуточный отчет
        /// </summary>
        private async Task SendIntermediateResponse(FilesDataServer filesDataServer)
        {
            FilesDataIntermediateResponseServer filesDataIntermediateResponse =
                _converterServerFilesDataToDTO.ConvertFilesToIntermediateResponse(filesDataServer);

            filesDataServer.StatusProcessingProject = await _fileConvertingServerService.Operations.
                                                      UpdateFromIntermediateResponse(filesDataIntermediateResponse);
        }

        /// <summary>
        /// Отправить окончательный ответ
        /// </summary>
        private async Task SendResponse(FilesDataServer filesDataServer)
        {
            if (filesDataServer.StatusProcessingProject == StatusProcessingProject.ConvertingComplete)
            {
                _messageAndLoggingService.ShowMessage($"Отправка данных в базу...");

                FilesDataResponseServer filesDataResponse = await _converterServerFilesDataToDTO.ConvertFilesToResponse(filesDataServer);
                await _fileConvertingServerService.Operations.UpdateFromResponse(filesDataResponse);

                _messageAndLoggingService.ShowMessage($"Конвертация пакета закончена");
            }
            else //в случае если пользователь отменил конвертацию
            {
                _messageAndLoggingService.ShowMessage($"Конвертация пакета прервана");
            }
        }

        private async Task AbortConverting(bool isDispose)
        {
            _messageAndLoggingService.ShowMessage($"Отмена выполнения конвертирования");

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
        private async Task<FilesDataServer> ConvertingFilesData(FilesDataServer filesDataServer)
        {
            _messageAndLoggingService.ShowMessage($"Конвертация пакета {filesDataServer.Id.ToString()}");

            foreach (var fileData in filesDataServer.FileDatas)
            {
                if (!filesDataServer.IsCompleted) //если пользователь не прервал процесс
                {
                    while (!filesDataServer.IsCompleted && !fileData.IsCompleted && fileData.IsValidByAttemptingCount)
                    {
                        await _executeAndCatchErrors.ExecuteAndHandleErrorAsync(() => _convertingFileData.Converting(fileData),
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
            _messageAndLoggingService.ShowMessage("Очередь пакетов пуста...");
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
                _messageAndLoggingService.ShowMessage("Очистка неиспользуемых пакетов...");
                await _fileConvertingServerService.Operations.DeleteAllUnusedPackagesUntilDate(dateTimeNow);
                Properties.Settings.Default.UnusedDataCheck = new TimeSpan(dateTimeNow.Ticks);               
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
                _messageAndLoggingService.ShowMessage("Очистка пространства на жестком диске...");
                await Task.Run(() => _fileSystemOperations.DeleteAllDataInDirectory(_projectSettings.ConvertingDirectory));
                Properties.Settings.Default.ConvertingDataFolderCheck = new TimeSpan(dateTimeNow.Ticks);
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
