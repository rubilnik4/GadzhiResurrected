using GadzhiDAL.Entities.FilesConvert.Main;
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

            var filesData = await packageDataEntity.FileDataEntities.AsQueryable().
                                  Select(fileData => FileDataToRequest(fileData)).ToListAsync();

            return new PackageDataRequestServer()
            {
                Id = Guid.Parse(packageDataEntity.Id),
                AttemptingConvertCount = packageDataEntity.AttemptingConvertCount,
                FilesData = filesData,
            };

        }

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
