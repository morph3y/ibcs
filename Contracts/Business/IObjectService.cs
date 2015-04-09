using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Contracts.Business
{
    public interface IObjectService
    {
        void Save(object entity);
        IEnumerable<T> Get<T>(Expression<Func<T, bool>> where) where T : class;
        T GetFirst<T>(Expression<Func<T, bool>> where) where T : class;
    }
}
