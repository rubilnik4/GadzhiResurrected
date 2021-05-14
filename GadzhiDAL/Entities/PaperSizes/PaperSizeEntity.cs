using System.Collections.Generic;
using GadzhiDAL.Entities.FilesConvert;

namespace GadzhiDAL.Entities.PaperSizes
{
    /// <summary>
    /// Формат
    /// </summary>
    public class PaperSizeEntity
    {
        /// <summary>
        /// Наименование формата
        /// </summary>
        public virtual string PaperSize { get; set; }
    }
}