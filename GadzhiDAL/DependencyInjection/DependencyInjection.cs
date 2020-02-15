using GadzhiDAL.Factories.Implementations;
using GadzhiDAL.Factories.Interfaces;
using GadzhiDAL.Infrastructure.Implementations;
using GadzhiDAL.Infrastructure.Implementations.Converters;
using GadzhiDAL.Infrastructure.Implementations.Converters.Client;
using GadzhiDAL.Infrastructure.Implementations.Converters.Server;
using GadzhiDAL.Infrastructure.Interfaces;
using GadzhiDAL.Infrastructure.Interfaces.Converters;
using GadzhiDAL.Infrastructure.Interfaces.Converters.Client;
using GadzhiDAL.Infrastructure.Interfaces.Converters.Server;
using GadzhiDAL.Services.Implementations;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Lifetime;

namespace GadzhiDAL.DependencyInjection
{
    public class GadzhiDALDependencyInjection
    {
        public static void ConfigureContainer(IUnityContainer container,
                                              string dataBasePath,
                                              bool isServerPart)
        {
            container
                 //регистрируем фабрику, синглтон
                 .RegisterFactory<ISessionFactory>((unity) =>
                      NhibernateFactoryManager.Instance(NhibernateFactoryManager.SQLiteConfigurationFactory(dataBasePath)),
                                                        new ContainerControlledLifetimeManager())

                 // с помощью фабрики открываем сессию
                 .RegisterType<IUnitOfWork, UnitOfWork>(); 

            if (isServerPart)
            {
                container
                  .RegisterType<IConverterDataAccessFilesDataFromDTOServer, ConverterDataAccessFilesDataFromDTOServer>()
                  .RegisterType<IConverterDataAccessFilesDataToDTOServer, ConverterDataAccessFilesDataToDTOServer>()               
                  .RegisterType<IFilesDataServiceServer, FilesDataServiceServer>();
            }
            else
            {
                container
                 .RegisterType<IConverterDataAccessFilesDataFromDTOClient, ConverterDataAccessFilesDataFromDTOClient>()
                 .RegisterType<IConverterDataAccessFilesDataToDTOClient, ConverterDataAccessFilesDataToDTOClient>()
                 .RegisterType<IFilesDataServiceClient, FilesDataServiceClient>();
            }
        }
    }
}
