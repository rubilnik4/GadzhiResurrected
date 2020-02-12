using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces.Converters;
using GadzhiConverting.Models.FilesConvert.Implementations;
using GadzhiDAL.Services.Implementations;
using GadzhiDTO.TransferModels.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Implementations
{
    /// <summary>
    /// Запуск процесса конвертирования
    /// </summary>
    public class ConvertingService : IConvertingService
    {
        /// <summary>
        /// Сервис для добавления и получения данных о конвертируемых пакетах в серверной части
        /// </summary>
        private readonly IFilesDataServiceServer _filesDataServiceServer;

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

        public ConvertingService(IFilesDataServiceServer filesDataServiceServer,
                                 IConverterServerFilesDataFromDTO converterServerFilesDataFromDTO,
                                 IConverterServerFilesDataToDTO converterServerFilesDataToDTO,
                                 IMessageAndLoggingService messageAndLoggingService)
        {
            _filesDataServiceServer = filesDataServiceServer;
            _converterServerFilesDataFromDTO = converterServerFilesDataFromDTO;
            _converterServerFilesDataToDTO = converterServerFilesDataToDTO;
            _messageAndLoggingService = messageAndLoggingService;
        }

        /// <summary>
        /// Получить пакет на конвертирование и запустить процесс
        /// </summary>        
        public async Task ConvertingFirstInQueuePackage()
        {
            _messageAndLoggingService.ShowMessage("Запрос пакета в базе...");

            FilesDataRequest filesDataRequest = await _filesDataServiceServer.GetFirstInQueuePackage();
            if (filesDataRequest != null)
            {
                FilesDataServer filesDataServer = await _converterServerFilesDataFromDTO.ConvertToFilesDataServerAndSaveFile(filesDataRequest);

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
                _messageAndLoggingService.ShowMessage("\n" + $"Конвертация пакета {filesDataServer.Id.ToString()}");

                foreach (var fileData in filesDataServer.FilesDataInfo)
                {
                    await ConvertingFileData(fileData);

                    await SendIntermediateResponse(filesDataServer);
                }

                await SendCompleteResponse(filesDataServer);
            }
            else
            {
                ReplyPackageIsEmpty(filesDataServer);
            }
        }

        #region Response
        /// <summary>
        /// Сообщить о пустом/некорректном пакете
        /// </summary>
        private void ReplyPackageIsEmpty(FilesDataServer filesDataServer)
        {
            _messageAndLoggingService.ShowMessage("Файлы для конвертации не обнаружены");

            filesDataServer.IsCompleted = true;
            filesDataServer.StatusProcessingProject = StatusProcessingProject.Receiving;
        }

        /// <summary>
        /// Отправить промежуточный отчет
        /// </summary>
        private async Task SendIntermediateResponse(FilesDataServer filesDataServer)
        {
            FilesDataIntermediateResponse filesDataIntermediateResponse =
                _converterServerFilesDataToDTO.ConvertFilesToIntermediateResponse(filesDataServer);

            await _filesDataServiceServer.UpdateFromIntermediateResponse(filesDataIntermediateResponse);
        }

        /// <summary>
        /// Отправить окончательный ответ
        /// </summary>
        private async Task SendCompleteResponse(FilesDataServer filesDataServer)
        {
            filesDataServer.StatusProcessingProject = StatusProcessingProject.Receiving;
            filesDataServer.IsCompleted = true;

            _messageAndLoggingService.ShowMessage($"Отправка данных в базу...");

            FilesDataResponse filesDataResponse =
                await _converterServerFilesDataToDTO.ConvertFilesToResponse(filesDataServer);

            await _filesDataServiceServer.UpdateFromResponse(filesDataResponse);

            _messageAndLoggingService.ShowMessage($"Конвертация пакета закончена" + "\n");
        }
        #endregion

        #region converting
        /// <summary>
        /// Конвертировать файл
        /// </summary>
        private async Task ConvertingFileData(FileDataServer fileDataServer)
        {
            fileDataServer = FileDataStartConvering(fileDataServer);

            await Task.Delay(2000);

            fileDataServer = FileDataEndConvering(fileDataServer);
        }

        /// <summary>
        /// Начать конвертирование файла
        /// </summary>
        private FileDataServer FileDataStartConvering(FileDataServer fileDataServer)
        {
            _messageAndLoggingService.ShowMessage($"Конвертация файла {fileDataServer.FileNameWithExtensionClient}");

            fileDataServer.StatusProcessing = StatusProcessing.Converting;

            return fileDataServer;
        }

        /// <summary>
        /// Закончить конвертирование файла
        /// </summary>
        private FileDataServer FileDataEndConvering(FileDataServer fileDataServer)
        {
            fileDataServer.IsCompleted = true;
            if (fileDataServer.IsValid)
            {
                fileDataServer.StatusProcessing = StatusProcessing.Completed;
            }
            else
            {
                fileDataServer.StatusProcessing = StatusProcessing.Error;
            }

            return fileDataServer;
        }
        #endregion

        /// <summary>
        /// Сообщить об отсутсвии пакетов на конвертирование
        /// </summary>
        private async Task QueueIsEmpty()
        {
            await Task.Delay(15500);
            _messageAndLoggingService.ShowMessage("Очередь пакетов пуста..." + "\n");
        }

    }
}
