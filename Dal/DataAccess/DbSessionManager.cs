using System.Web;
using Contracts.Dal;
using NHibernate;
using NHibernate.Context;

namespace Dal.DataAccess
{
    internal sealed class DbSessionManager : IDbSessionManager
    {
        private static ISessionFactory _dbSessionFactory;
        private static ISessionFactory GetFactory<T>() where T : ICurrentSessionContext
        {
            return FluentConfiguration.GetConfiguration().CurrentSessionContext<T>().BuildSessionFactory();
        }

        public static ISession CurrentSession
        {
            get
            {
                if (_dbSessionFactory == null)
                {
                    _dbSessionFactory = HttpContext.Current != null
                        ? GetFactory<WebSessionContext>()
                        : GetFactory<ThreadStaticSessionContext>();
                }

                if (CurrentSessionContext.HasBind(_dbSessionFactory))
                {
                    return _dbSessionFactory.GetCurrentSession();
                }

                var session = _dbSessionFactory.OpenSession();
                CurrentSessionContext.Bind(session);
                return session;
            }
        }

        public static void Close()
        {
            if (_dbSessionFactory == null)
            {
                return;
            }

            if(CurrentSessionContext.HasBind(_dbSessionFactory))
            {
                var session = CurrentSessionContext.Unbind(_dbSessionFactory);
                session.Close();
            }
        }

        public static void Commit(ISession sesssionToCommit)
        {
            try
            {
                sesssionToCommit.Transaction.Commit();
            }
            catch
            {
                sesssionToCommit.Transaction.Rollback();
                throw;
            }
        }

        void IDbSessionManager.Close()
        {
            Close();
        }
    }
}
