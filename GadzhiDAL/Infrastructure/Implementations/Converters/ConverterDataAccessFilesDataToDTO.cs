using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Infrastructure.Interfaces.Converters;
using GadzhiDAL.Models.Implementations;
using GadzhiDTO.TransferModels.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GadzhiDAL.Infrastructure.Implementations.Converters
{
    // Внимательно проверять на ленивую загрузку ToList()
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
        public FilesDataIntermediateResponse ConvertFilesDataAccessToIntermediateResponse(FilesDataEntity filesDataEntity,
                                                                                          FilesQueueInfo filesQueueInfo)
        {
            if (filesDataEntity != null)
            {
                return new FilesDataIntermediateResponse()
                {
                    Id = Guid.Parse(filesDataEntity.Id),
                    IsCompleted = filesDataEntity.IsCompleted,
                    StatusProcessingProject = filesDataEntity.StatusProcessingProject,
                    FilesData = filesDataEntity.FilesData?.Select(fileData => ConvertFileDataAccessToIntermediateResponse(fileData)).
                                                           ToList(),
                    FilesQueueInfo = ConvertFilesQueueInfoToResponse(filesQueueInfo),
                };
            }

            return null;
        }

        /// <summary>
        /// Конвертировать из модели базы данных в основной ответ
        /// </summary>          
        public FilesDataResponse ConvertFilesDataAccessToResponse(FilesDataEntity filesDataEntity)
        {
            if (filesDataEntity != null)
            {
                return new FilesDataResponse()
                {
                    Id = Guid.Parse(filesDataEntity.Id),
                    IsCompleted = filesDataEntity.IsCompleted,
                    StatusProcessingProject = filesDataEntity.StatusProcessingProject,
                    FilesData = filesDataEntity.FilesData?.Select(fileData => ConvertFileDataAccessToResponse(fileData)).
                                                           ToList(),
                };
            }

            return null;
        }

        /// <summary>
        /// Конвертировать из модели базы данных в запрос
        /// </summary>          
        public FilesDataRequest ConvertFilesDataAccessToRequest(FilesDataEntity filesDataEntity)
        {
            if (filesDataEntity != null)
            {
                return new FilesDataRequest()
                {
                    Id = Guid.Parse(filesDataEntity.Id),
                    AttemptingConvertCount = filesDataEntity.IdentityMachine.AttemptingConvertCount,
                    FilesData = filesDataEntity.FilesData?.
                                                Select(fileData => ConvertFileDataAccessToRequest(fileData)).
                                                ToList(),
                };
            }

            return null;
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
                FileConvertErrorType = fileDataEntity.FileConvertErrorType.
                                                      ToList(),
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
                FileConvertErrorType = fileDataEntity.FileConvertErrorType.
                                                      ToList(),
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

        /// <summary>
        /// Конвертировать информацию о количестве файлов в очереди
        /// </summary>        
        private FilesQueueInfoResponse ConvertFilesQueueInfoToResponse(FilesQueueInfo filesQueueInfo)
        {
            return new FilesQueueInfoResponse()
            {
                FilesInQueueCount = filesQueueInfo?.FilesInQueueCount ?? 0,
                PackagesInQueueCount = filesQueueInfo?.PackagesInQueueCount ?? 0,
            };
        }
    }
}