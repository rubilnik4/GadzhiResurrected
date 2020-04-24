using GadzhiConverting.DependencyInjection.GadzhiConverting;
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiWord.Helpers.Implementations;
using System;
using System.Collections.Generic;
using Unity;

namespace GadzhiConverting
{
    class Program
    {
        private static readonly IUnityContainer _container = new UnityContainer();

        static void Main()
        {
            NativeMethods._handler += new NativeMethods.EventHandler(Handler);
            NativeMethods.SetConsoleCtrlHandler(NativeMethods._handler, true);

            BootStrapUnity.ConfigureContainer(_container);
          
            var applicationConverting = _container.Resolve<IConvertingService>();
            applicationConverting.StartConverting();

            Console.ReadLine();
        }

        /// <summary>
        /// Событие закрытия окна
        /// </summary>
        private static bool Handler(NativeMethods.CtrlType sig)
        {
            switch (sig)
            {
                case NativeMethods.CtrlType.CTRL_C_EVENT:
                case NativeMethods.CtrlType.CTRL_LOGOFF_EVENT:
                case NativeMethods.CtrlType.CTRL_SHUTDOWN_EVENT:
                case NativeMethods.CtrlType.CTRL_CLOSE_EVENT:
                default:
                    {
                        _container.Dispose();
                        return false;
                    }
            }
        }
    }


}
