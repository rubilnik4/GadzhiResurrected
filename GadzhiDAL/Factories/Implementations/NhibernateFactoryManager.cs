using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using GadzhiCommonServer.Infrastructure.Implementations;
using GadzhiDAL.Mappings.FilesConvert;
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiDAL.Factories.Implementations
{
    //https://github.com/BrewingCoder/NhibernateUoWRepoPattern
    /// <summary>
    /// Фабрика для создания сессии подключения к БД
    /// </summary>
    public static class NhibernateFactoryManager
    {
        private static Lazy<ISessionFactory> _lazySessionFactory;

        public static ISessionFactory Instance(FluentConfiguration config)
        {
            if (_lazySessionFactory != null && _lazySessionFactory.IsValueCreated)
                return _lazySessionFactory.Value;

            _lazySessionFactory = new Lazy<ISessionFactory>(config.BuildSessionFactory);
            return _lazySessionFactory.Value;
        }

        public static FluentConfiguration SQLiteConfigurationFactory(string dataBasePath)
        {
            string directoryPath = Path.GetDirectoryName(dataBasePath);
            if (!Directory.Exists(directoryPath))
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
