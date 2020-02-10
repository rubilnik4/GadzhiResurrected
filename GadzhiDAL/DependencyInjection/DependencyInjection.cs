using GadzhiDAL.Factories.Implementations;
using GadzhiDAL.Factories.Interfaces;
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
        public static void ConfigureContainer(IUnityContainer container, string applicationPath)
        {
            container
                 //регистрируем фабрику, синглтон
                 .RegisterFactory<ISessionFactory>((unity) =>
                      NhibernateFactoryManager.Instance(NhibernateFactoryManager.SQLiteConfigurationFactory(applicationPath)), 
                                                        new ContainerControlledLifetimeManager())

                 // с помощью фабрики открываем сессию
                 .RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager())

                 // подключаем сессию для репозитория
                 .RegisterFactory<ISession>((unity) =>
                      unity.Resolve<IUnitOfWork>().GetCurrentSession(), new HierarchicalLifetimeManager())

                 // Репозиторий в общем виде
                 .RegisterType(typeof(IRepository<>), typeof(Repository<>))

                 .RegisterType<IFilesDataService, FilesDataService>();

        }
    }
}
