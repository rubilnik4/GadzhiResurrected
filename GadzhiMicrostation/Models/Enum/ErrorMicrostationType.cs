using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Models.Enum
{
    /// <summary>
    /// Тип ошибки при конвертации Microstation
    /// </summary>
    public enum ErrorMicrostationType
    {
        ApplicationLoad,
        DesingFileOpen,
        StampNotFound,
        FileNotFound,
        IncorrectExtension,
        UnknownError,
        NullReference,
        ArgumentNullReference
    }
}
