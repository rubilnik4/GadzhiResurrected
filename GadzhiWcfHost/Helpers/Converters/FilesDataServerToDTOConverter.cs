using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiWcfHost.Infrastructure.Implementations;
using GadzhiWcfHost.Infrastructure.Implementations.Information;
using GadzhiWcfHost.Models.FilesConvert.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GadzhiWcfHost.Helpers.Converters
{
    /// <summary>
    /// Конвертер из серверной модели в трансферную
    /// </summary>
    public static class FilesDataServerToDTOConverter
    {
        /// <summary>
        /// Конвертировать серверную модель в промежуточную
        /// </summary>       
        public static FilesDataIntermediateResponse ConvertFilesToIntermediateResponse(FilesDataServer filesDataServer, IQueueInformation queueInformation)
        {
            FilesQueueInfo filesQueueInfo = queueInformation.GetQueueInfoUpToIdPackage(filesDataServer.ID);

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
        public static async Task<FilesDataResponse> ConvertFilesToResponse(FilesDataServer filesDataServer, IFileSystemOperations fileSystemOperations)
        {
            var filesDataToResponseTasks = filesDataServer.FilesDataInfo?.Select(fileDataServer =>
                                                           ConvertFileResponse(fileDataServer, fileSystemOperations));
            var filesDataToResponse = await Task.WhenAll(filesDataToResponseTasks);

            return new FilesDataResponse()
            {
                FilesData = filesDataToResponse,
            };
        }

        /// <summary>
        /// Конвертировать файл серверной модели в промежуточную
        /// </summary>
        private static FileDataIntermediateResponse ConvertFileToIntermediateResponse(FileDataServer fileDataServer)
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
        private static async Task<FileDataResponse> ConvertFileResponse(FileDataServer fileDataServer, IFileSystemOperations fileSystemOperations)
        {
            var fileDataSource = await fileSystemOperations.ConvertFileToByteAndZip(fileDataServer.FilePathServer);

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