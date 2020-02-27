using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Infrastructure.Interfaces.Converters.Client;
using GadzhiDAL.Models.Implementations;
using GadzhiDTOClient.TransferModels.FilesConvert;
using System;
using System.Linq;

namespace GadzhiDAL.Infrastructure.Implementations.Converters.Client
{
    /// <summary>
    /// Конвертер из модели базы данных в трансферную для клиента
    /// </summary>
    public class ConverterDataAccessFilesDataToDTOClient : IConverterDataAccessFilesDataToDTOClient
    {
        public ConverterDataAccessFilesDataToDTOClient()
        {

        }

        /// <summary>
        /// Конвертировать из модели базы данных в промежуточную
        /// </summary>       
        public FilesDataIntermediateResponseClient ConvertFilesDataAccessToIntermediateResponse(FilesDataEntity filesDataEntity,
                                                                                                FilesQueueInfo filesQueueInfo)
        {
            if (filesDataEntity != null)
            {
                return new FilesDataIntermediateResponseClient()
                {
                    Id = Guid.Parse(filesDataEntity.Id),
                    IsCompleted = filesDataEntity.IsCompleted,
                    StatusProcessingProject = filesDataEntity.StatusProcessingProject,
                    FilesData = filesDataEntity.FilesData?.Select(fileData => ConvertFileDataAccessToIntermediateResponse(fileData)).ToList(),
                    FilesQueueInfo = ConvertFilesQueueInfoToResponse(filesQueueInfo),
                };
            }

            return null;
        }

        /// <summary>
        /// Конвертировать из модели базы данных в основной ответ
        /// </summary>          
        public FilesDataResponseClient ConvertFilesDataAccessToResponse(FilesDataEntity filesDataEntity)
        {
            if (filesDataEntity != null)
            {
                return new FilesDataResponseClient()
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
        /// Конвертировать файл модели базы данных в промежуточную
        /// </summary>
        private FileDataIntermediateResponseClient ConvertFileDataAccessToIntermediateResponse(FileDataEntity fileDataEntity)
        {
            return new FileDataIntermediateResponseClient()
            {
                FilePath = fileDataEntity.FilePath,
                StatusProcessing = fileDataEntity.StatusProcessing,
                IsCompleted = fileDataEntity.IsCompleted,
                FileConvertErrorType = fileDataEntity.FileConvertErrorType.ToList(),
            };
        }

        /// <summary>
        /// Конвертировать файл модели базы данных в основной ответ
        /// </summary>
        private FileDataResponseClient ConvertFileDataAccessToResponse(FileDataEntity fileDataEntity)
        {
            return new FileDataResponseClient()
            {
                FilePath = fileDataEntity.FilePath,
                StatusProcessing = fileDataEntity.StatusProcessing,
                IsCompleted = fileDataEntity.IsCompleted,
                FileDataSourceResponseClient = fileDataEntity.FileDataSourceEntity.
                                               Select(fileData => ConvertFileDataSourceResponse(fileData)).ToList(),
                FileConvertErrorType = fileDataEntity.FileConvertErrorType.ToList(),
            };
        }

        /// <summary>
        /// Конвертировать информацию о количестве файлов в очереди
        /// </summary>        
        private FilesQueueInfoResponseClient ConvertFilesQueueInfoToResponse(FilesQueueInfo filesQueueInfo)
        {
            return new FilesQueueInfoResponseClient()
            {
                FilesInQueueCount = filesQueueInfo?.FilesInQueueCount ?? 0,
                PackagesInQueueCount = filesQueueInfo?.PackagesInQueueCount ?? 0,
            };
        }

        /// <summary>
        /// Конвертировать информацию о готовых файлах
        /// </summary>        
        private FileDataSourceResponseClient ConvertFileDataSourceResponse(FileDataSourceEntity fileDataSourceEntity)
        {
            return new FileDataSourceResponseClient()
            {
                FileName = fileDataSourceEntity?.FileName,
                FileDataSource = fileDataSourceEntity?.FileDataSource,
            };
        }
    }
}