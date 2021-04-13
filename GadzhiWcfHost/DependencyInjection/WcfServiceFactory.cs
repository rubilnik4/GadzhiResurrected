using System.Configuration;
using GadzhiDAL.DependencyInjection;
using GadzhiDAL.Services.Implementations;
using GadzhiDAL.Services.Interfaces;
using GadzhiDTOClient.Contracts.FilesConvert;
using GadzhiDTOClient.Contracts.Signatures;
using GadzhiDTOServer.Contracts.FilesConvert;
using GadzhiDTOServer.Contracts.Signatures;
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
        /// <summary>
        /// Регистрация сервисов
        /// </summary>
        protected override void ConfigureContainer(IUnityContainer container)
        {
            container.
            RegisterType<IFileConvertingClientService, FileConvertingClientService>(new HierarchicalLifetimeManager()).
            RegisterType<ISignatureClientService, SignatureClientService>(new HierarchicalLifetimeManager()).
            RegisterType<IFileConvertingServerService, FileConvertingServerService>(new HierarchicalLifetimeManager()).
            RegisterType<ISignatureServerService, SignatureServerService>(new HierarchicalLifetimeManager()).
            RegisterType<IApplicationClientConverting, ApplicationClientConverting>(new HierarchicalLifetimeManager()).
            RegisterType<IApplicationServerConverting, ApplicationServerConverting>(new HierarchicalLifetimeManager());

            GadzhiDAL.DependencyInjection.DependencyInjection.ConfigureContainer(container);
        }
    }
}