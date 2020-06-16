using System;
using System.Linq;
using System.Threading.Tasks;
using GadzhiDAL.Entities.FilesConvert.Archive;
using GadzhiDAL.Entities.FilesConvert.Errors;
using GadzhiDAL.Entities.FilesConvert.Main;
using NHibernate.Linq;

namespace GadzhiDAL.Infrastructure.Implementations.Converters.Errors
{
    /// <summary>
    /// Конвертер для хранилища ошибок
    /// </summary>  
    public static class ConverterToErrorsStore
    {
        /// <summary>
        /// Конвертировать пакет с ошибками в хранилище базы данных
        /// </summary>    
        public static async Task<PackageDataErrorEntity> PackageDataToErrorStore(PackageDataEntity packageDataEntity)
        {
            if (packageDataEntity == null) throw new ArgumentNullException(nameof(packageDataEntity));

            var filesDataArchiveEntity = new PackageDataErrorEntity()
            {
                CreationDateTime = packageDataEntity.CreationDateTime,
                IdentityLocalName = packageDataEntity.IdentityLocalName,
                IdentityServerName = packageDataEntity.IdentityServerName
            };
            filesDataArchiveEntity.SetId(Guid.Parse(packageDataEntity.Id));

            var fileDataEntities = await packageDataEntity.FileDataEntities.AsQueryable().
                                   Where(fileData => fileData.FileErrors.Count > 0).
                                   Select(fileData => FileDataToErrorStore(fileData)).ToListAsync();
            filesDataArchiveEntity.SetFileDataEntities(fileDataEntities);

            return filesDataArchiveEntity;
        }

        /// <summary>
        /// Конвертировать файл с ошибками в хранилище базы данных
        /// </summary>    
        public static FileDataErrorEntity FileDataToErrorStore(FileDataEntity fileDataEntity) =>
            (fileDataEntity != null)
                ? new FileDataErrorEntity()
                {
                    ColorPrint = fileDataEntity.ColorPrint,
                    FilePath = fileDataEntity.FilePath,
                    FileErrorsStore = fileDataEntity.FileErrors.AsQueryable().ToList()
                }
                : throw new ArgumentNullException(nameof(fileDataEntity));
    }
}