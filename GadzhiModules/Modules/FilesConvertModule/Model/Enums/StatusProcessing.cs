using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Modules.FilesConvertModule.Model.Enums
{
    /// <summary>
    /// Статус обработки файлов
    /// </summary>
    public enum StatusProcessing
    {
        NotSend,
        InQueue,
        InProcess,
        Complited,
        Error,
    }
}
