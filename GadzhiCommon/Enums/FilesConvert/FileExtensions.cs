using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiCommon.Enums.FilesConvert
{
    /// <summary>
    /// Типы допустимых расширений
    /// </summary>
    [Flags]
    public enum FileExtensions
    {
        dgn,
        docx,
        pdf,
        dwg,
        xlsx,
    }
}
