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
            string dir = Environment.CurrentDirectory + "\\01.dgn";
            micro.ConvertingFile(new FileDataMicrostation(dir,
                                                          dir,
                                                          GadzhiMicrostation.Models.Enum.ColorPrint.BlackAndWhite));

            var microError = container.Resolve<IErrorMessagingMicrostation>();
            //var applicationConverting = container.Resolve<IApplicationConverting>();

            //applicationConverting.StartConverting();

            Console.ReadLine();
        }
    }
}
