using System.Collections.Generic;
using System.Linq;
using GadzhiDAL.Entities.FilesConvert.Base;
using GadzhiDAL.Entities.FilesConvert.Base.Components;

namespace GadzhiDAL.Entities.FilesConvert.Errors
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах с ошибками в базе данных
    /// </summary>
    public class FileDataErrorEntity : FileDataEntityBase
    {
        /// <summary>
        /// Конвертируемый файл данных в формате zip GZipStream
        /// </summary>       
        public virtual IList<byte> FileDataSourceError { get; set; }

        /// <summary>
        /// Тип ошибки при конвертации файла
        /// </summary>
        public virtual IList<ErrorComponent> FileErrorsStore { get; set; }

        /// <summary>
        /// Ссылка на родительский класс
        /// </summary>
        public virtual PackageDataErrorEntity PackageDataEntityError { get; set; }
    }
}
