using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Contracts.Dal;
using NHibernate;

namespace Dal.DataAccess
{
    internal sealed class DataAccessAdapter : IDataAccessAdapter
    {
        public void Save(object entity)
        {
            using (var tran = DbSessionManager.CurrentSession.BeginTransaction())
            {
                DbSessionManager.CurrentSession.SaveOrUpdate(entity);
                tran.Commit();
            }
        }

        public T Get<T>(object id) where T : class
        {
            return DbSessionManager.CurrentSession.Get<T>(id);
        }
    
        public IEnumerable<T> Get<T>(Expression<Func<T, bool>> where) where T : class
        {
            return DbSessionManager.CurrentSession.QueryOver<T>().Where(where).List<T>();
        }

        public void Delete(object entity)
        {
            using (var tran = DbSessionManager.CurrentSession.BeginTransaction())
            {
                DbSessionManager.CurrentSession.Delete(entity);
                tran.Commit();
            }
        }
    }
}
