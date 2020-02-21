using System.Runtime.InteropServices;

namespace GadzhiConverting
{
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
    }
}
