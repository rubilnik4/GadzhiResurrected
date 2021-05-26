using System;
using System.Collections.Generic;
using GadzhiDTOClient.TransferModels.FilesConvert;
using NHibernate.Linq;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions.Functional;
using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Entities.FilesConvert.Components;
using GadzhiDAL.Infrastructure.Implementations.DateTimes;
using GadzhiDTOBase.TransferModels.FilesConvert.Base;

namespace GadzhiDAL.Infrastructure.Implementations.Converters.Client
{
    /// <summary>
    /// Конвертер из трансферной модели в модель базы данных
    /// </summary>      
    public static class ConverterFilesDataEntitiesFromDtoClient
    {
        /// <summary>
        /// Конвертер пакета информации из трансферной модели в модель базы данных
        /// </summary>      
        public static PackageDataEntity ToPackageData(PackageDataRequestClient packageData, string identityName) =>
            new PackageDataEntity(packageData.Id.ToString(), StatusProcessingProject.InQueue,
                                  DateTimeService.GetDateTimeNow(), identityName, String.Empty, 0,
                                  packageData.FilesData.Select(ToFileData).ToList(),
                                  ConvertingSettingsToRequest(packageData.ConvertingSettings)).
            Void(package =>
                     {
                         foreach (var fileData in package.FileDataEntities) fileData.PackageDataEntity = package;
                     });

        /// <summary>
        /// Преобразовать параметры конвертации из трансферной модели
        /// </summary>
        private static ConvertingSettingsComponent ConvertingSettingsToRequest(ConvertingSettingsRequest convertingSettings) =>
           new ConvertingSettingsComponent(convertingSettings.PersonId, convertingSettings.PdfNamingType,
                                           convertingSettings.ConvertingModeTypes.ToList(),
                                           convertingSettings.UseDefaultSignature);

        /// <summary>
        /// Конвертер информации из трансферной модели в единичный класс базы данных
        /// </summary>      
        private static FileDataEntity ToFileData(FileDataRequestClient fileDataRequest) =>
            new FileDataEntity(fileDataRequest.FilePath, fileDataRequest.ColorPrintType, StatusProcessing.InQueue,
                               new List<FileDataSourceEntity>(), new List<ErrorComponent>(), fileDataRequest.FileDataSource,
                               fileDataRequest.FileExtensionAdditional, fileDataRequest.FileDataSourceAdditional);
    }
}
