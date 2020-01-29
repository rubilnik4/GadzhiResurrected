using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiWcfHost.Infrastructure.Implementations;
using GadzhiWcfHost.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity;
using Unity.Wcf;

namespace GadzhiWcfHost.DependencyInjection
{
    /// <summary>
    /// Класс для подключения инверсии зависимостей
    /// </summary>
    public class WcfServiceFactory : UnityServiceHostFactory
    {
        protected override void ConfigureContainer(IUnityContainer container)
        {
            // Регистрируем зависимости
            container
                .RegisterType<IApplicationReceiveAndSend, ApplicationReceiveAndSend>()
                .RegisterType<IFileSystemOperations, FileSystemOperations>();
        }
    }
}