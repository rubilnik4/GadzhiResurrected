using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiCommon.Enums.FilesConvert
{
    /// <summary>
    /// Статус обработки файлов
    /// </summary>
    public enum StatusProcessing
    {
        NotSend,
        Sending,
        InQueue,
        Converting,
        Completed,
        Writing,
        End,
        Error,
    }
}
