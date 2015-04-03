﻿using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;

namespace Dal
{
    public class FluentConfiguration
    {
        public static FluentNHibernate.Cfg.FluentConfiguration GetConfiguration()
        {
            return Fluently.Configure()
                .Database(
                    MsSqlConfiguration.MsSql2008.ConnectionString(x => x
                        .Server("LOCALHOST")
                        .Database("ibcs")
                        .Username("sa")
                        .Password("sa")))
                .Mappings(x => x.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
#if DEBUG
                .ExposeConfiguration((config) => { new SchemaExport(config).Create(false, true); })
#endif
                ;
        }
    }
}
