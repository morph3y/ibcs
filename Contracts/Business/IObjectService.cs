using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Contracts.Business
{
    public interface IObjectService
    {
        void Save(object entity);
        IEnumerable<T> GetCollection<T>() where T : class;
        IEnumerable<T> GetCollection<T>(Expression<Func<T, bool>> where) where T : class;
        T Get<T>(Expression<Func<T, bool>> where) where T : class;
    }
}
