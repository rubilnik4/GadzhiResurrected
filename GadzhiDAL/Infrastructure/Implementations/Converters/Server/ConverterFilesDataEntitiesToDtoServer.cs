using GadzhiDAL.Entities.FilesConvert.Main;
using GadzhiDTOBase.TransferModels.FilesConvert.Base;
using GadzhiDTOServer.TransferModels.FilesConvert;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GadzhiDAL.Infrastructure.Implementations.Converters.Server
{
    /// <summary>
    /// Конвертер из трансферной модели в модель базы данных
    /// </summary>      
    public static class ConverterFilesDataEntitiesToDtoServer
    {
        /// <summary>
        /// Конвертировать из модели базы данных в запрос
        /// </summary>          
        public static async Task<PackageDataRequestServer> PackageDataToRequest(PackageDataEntity packageDataEntity)
        {
            if (packageDataEntity == null)  return null;

            var filesData = await packageDataEntity.FileDataEntities.Select(FileDataToRequest).
                                                    AsQueryable().ToListAsync();

            return new PackageDataRequestServer()
            {
                Id = Guid.Parse(packageDataEntity.Id),
                AttemptingConvertCount = packageDataEntity.AttemptingConvertCount,
                ConvertingSettings = ConvertingSettingsToRequest(packageDataEntity.ConvertingSettings),
                FilesData = filesData,
            };

        }

        /// <summary>
        /// Преобразовать параметры конвертации в трансферную модель
        /// </summary>
        private static ConvertingSettingsRequest ConvertingSettingsToRequest(ConvertingSettingsComponent convertingSettings) =>
            (convertingSettings != null)
                ? new ConvertingSettingsRequest()
                {
                    PersonId = convertingSettings.PersonId,
                    PdfNamingType = convertingSettings.PdfNamingType,
                }
                : throw new ArgumentNullException(nameof(convertingSettings));

        /// <summary>
        /// Конвертировать файл модели базы данных в запрос
        /// </summary>
        private static FileDataRequestServer FileDataToRequest(FileDataEntity fileDataEntity)
        {
            return new FileDataRequestServer()
            {
                FilePath = fileDataEntity?.FilePath ?? throw new ArgumentNullException(nameof(fileDataEntity)),
                StatusProcessing = fileDataEntity.StatusProcessing,
                ColorPrint = fileDataEntity.ColorPrint,
                FileDataSource = fileDataEntity.FileDataSource.AsQueryable().ToArray(),
            };
        }
    }
}
