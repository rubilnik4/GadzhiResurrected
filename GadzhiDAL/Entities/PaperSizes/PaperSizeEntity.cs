using System.Collections.Generic;
using GadzhiDAL.Entities.FilesConvert;
// ReSharper disable VirtualMemberCallInConstructor

namespace GadzhiDAL.Entities.PaperSizes
{
    /// <summary>
    /// Формат
    /// </summary>
    public class PaperSizeEntity
    {
        public PaperSizeEntity()
        { }

        public PaperSizeEntity(string paperSize)
        {
            PaperSize = paperSize;
        }

        /// <summary>
        /// Наименование формата
        /// </summary>
        public virtual string PaperSize { get; protected set; }
    }
}