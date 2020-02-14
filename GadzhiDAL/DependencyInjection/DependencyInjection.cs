using GadzhiDAL.Factories.Implementations;
using GadzhiDAL.Factories.Interfaces;
using GadzhiDAL.Infrastructure.Implementations.Converters;
using GadzhiDAL.Infrastructure.Interfaces.Converters;
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
                 .RegisterType<IUnitOfWork, UnitOfWork>()

                 // подключаем сессию для репозитория
                 //.RegisterFactory<ISession>((unity) =>
                 //     unity.Resolve<IUnitOfWork>().GetCurrentSession(), new HierarchicalLifetimeManager())

                 // Репозиторий в общем виде
                // .RegisterType(typeof(IRepository<,>), typeof(Repository<,>))

                 .RegisterType<IConverterDataAccessFilesDataFromDTO, ConverterDataAccessFilesDataFromDTO>()
                 .RegisterType<IConverterDataAccessFilesDataToDTO, ConverterDataAccessFilesDataToDTO>();

            if (isServerPart)
            {
                container
                  .RegisterType<IFilesDataServiceServer, FilesDataServiceServer>();
            }
            else
            {
                container
                 .RegisterType<IFilesDataServiceClient, FilesDataServiceClient>();
            }
        }
    }
}
