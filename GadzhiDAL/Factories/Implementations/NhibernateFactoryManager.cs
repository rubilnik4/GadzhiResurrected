using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using GadzhiDAL.Mappings.FilesConvert.Main;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.IO;

namespace GadzhiDAL.Factories.Implementations
{
    //https://github.com/BrewingCoder/NhibernateUoWRepoPattern
    /// <summary>
    /// Фабрика для создания сессии подключения к БД
    /// </summary>
    public static class NHibernateFactoryManager
    {
        private static Lazy<ISessionFactory> _lazySessionFactory;

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
        public static FluentConfiguration SqLiteConfigurationFactory(string dataBasePath)
        {
            string directoryPath = Path.GetDirectoryName(dataBasePath);
            if (!String.IsNullOrWhiteSpace(directoryPath) && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.UsingFile(dataBasePath))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<FilesDataMap>())
                .ExposeConfiguration(c =>
                {
                    var schema = new SchemaUpdate(c);
                    schema.Execute(false, true);
                });
        }
    }
}
