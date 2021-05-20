using System;
using GadzhiDTOClient.TransferModels.FilesConvert;
using NHibernate.Linq;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Enums.FilesConvert;
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
        public static PackageDataEntity ToPackageData(PackageDataRequestClient packageDataRequest, string identityName)
        {
            if (packageDataRequest == null) return null;

            var packageDataAccess = packageDataRequest.FilesData?.AsQueryable().
                                    Select(fileData => ToFileData(fileData));

            var packageDataEntity = new PackageDataEntity();
            packageDataEntity.SetId(packageDataRequest.Id);
            packageDataEntity.IdentityLocalName = identityName;
            packageDataEntity.IdentityServerName = String.Empty;
            packageDataEntity.CreationDateTime = DateTimeService.GetDateTimeNow();
            packageDataEntity.ConvertingSettings = ConvertingSettingsToRequest(packageDataRequest.ConvertingSettings);
            packageDataEntity.SetFileDataEntities(packageDataAccess);
            packageDataEntity.StatusProcessingProject = StatusProcessingProject.InQueue;
            return packageDataEntity;
        }

        /// <summary>
        /// Преобразовать параметры конвертации из трансферной модели
        /// </summary>
        private static ConvertingSettingsComponent ConvertingSettingsToRequest(ConvertingSettingsRequest convertingSettings) =>
           new ConvertingSettingsComponent
           {
               PersonId = convertingSettings.PersonId,
               PdfNamingType = convertingSettings.PdfNamingType,
               ConvertingModeTypes = convertingSettings.ConvertingModeTypes,
               UseDefaultSignature = convertingSettings.UseDefaultSignature,
           };

        /// <summary>
        /// Конвертер информации из трансферной модели в единичный класс базы данных
        /// </summary>      
        private static FileDataEntity ToFileData(FileDataRequestClient fileDataRequest) =>
            new FileDataEntity
            {
                ColorPrintType = fileDataRequest.ColorPrintType,
                FilePath = fileDataRequest.FilePath,
                FileDataSource = fileDataRequest.FileDataSource,
                FileExtensionAdditional = fileDataRequest.FileExtensionAdditional,
                FileDataSourceAdditional = fileDataRequest.FileDataSourceAdditional,
                StatusProcessing = StatusProcessing.InQueue,
            };
    }
}
