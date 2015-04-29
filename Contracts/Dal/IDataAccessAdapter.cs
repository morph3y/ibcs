using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Contracts.Dal
{
    public interface IDataAccessAdapter
    {
        void Save(object entity);

        T Get<T>(object id) where T : class;
        IEnumerable<T> GetCollection<T>() where T : class;
        IEnumerable<T> GetCollection<T>(Expression<Func<T, bool>> where) where T : class;
        void Delete(object entity);
    }
}
