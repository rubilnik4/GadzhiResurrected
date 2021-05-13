using System.Collections.Generic;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiDAL.Entities.Base;
using GadzhiDAL.Entities.PaperSizes;

namespace GadzhiDAL.Entities.FilesConvert
{
    /// <summary>
    /// Информация об отконвертированных файлах в базе данных
    /// </summary>
    public class FileDataSourceEntity: EntityBase<int>
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public override int Id { get; protected set; }

        /// <summary>
        /// Имя и расширение файла
        /// </summary>       
        public virtual string FileName { get; set; }

        /// <summary>
        /// Путь файла
        /// </summary>      
        public virtual FileExtensionType FileExtensionType { get; set; }

        /// <summary>
        /// Формат печати
        /// </summary>
        public virtual IList<PaperSizeEntity> PaperSizes { get; set; }

        /// <summary>
        /// Имя принтера
        /// </summary>
        public virtual string PrinterName { get; set; }

        /// <summary>
        /// Файл данных в формате zip GZipStream
        /// </summary>       
        public virtual IList<byte> FileDataSource { get; set; }

        /// <summary>
        /// Ссылка на родительский класс
        /// </summary>
        public virtual FileDataEntity FileDataEntity { get; set; }
    }
}
