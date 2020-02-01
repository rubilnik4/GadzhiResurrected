using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiCommon.Enums.FilesConvert
{
    /// <summary>
    /// Статус обработки пакета
    /// </summary>
    public enum StatusProcessingProject
    {
        NeedToLoadFiles,
        NeedToStartConverting,
        Sending,
        InQueue,
        Converting,
        Receiving,
        Wrighting,
        End,
        Error,
    }
}
