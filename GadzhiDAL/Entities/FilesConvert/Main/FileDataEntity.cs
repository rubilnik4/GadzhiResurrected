using GadzhiCommon.Enums.FilesConvert;
using GadzhiDAL.Entities.FilesConvert.Base;
using GadzhiDAL.Entities.FilesConvert.Main;
using System.Collections.Generic;
using System.Linq;
using GadzhiDAL.Entities.FilesConvert.Main.Components;

namespace GadzhiDAL.Entities.FilesConvert.Main
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах в базе данных
    /// </summary>
    public class FileDataEntity : FileDataEntityBase
    {
        /// <summary>
        /// Статус обработки файла
        /// </summary>     
        public virtual StatusProcessing StatusProcessing { get; set; } = StatusProcessing.InQueue;

        /// <summary>
        /// Конвертируемый файл данных в формате zip GZipStream
        /// </summary>       
        public virtual IList<byte> FileDataSource { get; set; }

        /// <summary>
        /// Тип ошибки при конвертации файла
        /// </summary>
        public virtual IList<ErrorComponent> FileErrors { get; set; }

        /// <summary>
        /// Файлы отконвертированных данных в формате zip GZipStream
        /// </summary>      
        public virtual IList<FileDataSourceEntity> FileDataSourceServerEntities { get; protected set; }

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
