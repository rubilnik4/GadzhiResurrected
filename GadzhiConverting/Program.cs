using DependencyInjection.GadzhiConverting;
using GadzhiConverting.Infrastructure.Implementations;
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiMicrostation.Infrastructure.Interface;
using GadzhiMicrostation.Infrastructure.Interfaces;
using GadzhiMicrostation.Models.Implementations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace GadzhiConverting
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new UnityContainer();
            BootStrapUnity.Start(container);

            var micro = container.Resolve<IConvertingFileMicrostation>();
            micro.ConvertingFile(new FileDataMicrostation ("C:\\visual\\dwgToDgN\\testConverted\\01.dgn",
                                                           "C:\\visual\\dwgToDgN\\testConverted\\01.dgn",
                                                            GadzhiMicrostation.Models.Enum.ColorPrint.BlackAndWhite));

            var microError = container.Resolve<IErrorMessagingMicrostation>();
            //var applicationConverting = container.Resolve<IApplicationConverting>();

            //applicationConverting.StartConverting();

            Console.ReadLine();
        }
    }
}
