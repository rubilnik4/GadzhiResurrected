using GadzhiDAL.Factories.Implementations;
using GadzhiDAL.Factories.Interfaces;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace GadzhiDAL.DependencyInjection
{
    public class GadzhiDALDependencyInjection
    {
        public static void ConfigureContainer(IUnityContainer container)
        {
            container
                 .RegisterType<IUnitOfWork, UnitOfWork>()
                 .RegisterFactory<ISessionFactory>((unity) =>
                      NhibernateFactoryManager.Instance(NhibernateFactoryManager.SQLiteConfigurationFactory));
        }
    }
}
