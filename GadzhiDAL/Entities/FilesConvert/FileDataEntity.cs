using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiDAL.Entities.Base;
using GadzhiDAL.Entities.FilesConvert.Base.Components;

namespace GadzhiDAL.Entities.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах в базе данных
    /// </summary>
    public class FileDataEntity : EntityBase<int>
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public override int Id { get; protected set; }

        /// <summary>
        /// Путь файла
        /// </summary>      
        public virtual string FilePath { get; set; }

        /// <summary>
        /// Цвет печати
        /// </summary>       
        public virtual ColorPrintType ColorPrintType { get; set; }

        /// <summary>
        /// Статус обработки файла
        /// </summary>     
        public virtual StatusProcessing StatusProcessing { get; set; }

        /// <summary>
        /// Файлы отконвертированных данных в формате zip GZipStream
        /// </summary>      
        public virtual IList<FileDataSourceEntity> FileDataSourceServerEntities { get; protected set; }

        /// <summary>
        /// Тип ошибки при конвертации файла
        /// </summary>
        public virtual IList<ErrorComponent> FileErrors { get; set; }

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
