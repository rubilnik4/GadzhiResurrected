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
        private static readonly IUnityContainer _container = new UnityContainer();

        [Logger("----------Start application----------")]
        private static void Main()
        {
            NativeMethods.Handler += Handler;
            NativeMethods.SetConsoleCtrlHandler(NativeMethods.Handler, true);

            BootStrapUnity.ConfigureContainer(_container);

            var applicationConverting = _container.Resolve<IConvertingService>();
            applicationConverting.StartConverting();

            Console.ReadLine();
        }

        /// <summary>
        /// Событие закрытия окна
        /// </summary>
        [Logger("----------Close application----------")]
        private static bool Handler(NativeMethods.CtrlType sig)
        {
            _container.Dispose();
            return false;
        }
    }
}
