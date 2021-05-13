using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.IO;
using GadzhiDAL.Mappings.FilesConvert;

namespace GadzhiDAL.Factories.Implementations
{
    /// <summary>
    /// Фабрика для создания сессии подключения к БД
    /// </summary>
    public static class NHibernateFactoryManager
    {
        /// <summary>
        /// Фабрика создания подключения
        /// </summary>
        private static Lazy<ISessionFactory> _lazySessionFactory;

        /// <summary>
        /// Загрузить сущность базы
        /// </summary>
        public static ISessionFactory Instance(FluentConfiguration config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            if (_lazySessionFactory != null && _lazySessionFactory.IsValueCreated)
                return _lazySessionFactory.Value;

            _lazySessionFactory = new Lazy<ISessionFactory>(config.BuildSessionFactory);
            return _lazySessionFactory.Value;
        }

        /// <summary>
        /// Параметры для подключения базы данных SqLite
        /// </summary>
        public static FluentConfiguration SqLiteConfigurationFactory() =>
            Fluently.Configure().
            Database(SQLiteConfiguration.Standard.
                     ConnectionString(c => c.FromConnectionStringWithKey("SQLiteConnectionString"))).
            Mappings(m => m.FluentMappings.AddFromAssemblyOf<PackageDataMap>()).
            ExposeConfiguration(c =>
            {
                var schema = new SchemaUpdate(c);
                schema.Execute(false, true);
            });
        
    }
}
