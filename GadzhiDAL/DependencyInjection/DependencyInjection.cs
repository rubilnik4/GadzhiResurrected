using GadzhiDAL.Factories.Implementations;
using GadzhiDAL.Factories.Interfaces;
using GadzhiDAL.Infrastructure.Implementations.Converters.Archive;
using GadzhiDAL.Infrastructure.Implementations.Converters.Client;
using GadzhiDAL.Infrastructure.Implementations.Converters.Server;
using GadzhiDAL.Services.Implementations;
using GadzhiDAL.Services.Interfaces;
using NHibernate;
using Unity;
using Unity.Lifetime;

namespace GadzhiDAL.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void ConfigureContainer(IUnityContainer container, string dataBasePath)
        {
            container
                 //регистрируем фабрику
                 .RegisterFactory<ISessionFactory>(unity =>
                      NHibernateFactoryManager.Instance(NHibernateFactoryManager.SqLiteConfigurationFactory(dataBasePath)),
                                                        new ContainerControlledLifetimeManager())

                 //с помощью фабрики открываем сессию
                 .RegisterType<IUnitOfWork, UnitOfWork>()

                .RegisterType<IFilesDataServerService, FilesDataServerService>()
                 .RegisterType<IFilesDataClientService, FilesDataClientService>()
                 .RegisterType<ISignaturesService, SignaturesService>(new HierarchicalLifetimeManager());

        }
    }
}
