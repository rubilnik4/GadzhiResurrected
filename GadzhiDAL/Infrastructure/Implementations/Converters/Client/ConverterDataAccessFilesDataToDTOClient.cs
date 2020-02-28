using GadzhiCommon.Enums.FilesConvert;
using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Infrastructure.Interfaces.Converters.Client;
using GadzhiDAL.Models.Implementations;
using GadzhiDTOClient.TransferModels.FilesConvert;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<FilesDataIntermediateResponseClient> ConvertFilesDataAccessToIntermediateResponse(FilesDataEntity filesDataEntity,
                                                                                                FilesQueueInfo filesQueueInfo)
        {
            if (filesDataEntity != null)
            {
                var filesDataTasks = filesDataEntity.FilesData?.AsQueryable()?.
                                     Select(fileData => ConvertFileDataAccessToIntermediateResponse(fileData));
                var filesData = await Task.WhenAll(filesDataTasks);

                return new FilesDataIntermediateResponseClient()
                {
                    Id = Guid.Parse(filesDataEntity.Id),
                    IsCompleted = filesDataEntity.IsCompleted,
                    StatusProcessingProject = filesDataEntity.StatusProcessingProject,
                    FilesData = filesData,
                    FilesQueueInfo = ConvertFilesQueueInfoToResponse(filesQueueInfo),
                };
            }

            return null;
        }

        /// <summary>
        /// Конвертировать из модели базы данных в основной ответ
        /// </summary>          
        public async Task<FilesDataResponseClient> ConvertFilesDataAccessToResponse(FilesDataEntity filesDataEntity)
        {
            if (filesDataEntity != null)
            {
                var filesDataTasks = filesDataEntity.FilesData?.AsQueryable()?.
                                     Select(fileData => ConvertFileDataAccessToResponse(fileData));
                var filesData = await Task.WhenAll(filesDataTasks);

                return new FilesDataResponseClient()
                {
                    Id = Guid.Parse(filesDataEntity.Id),
                    IsCompleted = filesDataEntity.IsCompleted,
                    StatusProcessingProject = filesDataEntity.StatusProcessingProject,
                    FilesData = filesData,
                };
            }

            return null;
        }

        /// <summary>
        /// Конвертировать файл модели базы данных в промежуточную
        /// </summary>
        private async Task<FileDataIntermediateResponseClient> ConvertFileDataAccessToIntermediateResponse(FileDataEntity fileDataEntity)
        {
            var fileConvertErrorType = await fileDataEntity.FileConvertErrorType.AsQueryable().ToListAsync();
            if (!fileDataEntity.IsCompleted && !fileDataEntity.FileConvertErrorType.Any())
            {
                fileConvertErrorType = new List<FileConvertErrorType> { FileConvertErrorType.UnknownError };
            }

            return new FileDataIntermediateResponseClient()
            {
                FilePath = fileDataEntity.FilePath,
                StatusProcessing = fileDataEntity.StatusProcessing,
                IsCompleted = fileDataEntity.IsCompleted,
                FileConvertErrorType = fileConvertErrorType,
            };
        }

        /// <summary>
        /// Конвертировать файл модели базы данных в основной ответ
        /// </summary>
        private async Task<FileDataResponseClient> ConvertFileDataAccessToResponse(FileDataEntity fileDataEntity)
        {
            var fileDataSourceResponseClientTasks = fileDataEntity.FileDataSourceEntity.AsQueryable().
                                                    Select(fileData => ConvertFileDataSourceResponse(fileData));
            var fileDataSourceResponseClient = await Task.WhenAll(fileDataSourceResponseClientTasks);

            return new FileDataResponseClient()
            {
                FilePath = fileDataEntity.FilePath,
                StatusProcessing = fileDataEntity.StatusProcessing,
                IsCompleted = fileDataEntity.IsCompleted,
                FileDataSourceResponseClient = fileDataSourceResponseClient,
                FileConvertErrorType = await fileDataEntity.FileConvertErrorType.AsQueryable().ToListAsync(),
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
        private async Task<FileDataSourceResponseClient> ConvertFileDataSourceResponse(FileDataSourceEntity fileDataSourceEntity)
        {
            return new FileDataSourceResponseClient()
            {
                FileName = fileDataSourceEntity?.FileName,
                FileDataSource = await fileDataSourceEntity?.FileDataSource?.AsQueryable().ToListAsync(),
            };
        }
    }
}