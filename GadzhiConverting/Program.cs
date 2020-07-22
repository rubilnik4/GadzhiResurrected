using GadzhiConverting.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiConverting.DependencyInjection;
using GadzhiConverting.Helpers;
using Unity;

namespace GadzhiConverting
{
    internal class Program
    {
        /// <summary>
        /// Контейнер инверсии зависимости
        /// </summary>
        private static readonly IUnityContainer Container = new UnityContainer();

        [Logger("----------Start application----------")]
        private static void Main()
        {
            NativeMethods.Handler += Handler;
            NativeMethods.SetConsoleCtrlHandler(NativeMethods.Handler, true);

            BootStrapUnity.ConfigureContainer(Container);

            var applicationConverting = Container.Resolve<IConvertingService>();
            applicationConverting.StartConverting();

            Console.ReadLine();
        }

        /// <summary>
        /// Событие закрытия окна
        /// </summary>
        [Logger("----------Close application----------")]
        private static bool Handler(NativeMethods.CtrlType sig)
        {
            Container.Dispose();
            return false;
        }
    }
}
