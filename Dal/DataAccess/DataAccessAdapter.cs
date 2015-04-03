using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Contracts.Dal;
using NHibernate;

namespace Dal.DataAccess
{
    internal sealed class DataAccessAdapter : IDataAccessAdapter
    {
        private ISession _session;
        private ISession Session
        {
            get
            {
                return _session ?? (_session = DbSessionManager.CurrentSession);
            }
        }

        public void Save(object entity)
        {
            using (var tran = Session.BeginTransaction())
            {
                Session.SaveOrUpdate(entity);
                tran.Commit();
            }
        }

        public T Get<T>(object id) where T : class
        {
            return Session.Get<T>(id);
        }
    
        public IEnumerable<T> Get<T>(Expression<Func<T, bool>> where) where T : class
        {
            return Session.QueryOver<T>().Where(where).List<T>();
        }

        public void Delete(object entity)
        {
            using (var tran = Session.BeginTransaction())
            {
                Session.Delete(entity);
                tran.Commit();
            }
        }
    }
}
