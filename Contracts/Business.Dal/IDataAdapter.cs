using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Contracts.Business.Dal
{
    public interface IDataAdapter<T>
    {
        T Get(Expression<Func<T, bool>> where);
        IEnumerable<T> GetList();
        IEnumerable<T> GetCollection(Expression<Func<T, bool>> where);
        void Save(T entity);
    }
}
