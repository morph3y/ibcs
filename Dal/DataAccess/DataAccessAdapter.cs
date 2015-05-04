using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.UI.WebControls;
using Contracts.Dal;
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

        public T Get<T>(object id) where T : class
        {
            return DbSessionManager.CurrentSession.Get<T>(id);
        }

        public IEnumerable<T> GetCollection<T>() where T : class
        {
            return DbSessionManager.CurrentSession.QueryOver<T>().List<T>();
        }

        public IEnumerable<T> GetCollection<T>(Expression<Func<T, bool>> where) where T : class
        {
            return DbSessionManager.CurrentSession.QueryOver<T>().Where(where).List<T>();
        }

        public IEnumerable<T> GetCollectionJoin<T, TSubType>(
            Expression<Func<T, IEnumerable<TSubType>>> @on,
            Expression<Func<T, bool>> whereType = null,
            Expression<Func<TSubType, bool>> whereSubType = null
            ) where T : class where TSubType : class
        {
            var detached = QueryOver.Of<T>();
            if (whereType != null)
            {
                detached.Where(whereType);
            }

            var detachedJoin = detached.Inner.JoinQueryOver<TSubType>(@on);

            if (whereSubType != null)
            {
                detachedJoin.Where(whereSubType);
            }

            return detached.GetExecutableQueryOver(DbSessionManager.CurrentSession).List<T>();
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
