using GadzhiConverting.DependencyInjection.GadzhiConverting;
using GadzhiMicrostation.Infrastructure.Interfaces;
using GadzhiMicrostation.Models.Implementations;
using System;
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

            BootStrapUnity.Start(_container);

            var micro = _container.Resolve<IConvertingFileMicrostation>();
            string dir = Environment.CurrentDirectory + "\\01.dgn";
            micro.ConvertingFile(new FileDataMicrostation(dir,
                                                          dir,
                                                          GadzhiMicrostation.Models.Enums.ColorPrint.BlackAndWhite));            
           
            //var applicationConverting = _container.Resolve<IApplicationConverting>();

            //applicationConverting.StartConverting();
            Console.ReadLine();
        }

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
