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
using GadzhiDAL.Entities.FilesConvert.Components;
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
           new PackageDataShortResponseClient(Guid.Parse(packageDataEntity.Id),
                                              packageDataEntity.StatusProcessingProject,
                                              packageDataEntity.FileDataEntities.Select(FileDataAccessToIntermediateResponse).ToList(),
                                              FilesQueueInfoToResponse(filesQueueInfo));

        /// <summary>
        /// Конвертировать из модели базы данных в основной ответ
        /// </summary>          
        public static PackageDataResponseClient PackageDataAccessToResponse(PackageDataEntity packageDataEntity) =>
            new PackageDataResponseClient(Guid.Parse(packageDataEntity.Id),
                                          packageDataEntity.StatusProcessingProject,
                                          packageDataEntity.FileDataEntities.Select(FileDataAccessToResponse).ToList());


        /// <summary>
        /// Конвертировать файл модели базы данных в промежуточную
        /// </summary>
        private static FileDataShortResponseClient FileDataAccessToIntermediateResponse(FileDataEntity fileDataEntity) =>
            new FileDataShortResponseClient(fileDataEntity.FilePath, fileDataEntity.StatusProcessing,
                                            fileDataEntity.FileErrors.Select(ToErrorCommon).ToList());

        /// <summary>
        /// Конвертировать файл модели базы данных в основной ответ
        /// </summary>
        public static FileDataResponseClient FileDataAccessToResponse(FileDataEntity fileDataEntity) =>
            new FileDataResponseClient(fileDataEntity.FilePath, fileDataEntity.StatusProcessing,
                                       fileDataEntity.FileErrors.Select(ToErrorCommon).ToList(),
                                       fileDataEntity.FileDataSourceServerEntities.
                                       Where(fileData => fileData.FileDataSource != null).
                                       Select(FileDataSourceToResponse).ToList());

        /// <summary>
        /// Конвертировать информацию о количестве файлов в очереди
        /// </summary>        
        private static FilesQueueInfoResponseClient FilesQueueInfoToResponse(FilesQueueInfo filesQueueInfo) =>
            new FilesQueueInfoResponseClient(filesQueueInfo.FilesInQueueCount, filesQueueInfo.PackagesInQueueCount);

        /// <summary>
        /// Конвертировать информацию о готовых файлах
        /// </summary>        
        private static FileDataSourceResponseClient FileDataSourceToResponse(FileDataSourceEntity fileDataSourceEntity) =>
            new FileDataSourceResponseClient(fileDataSourceEntity.FileName, fileDataSourceEntity.FileExtensionType,
                                             fileDataSourceEntity.PaperSize, fileDataSourceEntity.PrinterName,
                                             fileDataSourceEntity.FileDataSource.ToArray());

        /// <summary>
        /// Конвертировать ошибки в трансферную модель
        /// </summary>
        private static ErrorCommonResponse ToErrorCommon(ErrorComponent errorComponent) =>
            new ErrorCommonResponse(errorComponent.ErrorConvertingType, errorComponent.Description);
    }
}