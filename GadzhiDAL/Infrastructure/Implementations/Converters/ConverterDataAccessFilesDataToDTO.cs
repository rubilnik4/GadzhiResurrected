using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Infrastructure.Interfaces.Converters;
using GadzhiDTO.TransferModels.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GadzhiDAL.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Конвертер из модели базы данных в трансферную
    /// </summary>
    public class ConverterDataAccessFilesDataToDTO : IConverterDataAccessFilesDataToDTO
    {      
        public ConverterDataAccessFilesDataToDTO()
        {
            
        }

        /// <summary>
        /// Конвертировать из модели базы данных в промежуточную
        /// </summary>       
        public FilesDataIntermediateResponse ConvertFilesDataAccessToIntermediateResponse(FilesDataEntity filesDataEntity)
        {            
            return new FilesDataIntermediateResponse()
            {
                IsCompleted = filesDataEntity.IsCompleted,
                StatusProcessingProject = filesDataEntity.StatusProcessingProject,
                FilesData = filesDataEntity.FilesData?.Select(fileData =>
                                                              ConvertFileAccessToIntermediateResponse(fileData)),               
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
        /// Конвертировать файл  модели базы данных в промежуточную
        /// </summary>
        private FileDataIntermediateResponse ConvertFileAccessToIntermediateResponse(FileDataEntity fileDataEntity)
        {
            return new FileDataIntermediateResponse()
            {
                FilePath = fileDataEntity.FilePath,
                StatusProcessing = fileDataEntity.StatusProcessing,
                FileConvertErrorType = fileDataEntity.FileConvertErrorType,
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