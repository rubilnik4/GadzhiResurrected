using GadzhiDAL.Factories.Implementations;
using GadzhiDAL.Factories.Interfaces;
using GadzhiDAL.Services.Implementations;
using GadzhiDAL.Services.Implementations.FileConvert;
using GadzhiDAL.Services.Implementations.ServerStates;
using GadzhiDAL.Services.Interfaces;
using GadzhiDAL.Services.Interfaces.FileConvert;
using GadzhiDAL.Services.Interfaces.ServerStates;
using NHibernate;
using Unity;
using Unity.Lifetime;

namespace GadzhiDAL.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void ConfigureContainer(IUnityContainer container)
        {
            container.
            RegisterFactory<ISessionFactory>(unity =>
                NHibernateFactoryManager.Instance(NHibernateFactoryManager.SqLiteConfigurationFactory()),
                                                  new ContainerControlledLifetimeManager()).
            RegisterType<IUnitOfWork, UnitOfWork>().
            RegisterType<IFilesDataServerService, FilesDataServerService>(new HierarchicalLifetimeManager()).
            RegisterType<IFilesDataClientService, FilesDataClientService>(new HierarchicalLifetimeManager()).
            RegisterType<ISignaturesService, SignaturesService>(new HierarchicalLifetimeManager()).
            RegisterType<IServerStateService, ServerStateService>(new HierarchicalLifetimeManager()).
            RegisterType<IServerInfoService, ServerInfoService>(new HierarchicalLifetimeManager()).
            RegisterType<IServerAccessService, ServerAccessService>(new HierarchicalLifetimeManager());
        }
    }
}
