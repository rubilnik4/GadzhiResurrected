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
                                                              ConvertFileDataAccessToIntermediateResponse(fileData)),
            };
        }

        /// <summary>
        /// Конвертировать из модели базы данных в основной ответ
        /// </summary>          
        public FilesDataResponse ConvertFilesDataAccessToResponse(FilesDataEntity filesDataEntity)
        {
            return new FilesDataResponse()
            {
                Id = Guid.Parse(filesDataEntity.Id),
                IsCompleted = filesDataEntity.IsCompleted,
                StatusProcessingProject = filesDataEntity.StatusProcessingProject,
                FilesData = filesDataEntity.FilesData?.Select(fileData =>
                                                              ConvertFileDataAccessToResponse(fileData)),
            };
        }

        /// <summary>
        /// Конвертировать из модели базы данных в запрос
        /// </summary>          
        public FilesDataRequest ConvertFilesDataAccessToRequest(FilesDataEntity filesDataEntity)
        {
            FilesDataRequest filesDataRequest = null;

            if (filesDataEntity != null)
            {
                filesDataRequest = new FilesDataRequest()
                {
                    Id = Guid.Parse(filesDataEntity.Id),
                    
                    FilesData = filesDataEntity.FilesData?.Select(fileData =>
                                                                  ConvertFileDataAccessToRequest(fileData)),
                };
            }

            return filesDataRequest;
        }

        /// <summary>
        /// Конвертировать файл модели базы данных в промежуточную
        /// </summary>
        private FileDataIntermediateResponse ConvertFileDataAccessToIntermediateResponse(FileDataEntity fileDataEntity)
        {
            return new FileDataIntermediateResponse()
            {
                FilePath = fileDataEntity.FilePath,
                StatusProcessing = fileDataEntity.StatusProcessing,
                IsCompleted = fileDataEntity.IsCompleted,
                FileConvertErrorType = fileDataEntity.FileConvertErrorType,
            };
        }

        /// <summary>
        /// Конвертировать файл модели базы данных в основной ответ
        /// </summary>
        private FileDataResponse ConvertFileDataAccessToResponse(FileDataEntity fileDataEntity)
        {
            return new FileDataResponse()
            {
                FilePath = fileDataEntity.FilePath,
                StatusProcessing = fileDataEntity.StatusProcessing,
                IsCompleted = fileDataEntity.IsCompleted,
                FileDataSource = fileDataEntity.FileDataSource,
                FileConvertErrorType = fileDataEntity.FileConvertErrorType,
            };
        }

        /// <summary>
        /// Конвертировать файл модели базы данных в запрос
        /// </summary>
        private FileDataRequest ConvertFileDataAccessToRequest(FileDataEntity fileDataEntity)
        {
            return new FileDataRequest()
            {
                FilePath = fileDataEntity.FilePath,
                StatusProcessing = fileDataEntity.StatusProcessing,
                FileDataSource = fileDataEntity.FileDataSource,
            };
        }
    }
}