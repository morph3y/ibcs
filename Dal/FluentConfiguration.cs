using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace Dal
{
    public class FluentConfiguration
    {
        private FluentConfiguration() { }

        private static FluentConfiguration _instance;
        public static FluentConfiguration Instance
        {
            get { return _instance ?? (_instance = new FluentConfiguration()); }
        }


        public FluentNHibernate.Cfg.FluentConfiguration GetConfiguration()
        {
            return Fluently.Configure()
                .Database(
                    MsSqlConfiguration.MsSql2008.ConnectionString(x => x
                        .Server("LOCALHOST")
                        .Database("ibcs")
                        .Username("sa")
                        .Password("sa")))
                .Mappings(x => x.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()));
        }

        public ISessionFactory GetSessionFactory()
        {
            return GetConfiguration().BuildSessionFactory();
        }

        private void ValidateOrCreateSchema()
        {
            GetConfiguration().ExposeConfiguration((config) =>
            {
                new SchemaUpdate(config).Execute(false, true);
            });
        }
    }
}
