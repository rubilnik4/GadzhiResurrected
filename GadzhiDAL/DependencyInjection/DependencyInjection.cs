using GadzhiDAL.Factories.Implementations;
using GadzhiDAL.Factories.Interfaces;
using GadzhiDAL.Infrastructure.Implementations.Converters.Client;
using GadzhiDAL.Infrastructure.Implementations.Converters.Server;
using GadzhiDAL.Infrastructure.Interfaces.Converters.Client;
using GadzhiDAL.Infrastructure.Interfaces.Converters.Server;
using GadzhiDAL.Services.Implementations;
using NHibernate;
using Unity;
using Unity.Lifetime;

namespace GadzhiDAL.DependencyInjection
{
    public static class GadzhiDALDependencyInjection
    {
        public static void ConfigureContainer(IUnityContainer container,
                                              string dataBasePath)
        {
            container
                 //регистрируем фабрику, синглтон
                 .RegisterFactory<ISessionFactory>((unity) =>
                      NhibernateFactoryManager.Instance(NhibernateFactoryManager.SQLiteConfigurationFactory(dataBasePath)),
                                                        new ContainerControlledLifetimeManager())

                 // с помощью фабрики открываем сессию
                 .RegisterType<IUnitOfWork, UnitOfWork>()

                 //регистрация клиенской части
                 .RegisterType<IConverterDataAccessFilesDataFromDTOServer, ConverterDataAccessFilesDataFromDTOServer>()
                 .RegisterType<IConverterDataAccessFilesDataToDTOServer, ConverterDataAccessFilesDataToDTOServer>()
                 .RegisterType<IFilesDataServerService, FilesDataServerService>()

                 //регистрация серверной части
                 .RegisterType<IConverterDataAccessFilesDataFromDTOClient, ConverterDataAccessFilesDataFromDTOClient>()
                 .RegisterType<IConverterDataAccessFilesDataToDTOClient, ConverterDataAccessFilesDataToDTOClient>()
                 .RegisterType<IFilesDataClientService, FilesDataClientService>();

        }
    }
}
