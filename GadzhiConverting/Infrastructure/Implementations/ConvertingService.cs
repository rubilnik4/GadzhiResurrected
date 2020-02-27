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
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

        /// <summary>
        /// Класс обертка для отлова ошибок
        /// </summary> 
        private readonly IExecuteAndCatchErrors _executeAndCatchErrors;

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
                                 IFileSystemOperations fileSystemOperations,
                                 IExecuteAndCatchErrors executeAndCatchErrors)
        {
            _convertingFileData = convertingFileData;
            _projectSettings = projectSettings;
            _fileConvertingServerService = fileConvertingServerService;
            _converterServerFilesDataFromDTO = converterServerFilesDataFromDTO;
            _converterServerFilesDataToDTO = converterServerFilesDataToDTO;
            _messageAndLoggingService = messageAndLoggingService;
            _fileSystemOperations = fileSystemOperations;
            _executeAndCatchErrors = executeAndCatchErrors;

            _idPackage = null;
        }

        /// <summary>
        /// Получить пакет на конвертирование и запустить процесс
        /// </summary>        
        public async Task ConvertingFirstInQueuePackage()
        {
            _messageAndLoggingService.ShowMessage("Запрос пакета в базе...");

            FilesDataRequestServer filesDataRequest = await _fileConvertingServerService.
                                                             Operations.
                                                             GetFirstInQueuePackage(_projectSettings.NetworkName);
            if (filesDataRequest != null)
            {
                FilesDataServer filesDataServer = await _converterServerFilesDataFromDTO.ConvertToFilesDataServerAndSaveFile(filesDataRequest);
                _idPackage = filesDataServer.Id;

                await ConvertingPackage(filesDataServer);
            }
            else
            {
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

                ReplyPackageIsComplete(filesDataServer);
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
            if (!filesDataServer.IsValidByFileData)
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
            filesDataServer.IsCompleted = true;
            filesDataServer.StatusProcessingProject = StatusProcessingProject.Error;
        }

        /// <summary>
        /// Сообщить об отконвертированном пакете
        /// </summary>
        private void ReplyPackageIsComplete(FilesDataServer filesDataServer)
        {
            filesDataServer.StatusProcessingProject = StatusProcessingProject.Receiving;
            filesDataServer.IsCompleted = true;
        }

        /// <summary>
        /// Отправить промежуточный отчет
        /// </summary>
        private async Task SendIntermediateResponse(FilesDataServer filesDataServer)
        {
            FilesDataIntermediateResponseServer filesDataIntermediateResponse =
                _converterServerFilesDataToDTO.ConvertFilesToIntermediateResponse(filesDataServer);

            await _fileConvertingServerService.
                   Operations.
                   UpdateFromIntermediateResponse(filesDataIntermediateResponse);
        }

        /// <summary>
        /// Отправить окончательный ответ
        /// </summary>
        private async Task SendResponse(FilesDataServer filesDataServer)
        {
            _messageAndLoggingService.ShowMessage($"Отправка данных в базу...");

            FilesDataResponseServer filesDataResponse =
                await _converterServerFilesDataToDTO.ConvertFilesToResponse(filesDataServer);

            await _fileConvertingServerService.
                   Operations.
                   UpdateFromResponse(filesDataResponse);

            _messageAndLoggingService.ShowMessage($"Конвертация пакета закончена");
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
            _messageAndLoggingService.ShowMessage("\n" + $"Конвертация пакета {filesDataServer.Id.ToString()}");

            foreach (var fileData in filesDataServer.FilesDataInfo)
            {
                while (!fileData.IsCompleted && fileData.IsValidByAttemptingCount)
                {
                    await _executeAndCatchErrors.ExecuteAndHandleErrorAsync(() => _convertingFileData.Converting(fileData),
                                                                            () => fileData.AttemptingConvertCount += 1);
                }

                await SendIntermediateResponse(filesDataServer);
            }
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
