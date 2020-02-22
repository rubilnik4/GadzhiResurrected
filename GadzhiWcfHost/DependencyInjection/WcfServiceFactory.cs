using GadzhiDAL.DependencyInjection;
using GadzhiDTOClient.Contracts.FilesConvert;
using GadzhiDTOServer.Contracts.FilesConvert;
using GadzhiWcfHost.Helpers;
using GadzhiWcfHost.Infrastructure.Implementations.Client;
using GadzhiWcfHost.Infrastructure.Implementations.Server;
using GadzhiWcfHost.Infrastructure.Interfaces.Client;
using GadzhiWcfHost.Infrastructure.Interfaces.Server;
using GadzhiWcfHost.Services;
using Unity;
using Unity.Lifetime;
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
                .RegisterType<IFileConvertingClientService, FileConvertingClientService>(new HierarchicalLifetimeManager())
                .RegisterType<IFileConvertingServerService, FileConvertingServerService>(new HierarchicalLifetimeManager())
                .RegisterType<IAuthentication, Authentication>(new HierarchicalLifetimeManager())
                .RegisterType<IApplicationClientConverting, ApplicationClientConverting>(new HierarchicalLifetimeManager())
                .RegisterType<IApplicationServerConverting, ApplicationServerConverting>(new HierarchicalLifetimeManager());

            GadzhiDALDependencyInjection.ConfigureContainer(container,
                                                            HostSystemInformation.DataBasePath);
        }

    }
}