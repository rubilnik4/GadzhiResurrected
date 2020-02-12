using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces.Converters;
using GadzhiConverting.Models.FilesConvert.Implementations;
using GadzhiDTO.TransferModels.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GadzhiConverting.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Конвертер из серверной модели в трансферную
    /// </summary>
    public class ConverterServerFilesDataToDTO : IConverterServerFilesDataToDTO
    {
        /// <summary>
        /// Проверка состояния папок и файлов
        /// </summary>   
        private readonly IFileSystemOperations _fileSystemOperations;

      
        public ConverterServerFilesDataToDTO(IFileSystemOperations fileSystemOperations)
        {
            _fileSystemOperations = fileSystemOperations;           
        }

        /// <summary>
        /// Конвертировать серверную модель в промежуточную
        /// </summary>       
        public FilesDataIntermediateResponse ConvertFilesToIntermediateResponse(FilesDataServer filesDataServer)
        {
           // FilesQueueInfo filesQueueInfo = _queueInformation.GetQueueInfoUpToIdPackage(filesDataServer.ID);

            return new FilesDataIntermediateResponse()
            {
                IsCompleted = filesDataServer.IsCompleted,
                StatusProcessingProject = filesDataServer.StatusProcessingProject,
                FilesData = filesDataServer.FilesDataInfo?.Select(fileDataServer =>
                                                                  ConvertFileToIntermediateResponse(fileDataServer)),               
            };
        }

        ///// <summary>
        ///// Конвертировать серверную модель в окончательный ответ
        ///// </summary>          
        //public async Task<FilesDataResponse> ConvertFilesToResponse(FilesDataServer filesDataServer)
        //{
        //    var filesDataToResponseTasks = filesDataServer.FilesDataInfo?.Select(fileDataServer =>
        //                                                   ConvertFileResponse(fileDataServer));
        //    var filesDataToResponse = await Task.WhenAll(filesDataToResponseTasks);

        //    return new FilesDataResponse()
        //    {
        //        FilesData = filesDataToResponse,
        //    };
        //}

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

        ///// <summary>
        ///// Конвертировать файл серверной модели в окончательный ответ
        ///// </summary>
        //private async Task<FileDataResponse> ConvertFileResponse(FileDataServer fileDataServer)
        //{
        //    var fileDataSource = await _fileSystemOperations.ConvertFileToByteAndZip(fileDataServer.FilePathServer);

        //    return new FileDataResponse()
        //    {
        //        FilePath = fileDataServer.FilePathClient,
        //        StatusProcessing = fileDataServer.StatusProcessing,
        //        FileDataSource = fileDataSource,
        //        FileConvertErrorType = fileDataServer.FileConvertErrorType,
        //    };
        //}
    }
}