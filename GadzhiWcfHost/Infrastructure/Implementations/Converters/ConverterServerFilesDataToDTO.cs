using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiWcfHost.Infrastructure.Implementations;
using GadzhiWcfHost.Infrastructure.Implementations.Information;
using GadzhiWcfHost.Infrastructure.Interfaces.Converters;
using GadzhiWcfHost.Models.FilesConvert.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GadzhiWcfHost.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Конвертер из серверной модели в трансферную
    /// </summary>
    public class ConverterServerFilesDataToDTO : IConverterServerFilesDataToDTO
    {
        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private IFileSystemOperations FileSystemOperations { get; }

        /// <summary>
        /// Информация о статусе конвертируемых файлов
        /// </summary>   
        private IQueueInformation QueueInformation { get; }

        public ConverterServerFilesDataToDTO(IFileSystemOperations fileSystemOperations,
                                             IQueueInformation queueInformation)
        {
            FileSystemOperations = fileSystemOperations;
            QueueInformation = queueInformation;
        }

        /// <summary>
        /// Конвертировать серверную модель в промежуточную
        /// </summary>       
        public FilesDataIntermediateResponse ConvertFilesToIntermediateResponse(FilesDataServer filesDataServer)
        {
            FilesQueueInfo filesQueueInfo = QueueInformation.GetQueueInfoUpToIdPackage(filesDataServer.ID);

            return new FilesDataIntermediateResponse()
            {
                IsCompleted = filesDataServer.IsCompleted,
                StatusProcessingProject = filesDataServer.StatusProcessingProject,
                FilesData = filesDataServer.FilesDataInfo?.Select(fileDataServer =>
                                                           ConvertFileToIntermediateResponse(fileDataServer)),
                FilesQueueInfo = new FilesQueueInfoResponse()
                {
                    FilesInQueueCount = filesQueueInfo.FilesInQueueCount,
                    PackagesInQueueCount = filesQueueInfo.PackagesInQueueCount,
                },
            };
        }

        /// <summary>
        /// Конвертировать серверную модель в окончательный ответ
        /// </summary>          
        public async Task<FilesDataResponse> ConvertFilesToResponse(FilesDataServer filesDataServer)
        {
            var filesDataToResponseTasks = filesDataServer.FilesDataInfo?.Select(fileDataServer =>
                                                           ConvertFileResponse(fileDataServer));
            var filesDataToResponse = await Task.WhenAll(filesDataToResponseTasks);

            return new FilesDataResponse()
            {
                FilesData = filesDataToResponse,
            };
        }

        /// <summary>
        /// Конвертировать файл серверной модели в промежуточную
        /// </summary>
        private FileDataIntermediateResponse ConvertFileToIntermediateResponse(FileDataServer fileDataServer)
        {
            return new FileDataIntermediateResponse()
            {
                FilePath = fileDataServer.FilePathClient,
                StatusProcessing = fileDataServer.StatusProcessing,
                FileConvertErrorType = fileDataServer.FileConvertErrorType,
            };
        }

        /// <summary>
        /// Конвертировать файл серверной модели в окончательный ответ
        /// </summary>
        private async Task<FileDataResponse> ConvertFileResponse(FileDataServer fileDataServer)
        {
            var fileDataSource = await FileSystemOperations.ConvertFileToByteAndZip(fileDataServer.FilePathServer);

            return new FileDataResponse()
            {
                FilePath = fileDataServer.FilePathClient,
                StatusProcessing = fileDataServer.StatusProcessing,
                FileDataSource = fileDataSource,
                FileConvertErrorType = fileDataServer.FileConvertErrorType,
            };
        }
    }
}