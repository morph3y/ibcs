using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Dal
{
    public static class FluentConfiguration
    {
        public static ISessionFactory GetSessionFactory()
        {
            return Fluently.Configure()
                .Database(
                    MsSqlConfiguration.MsSql2008.ConnectionString(x=>x
                        .Server("LOCALHOST")
                        .Database("ibcs")
                        .Username("sa")
                        .Password("sa")))
                .Mappings(x=>x.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                .ExposeConfiguration(GenerateSchema)
                .BuildSessionFactory();
        }

        private static void GenerateSchema(Configuration obj)
        {
            new SchemaUpdate(obj).Execute(false, true);
        }
    }
}
