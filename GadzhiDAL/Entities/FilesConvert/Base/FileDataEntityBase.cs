using GadzhiCommon.Enums.FilesConvert;
using GadzhiDAL.Entities.FilesConvert.Main;
using System.Collections.Generic;
using System.Linq;

namespace GadzhiDAL.Entities.FilesConvert.Base
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах в базе данных
    /// </summary>
    public abstract class FileDataEntityBase : EntityBase<int>
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
    }
}
