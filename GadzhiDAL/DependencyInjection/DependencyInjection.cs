using GadzhiDAL.Factories.Implementations;
using GadzhiDAL.Factories.Interfaces;
using GadzhiDAL.Services.Implementations;
using GadzhiDAL.Services.Implementations.FileConvert;
using GadzhiDAL.Services.Implementations.Histories;
using GadzhiDAL.Services.Implementations.Likes;
using GadzhiDAL.Services.Implementations.ServerStates;
using GadzhiDAL.Services.Implementations.Signatures;
using GadzhiDAL.Services.Interfaces;
using GadzhiDAL.Services.Interfaces.FileConvert;
using GadzhiDAL.Services.Interfaces.Histories;
using GadzhiDAL.Services.Interfaces.Likes;
using GadzhiDAL.Services.Interfaces.ServerStates;
using GadzhiDAL.Services.Interfaces.Signatures;
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
            RegisterType<IAccessService, AccessService>(new HierarchicalLifetimeManager()).
            RegisterType<IHistoryService, HistoryService>(new HierarchicalLifetimeManager()).
            RegisterType<ILikeService, LikeService>(new HierarchicalLifetimeManager());
        }
    }
}
