using DependencyInjection.GadzhiConverting;
using GadzhiConverting.Infrastructure.Implementations;
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiMicrostation.Infrastructure.Interface;
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
            micro.ConvertingFile("C:\\visual\\dwgToDgN\\testConverted\\01.dgn");
            //var applicationConverting = container.Resolve<IApplicationConverting>();

            //applicationConverting.StartConverting();

            Console.ReadLine();
        }
    }
}
