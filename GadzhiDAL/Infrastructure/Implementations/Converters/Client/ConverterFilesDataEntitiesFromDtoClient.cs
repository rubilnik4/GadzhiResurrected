using System;
using GadzhiDAL.Entities.FilesConvert.Main;
using GadzhiDTOClient.TransferModels.FilesConvert;
using NHibernate.Linq;
using System.Linq;
using System.Threading.Tasks;
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

            var filesDataEntity = new PackageDataEntity();
            filesDataEntity.SetId(packageDataRequest.Id);
            filesDataEntity.IdentityLocalName = identityName;
            filesDataEntity.IdentityServerName = String.Empty;
            filesDataEntity.ConvertingSettings = ConvertingSettingsToRequest(packageDataRequest.ConvertingSettings);
            filesDataEntity.SetFileDataEntities(packageDataAccess);

            return filesDataEntity;
        }

        /// <summary>
        /// Преобразовать параметры конвертации из трансферной модели
        /// </summary>
        private static ConvertingSettingsComponent ConvertingSettingsToRequest(ConvertingSettingsRequest convertingSettings) =>
            (convertingSettings != null)
                ? new ConvertingSettingsComponent()
                {
                    Department = convertingSettings.Department
                }
                : throw new ArgumentNullException(nameof(convertingSettings));

        /// <summary>
        /// Конвертер информации из трансферной модели в единичный класс базы данных
        /// </summary>      
        private static FileDataEntity ToFileData(FileDataRequestClient fileDataRequest) =>
            new FileDataEntity()
            {
                ColorPrint = fileDataRequest.ColorPrint,
                FilePath = fileDataRequest.FilePath,
                FileDataSource = fileDataRequest.FileDataSource,
            };
    }
}
