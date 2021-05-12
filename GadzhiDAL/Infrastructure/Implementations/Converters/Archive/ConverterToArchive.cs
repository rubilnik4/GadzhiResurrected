using GadzhiDAL.Entities.FilesConvert.Archive;
using GadzhiDAL.Entities.FilesConvert.Main;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiDAL.Infrastructure.Implementations.Converters.Archive
{
    /// <summary>
    /// Конвертер в архивную версию
    /// </summary>    
    public static class ConverterToArchive
    {
        /// <summary>
        /// Конвертировать пакет в архивную версию базы данных
        /// </summary>    
        public static async Task<PackageDataArchiveEntity> PackageDataToArchive(PackageDataEntity packageDataEntity)
        {
            var filesDataArchiveEntity = new PackageDataArchiveEntity
            {
                CreationDateTime = packageDataEntity.CreationDateTime,
                IdentityLocalName = packageDataEntity.IdentityLocalName,
                IdentityServerName = packageDataEntity.IdentityServerName,
                StatusProcessingProject = StatusProcessingProject.Archived,
            };
            filesDataArchiveEntity.SetId(Guid.Parse(packageDataEntity.Id));

            var fileDataEntitiesTask = packageDataEntity.FileDataEntities.AsQueryable().
                                       Select(fileData => FileDataToArchive(fileData));
            var fileDataEntities = await Task.WhenAll(fileDataEntitiesTask);
            filesDataArchiveEntity.SetFileDataArchiveEntities(fileDataEntities);

            return filesDataArchiveEntity;
        }

        /// <summary>
        /// Конвертировать файл в архивную версию базы данных
        /// </summary>    
        public static async Task<FileDataArchiveEntity> FileDataToArchive(FileDataEntity fileDataEntity)
        {
            if (fileDataEntity == null) throw new ArgumentNullException(nameof(fileDataEntity));

            var fileDataArchiveEntity = new FileDataArchiveEntity
            {
                ColorPrintType = fileDataEntity.ColorPrintType,
                FilePath = fileDataEntity.FilePath,
                StatusProcessing = StatusProcessing.Archive,
                FileErrors = fileDataEntity.FileErrors.ToList(),
            };
            var fileDataSourceServerEntities = await fileDataEntity.FileDataSourceServerEntities.AsQueryable().
                                               Select(fileDataSource => FileDataSourceToArchive(fileDataSource)).ToListAsync();
            fileDataArchiveEntity.SetFileDataSourceArchiveEntities(fileDataSourceServerEntities);

            return fileDataArchiveEntity;
        }

        /// <summary>
        /// Конвертировать информацию о готовых файлах в архивную версию
        /// </summary>     
        public static FileDataSourceArchiveEntity FileDataSourceToArchive(FileDataSourceEntity fileDataSourceEntity) =>
            new FileDataSourceArchiveEntity
            {
                FileName = fileDataSourceEntity.FileName,
                FileExtensionType = fileDataSourceEntity.FileExtensionType,
                PaperSize = fileDataSourceEntity.PaperSize,
                PrinterName = fileDataSourceEntity.PrinterName,
            };
    }
}
