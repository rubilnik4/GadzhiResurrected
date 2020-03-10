using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace GadzhiWord.Helpers.Implementations
{
    /// <summary>
    /// Работа с системой
    /// </summary>
    internal static class NativeMethods
    {
        /// <summary>
        /// Событие закрытия консоли
        /// </summary>        
        [DllImport("Kernel32")]
        internal static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        internal delegate bool EventHandler(CtrlType sig);
        internal static EventHandler _handler;

        internal enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        /// <summary>
        /// Установить принтер по умолчанию
        /// </sum>mary>        
        [DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool SetDefaultPrinter(string printerName);
    }
}
