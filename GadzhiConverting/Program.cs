using GadzhiConverting.DependencyInjection.GadzhiConverting;
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

            //var micro = _container.Resolve<IConvertingFileMicrostation>();
            //string dir = Environment.CurrentDirectory + "\\01.dgn";

            //var pdfPrinter = new PrinterInformationMicrostation("PDFCreator", "GTNG");
            //var printersInformationMicrostation = new PrintersInformationMicrostation(pdfPrinter);
            //micro.ConvertingFile(new FileDataMicrostation(dir,
            //                                              dir,
            //                                              ColorPrintMicrostation.BlackAndWhite),
            //                    printersInformationMicrostation);

            //var word = _container.Resolve<IConvertingFileWord>();
            //string dir = Environment.CurrentDirectory + "\\Converting.gitignore\\01.docx";

            //var pdfPrinters = new List<IPrinterInformation> { new PrinterInformation("PDFCreator", "GTNG") };
            //word.ConvertingFile(new FileDataServer(dir,
            //                                       dir,
            //                                       ColorPrint.BlackAndWhite),
            //                    new PrintersInformation(pdfPrinters));

            //var applicationConverting = _container.Resolve<IApplicationConverting>();
            //applicationConverting.StartConverting();

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
