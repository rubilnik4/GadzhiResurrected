using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace GadzhiMicrostation.Helpers.Implementations
{
    /// <summary>
    /// Работа с системой
    /// </summary>
    internal static class NativeMethods
    {
        /// <summary>
        /// Установить принтер по умолчанию
        /// </sum>mary>        
        [DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool SetDefaultPrinter(string printerName);
    }
}
