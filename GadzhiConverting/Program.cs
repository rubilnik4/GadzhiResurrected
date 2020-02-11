using DependencyInjection.GadzhiConverting;
using GadzhiConverting.Infrastructure.Implementations;
using GadzhiConverting.Infrastructure.Interfaces;
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
           
            var applicationConverting = container.Resolve<IApplicationConverting>();

            try
            {
                applicationConverting.StartConverting();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                Console.Write(ex.StackTrace);
            }          

            Console.ReadLine();
        }
    }
}
