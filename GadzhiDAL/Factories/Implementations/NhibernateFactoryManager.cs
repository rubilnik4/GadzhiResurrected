using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using GadzhiDAL.Mappings.FilesConvert;
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
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

        public static FluentConfiguration SQLiteConfigurationFactory
        {
            get
            {
                return Fluently.Configure()
                    //.Database(SQLiteConfiguration.Standard.ConnectionString(
                    //          c => c.FromConnectionStringWithKey("SQLiteConnectionString"))
                    //         .ShowSql())
                    .Database(SQLiteConfiguration.Standard.UsingFile("GadzhiSQLite.db"))
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<FilesDataMap>())
                    //.CurrentSessionContext(typeof(ThreadStaticSessionContext).FullName)
                    .ExposeConfiguration(c =>
                    {
                        var schema = new SchemaUpdate(c);
                        schema.Execute(false, true);
                    });
            }
        }
    }   
}
