using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Dal.Contracts;

using NHibernate.Criterion;

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

        public IEnumerable<TRoot> GetCollection<TRoot, TSubType>(QueryOver<TRoot, TSubType> query)
        {
            return query.GetExecutableQueryOver(DbSessionManager.CurrentSession).List<TRoot>();
        } 

        public IEnumerable<T> GetCollection<T>() where T : class
        {
            return DbSessionManager.CurrentSession.QueryOver<T>().List<T>();
        }

        public IEnumerable<T> GetCollection<T>(Expression<Func<T, bool>> where) where T : class
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
