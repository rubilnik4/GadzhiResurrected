using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiDAL.Entities.FilesConvert.Base
{
    /// <summary>
    /// Информация об отконвертированных файлах в базе данных
    /// </summary>
    public abstract class FileDataSourceEntityBase: EntityBase<int>
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
        /// Формат печати
        /// </summary>
        public virtual string PaperSize { get; set; }

        /// <summary>
        /// Имя принтера
        /// </summary>
        public virtual string PrinterName { get; set; }
    }
}
