using GadzhiCommon.Enums.FilesConvert;
using GadzhiDAL.Entities.FilesConvert.Base;
using System.Collections.Generic;
using System.Linq;
using GadzhiDAL.Entities.FilesConvert.Base.Components;

namespace GadzhiDAL.Entities.FilesConvert.Main
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах в базе данных
    /// </summary>
    public class FileDataEntity : FileDataEntityBase<FileDataSourceEntity>
    {
        /// <summary>
        /// Конвертируемый файл данных в формате zip GZipStream
        /// </summary>       
        public virtual IList<byte> FileDataSource { get; set; }

        /// <summary>
        /// Расширение дополнительного файла
        /// </summary>       
        public virtual string FileExtensionAdditional { get; set; }

        /// <summary>
        /// Дополнительный файл данных в формате zip GZipStream
        /// </summary>       
        public virtual IList<byte> FileDataSourceAdditional { get; set; }

        /// <summary>
        /// Ссылка на родительский класс
        /// </summary>
        public virtual PackageDataEntity PackageDataEntity { get; set; }

        /// <summary>
        /// Поместить файлы в пакет для конвертирования и присвоить ссылки
        /// </summary>      
        public virtual void SetFileDataSourceEntities(IEnumerable<FileDataSourceEntity> fileDataSourceEntities)
        {
            FileDataSourceServerEntities = fileDataSourceEntities?.Select(fileDataSourceEntity =>
            {
                fileDataSourceEntity.FileDataEntity = this;
                return fileDataSourceEntity;
            }).ToList()
            ?? new List<FileDataSourceEntity>();
        }
    }
}
