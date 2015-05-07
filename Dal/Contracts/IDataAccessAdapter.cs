using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using NHibernate.Criterion;

namespace Dal.Contracts
{
    public interface IDataAccessAdapter
    {
        IEnumerable<TRoot> GetCollection<TRoot, TSubType>(QueryOver<TRoot, TSubType> query);

        void Save(object entity);

        IEnumerable<T> GetCollection<T>() where T : class;
        IEnumerable<T> GetCollection<T>(Expression<Func<T, bool>> where) where T : class;

        void Delete(object entity);
    }
}
