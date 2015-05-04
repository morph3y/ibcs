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

        IEnumerable<TRoot> GetColectionJoin<TRoot, TSubType>(Expression<Func<TRoot, IEnumerable<TSubType>>> @on, 
            Expression<Func<TRoot, bool>> whereType = null,
            Expression<Func<TSubType, bool>> whereSubType = null
            ) where TRoot : class where TSubType : class;

        T Get<T>(Expression<Func<T, bool>> where) where T : class;
    }
}
