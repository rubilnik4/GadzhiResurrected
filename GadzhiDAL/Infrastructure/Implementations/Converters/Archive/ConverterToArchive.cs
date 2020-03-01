using GadzhiDAL.Entities.FilesConvert.Archive;
using GadzhiDAL.Entities.FilesConvert.Main;
using GadzhiDAL.Infrastructure.Interfaces.Converters.Archive;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiDAL.Infrastructure.Implementations.Converters.Archive
{
    /// <summary>
    /// Конвертер в архивную версию
    /// </summary>    
    public class ConverterToArchive : IConverterToArchive
    {
        public ConverterToArchive()
        {

        }

        /// <summary>
        /// Конвертировать пакет в архивную версию базы данных
        /// </summary>    
        public async Task<FilesDataArchiveEntity> ConvertFilesDataToArchive(FilesDataEntity filesDataEntity)
        {
            if (filesDataEntity != null)
            {
                var filesDataArchiveEntity = new FilesDataArchiveEntity()
                {
                    CreationDateTime = filesDataEntity.CreationDateTime,
                    IdentityLocalName = filesDataEntity.IdentityLocalName,
                    IdentityServerName = filesDataEntity.IdentityServerName
                };
                filesDataArchiveEntity.SetId(Guid.Parse(filesDataEntity.Id));

                var fileDataEntitiesTask = filesDataEntity.FileDataEntities?.AsQueryable().
                                           Select(fileData => ConvertFileDataToArchive(fileData));
                var fileDataEntities = await Task.WhenAll(fileDataEntitiesTask);
                filesDataArchiveEntity.SetFileDataArchiveEntities(fileDataEntities);

                return filesDataArchiveEntity;
            }
            else
            {
                throw new ArgumentNullException(nameof(filesDataEntity));
            }
        }

        /// <summary>
        /// Конвертировать файл в архивную версию базы данных
        /// </summary>    
        public async Task<FileDataArchiveEntity> ConvertFileDataToArchive(FileDataEntity fileDataEntity)
        {
            if (fileDataEntity != null)
            {
                var fileDataArchiveEntity = new FileDataArchiveEntity()
                {
                    ColorPrint = fileDataEntity.ColorPrint,
                    FilePath = fileDataEntity.FilePath,
                    FileConvertErrorTypeArchive = fileDataEntity.FileConvertErrorType.AsQueryable().ToList()
                };
                var fileDataSourceServerEntities = await fileDataEntity.FileDataSourceServerEntities.AsQueryable().
                                                   Select(fileDataSource => ConvertFileDataSourceToArchive(fileDataSource)).ToListAsync();
                fileDataArchiveEntity.SetFileDataSourceArchiveEntities(fileDataSourceServerEntities);

                return fileDataArchiveEntity;
            }
            else
            {
                throw new ArgumentNullException(nameof(fileDataEntity));
            }
        }

        /// <summary>
        /// КОнвертироват информацию о готовых файлах в архивную версию
        /// </summary>     
        public FileDataSourceArchiveEntity ConvertFileDataSourceToArchive(FileDataSourceEntity fileDataSourceEntity) =>
            new FileDataSourceArchiveEntity()
            {
                FileName = fileDataSourceEntity?.FileName,
                PaperSize = fileDataSourceEntity?.PaperSize,
                PrinterName = fileDataSourceEntity?.PrinterName,
            };
    }
}
