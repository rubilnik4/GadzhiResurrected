using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiDAL.Models.Implementations;
using GadzhiDTOClient.TransferModels.FilesConvert;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Entities.FilesConvert.Base.Components;
using GadzhiDTOBase.TransferModels.FilesConvert.Base;

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
        public static PackageDataShortResponseClient PackageDataToIntermediateResponse(PackageDataEntity packageDataEntity,
                                                                                       FilesQueueInfo filesQueueInfo) =>
           new PackageDataShortResponseClient()
           {
               Id = Guid.Parse(packageDataEntity.Id),
               StatusProcessingProject = packageDataEntity.StatusProcessingProject,
               FilesData = packageDataEntity.FileDataEntities.Select(FileDataAccessToIntermediateResponse).ToList(),
               FilesQueueInfo = FilesQueueInfoToResponse(filesQueueInfo),
           };

        /// <summary>
        /// Конвертировать из модели базы данных в основной ответ
        /// </summary>          
        public static async Task<PackageDataResponseClient> PackageDataAccessToResponse(PackageDataEntity packageDataEntity)
        {
            var filesDataTasks = packageDataEntity.FileDataEntities.AsQueryable().
                                 Select(fileData => FileDataAccessToResponse(fileData));
            var filesData = await Task.WhenAll(filesDataTasks);

            return new PackageDataResponseClient
            {
                Id = Guid.Parse(packageDataEntity.Id),
                StatusProcessingProject = packageDataEntity.StatusProcessingProject,
                FilesData = filesData,
            };
        }

        /// <summary>
        /// Конвертировать файл модели базы данных в промежуточную
        /// </summary>
        private static FileDataShortResponseClient FileDataAccessToIntermediateResponse(FileDataEntity fileDataEntity)
        {
            if (fileDataEntity == null) throw new ArgumentNullException(nameof(fileDataEntity));

            var fileConvertErrorType = fileDataEntity.FileErrors.Select(ToErrorCommon).ToList();
            if (!CheckStatusProcessing.CompletedStatusProcessing.Contains(fileDataEntity.StatusProcessing) &&
                !fileDataEntity.FileErrors.Any())
            {
                var error = new ErrorCommonResponse()
                {
                    ErrorConvertingType = ErrorConvertingType.UnknownError,
                    ErrorDescription = "Конвертирование пакета не может быть завершено"
                };
                fileConvertErrorType = new List<ErrorCommonResponse> { error };
            }

            return new FileDataShortResponseClient()
            {
                FilePath = fileDataEntity.FilePath,
                StatusProcessing = fileDataEntity.StatusProcessing,
                FileErrors = fileConvertErrorType,
            };
        }

        /// <summary>
        /// Конвертировать файл модели базы данных в основной ответ
        /// </summary>
        public static async Task<FileDataResponseClient> FileDataAccessToResponse(FileDataEntity fileDataEntity)
        {
            if (fileDataEntity == null) throw new ArgumentNullException(nameof(fileDataEntity));

            var fileDataSourceResponseClient = await fileDataEntity.FileDataSourceServerEntities.AsQueryable().
                                               Select(fileData => FileDataSourceToResponse(fileData)).ToListAsync();

            return new FileDataResponseClient()
            {
                FilePath = fileDataEntity.FilePath,
                StatusProcessing = fileDataEntity.StatusProcessing,
                FilesDataSource = fileDataSourceResponseClient,
                FileErrors = fileDataEntity.FileErrors.Select(ToErrorCommon).ToList(),
            };
        }

        /// <summary>
        /// Конвертировать информацию о количестве файлов в очереди
        /// </summary>        
        private static FilesQueueInfoResponseClient FilesQueueInfoToResponse(FilesQueueInfo filesQueueInfo) =>
            new FilesQueueInfoResponseClient
            {
                FilesInQueueCount = filesQueueInfo?.FilesInQueueCount ?? 0,
                PackagesInQueueCount = filesQueueInfo?.PackagesInQueueCount ?? 0,
            };

        /// <summary>
        /// Конвертировать информацию о готовых файлах
        /// </summary>        
        private static FileDataSourceResponseClient FileDataSourceToResponse(FileDataSourceEntity fileDataSourceEntity) =>
            new FileDataSourceResponseClient
            {
                FileName = fileDataSourceEntity?.FileName ?? throw new ArgumentNullException(nameof(fileDataSourceEntity)),
                FileDataSource = fileDataSourceEntity.FileDataSource?.AsQueryable().ToArray(),
            };

        /// <summary>
        /// Конвертировать ошибки в трансферную модель
        /// </summary>
        private static ErrorCommonResponse ToErrorCommon(ErrorComponent errorComponent) =>
            new ErrorCommonResponse
            {
                ErrorConvertingType = errorComponent.ErrorConvertingType,
                ErrorDescription = errorComponent.ErrorDescription
            };
    }
}