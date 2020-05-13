using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiDAL.Entities.FilesConvert.Main;
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
    public static class ConverterFilesDataEntitiesToDtoClient
    {
        /// <summary>
        /// Конвертировать из модели базы данных в промежуточную
        /// </summary>       
        public static async Task<PackageDataIntermediateResponseClient> PackageDataToIntermediateResponse(PackageDataEntity packageDataEntity,
                                                                                                          FilesQueueInfo filesQueueInfo)
        {
            if (packageDataEntity == null) throw new ArgumentNullException(nameof(packageDataEntity));

            var filesDataTasks = packageDataEntity.FileDataEntities.AsQueryable().
                                 Select(fileData => FileDataAccessToIntermediateResponse(fileData));
            var filesData = await Task.WhenAll(filesDataTasks);

            return new PackageDataIntermediateResponseClient()
            {
                Id = Guid.Parse(packageDataEntity.Id),
                StatusProcessingProject = packageDataEntity.StatusProcessingProject,
                FilesData = filesData,
                FilesQueueInfo = FilesQueueInfoToResponse(filesQueueInfo),
            };
        }

        /// <summary>
        /// Конвертировать из модели базы данных в основной ответ
        /// </summary>          
        public static async Task<PackageDataResponseClient> PackageDataAccessToResponse(PackageDataEntity packageDataEntity)
        {
            if (packageDataEntity == null) throw new ArgumentNullException(nameof(packageDataEntity));

            var filesDataTasks = packageDataEntity.FileDataEntities.AsQueryable().
                                 Select(fileData => FileDataAccessToResponse(fileData));
            var filesData = await Task.WhenAll(filesDataTasks);

            return new PackageDataResponseClient()
            {
                Id = Guid.Parse(packageDataEntity.Id),
                StatusProcessingProject = packageDataEntity.StatusProcessingProject,
                FilesData = filesData,
            };
        }

        /// <summary>
        /// Конвертировать файл модели базы данных в промежуточную
        /// </summary>
        private static async Task<FileDataIntermediateResponseClient> FileDataAccessToIntermediateResponse(FileDataEntity fileDataEntity)
        {
            if (fileDataEntity == null) throw new ArgumentNullException(nameof(fileDataEntity));

            var fileConvertErrorType = await fileDataEntity.FileConvertErrorType.AsQueryable().ToListAsync();
            if (!CheckStatusProcessing.CompletedStatusProcessingServer.Contains(fileDataEntity.StatusProcessing) &&
                !fileDataEntity.FileConvertErrorType.Any())
            {
                fileConvertErrorType = new List<FileConvertErrorType> { FileConvertErrorType.UnknownError };
            }

            return new FileDataIntermediateResponseClient()
            {
                FilePath = fileDataEntity.FilePath,
                StatusProcessing = fileDataEntity.StatusProcessing,
                FileConvertErrorTypes = fileConvertErrorType,
            };
        }

        /// <summary>
        /// Конвертировать файл модели базы данных в основной ответ
        /// </summary>
        private static async Task<FileDataResponseClient> FileDataAccessToResponse(FileDataEntity fileDataEntity)
        {
            if (fileDataEntity == null) throw new ArgumentNullException(nameof(fileDataEntity));

            var fileDataSourceResponseClient = await fileDataEntity.FileDataSourceServerEntities.AsQueryable().
                                               Select(fileData => FileDataSourceToResponse(fileData)).ToListAsync();

            return new FileDataResponseClient()
            {
                FilePath = fileDataEntity.FilePath,
                StatusProcessing = fileDataEntity.StatusProcessing,
                FilesDataSource = fileDataSourceResponseClient,
                FileConvertErrorTypes = fileDataEntity.FileConvertErrorType.AsQueryable().ToList(),
            };
        }

        /// <summary>
        /// Конвертировать информацию о количестве файлов в очереди
        /// </summary>        
        private static FilesQueueInfoResponseClient FilesQueueInfoToResponse(FilesQueueInfo filesQueueInfo) =>
            new FilesQueueInfoResponseClient()
            {
                FilesInQueueCount = filesQueueInfo?.FilesInQueueCount ?? 0,
                PackagesInQueueCount = filesQueueInfo?.PackagesInQueueCount ?? 0,
            };

        /// <summary>
        /// Конвертировать информацию о готовых файлах
        /// </summary>        
        private static FileDataSourceResponseClient FileDataSourceToResponse(FileDataSourceEntity fileDataSourceEntity) =>
            new FileDataSourceResponseClient()
            {
                FileName = fileDataSourceEntity?.FileName ?? throw new ArgumentNullException(nameof(fileDataSourceEntity)),
                FileDataSource = fileDataSourceEntity.FileDataSource?.AsQueryable().ToArray(),
            };
    }
}