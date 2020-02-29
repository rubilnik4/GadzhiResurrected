using GadzhiCommon.Enums.FilesConvert;
using GadzhiDAL.Entities.FilesConvert.Base;
using GadzhiDAL.Entities.FilesConvert.Main;
using System.Collections.Generic;
using System.Linq;

namespace GadzhiDAL.Entities.FilesConvert.Main
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах в базе данных
    /// </summary>
    public class FileDataEntity : FileDataEntityBase
    {
        public FileDataEntity()
        {
            StatusProcessing = StatusProcessing.InQueue;
        }

        /// <summary>
        /// Статус обработки файла
        /// </summary>     
        public virtual StatusProcessing StatusProcessing { get; set; }

        /// <summary>
        /// Конвертируемый файл данных в формате zip GZipStream
        /// </summary>       
        public virtual IList<byte> FileDataSourceClient { get; set; }

        /// <summary>
        /// Файлы отконвертированных данных в формате zip GZipStream
        /// </summary>      
        public virtual IList<FileDataSourceEntity> FileDataSourceServerEntities { get; protected set; }

        /// <summary>
        /// Ссылка на родительский класс
        /// </summary>
        public virtual FilesDataEntity FilesDataEntity { get; set; }        

        /// <summary>
        /// Поместить файлы в пакет для конвертирования и присвоить ссылки
        /// </summary>      
        public virtual void SetFileDataSourceEntities(IEnumerable<FileDataSourceEntity> fileDataSourceEntities)
        {
            FileDataSourceServerEntities = fileDataSourceEntities?.Select(fileDataSourceEntity =>
            {
                fileDataSourceEntity.FileDataEntity = this;
                return fileDataSourceEntity;
            })?.ToList();
        }
    }
}
