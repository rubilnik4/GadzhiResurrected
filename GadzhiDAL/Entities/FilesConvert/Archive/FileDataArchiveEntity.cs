using GadzhiDAL.Entities.FilesConvert.Base;
using System.Collections.Generic;
using System.Linq;
using GadzhiDAL.Entities.FilesConvert.Main.Components;

namespace GadzhiDAL.Entities.FilesConvert.Archive
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах в базе данных
    /// </summary>
    public class FileDataArchiveEntity : FileDataEntityBase
    {
        /// <summary>
        /// Тип ошибки при конвертации файла
        /// </summary>
        public virtual IList<ErrorComponent> FileErrorsArchive { get; set; }

        /// <summary>
        /// Файлы отконвертированных данных в формате zip GZipStream
        /// </summary>      
        public virtual IList<FileDataSourceArchiveEntity> FileDataSourceServerArchiveEntities { get; protected set; }

        /// <summary>
        /// Ссылка на родительский класс
        /// </summary>
        public virtual PackageDataArchiveEntity PackageDataArchiveEntity { get; set; }        

        /// <summary>
        /// Поместить файлы в пакет для конвертирования и присвоить ссылки
        /// </summary>      
        public virtual void SetFileDataSourceArchiveEntities(IEnumerable<FileDataSourceArchiveEntity> fileDataSourceArchiveEntities)
        {
            FileDataSourceServerArchiveEntities = fileDataSourceArchiveEntities?.Select(fileDataSourceArchiveEntity =>
            {
                fileDataSourceArchiveEntity.FileDataArchiveEntity = this;
                return fileDataSourceArchiveEntity;
            }).ToList();
        }
    }
}
