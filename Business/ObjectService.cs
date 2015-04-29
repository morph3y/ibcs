using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Contracts.Business;
using Contracts.Dal;

namespace Business
{
    internal sealed class ObjectService : IObjectService
    {
        private readonly IDataAccessAdapter _dataAccessAdapter;
        public ObjectService(IDataAccessAdapter dataAccessAdapter)
        {
            _dataAccessAdapter = dataAccessAdapter;
        }

        public void Save(object entity)
        {
            _dataAccessAdapter.Save(entity);
        }

        public IEnumerable<T> GetCollection<T>() where T : class
        {
            return _dataAccessAdapter.GetCollection<T>();
        }

        public IEnumerable<T> GetCollection<T>(Expression<Func<T, bool>> where) where T : class
        {
            return _dataAccessAdapter.GetCollection(where);
        }
        
        public T Get<T>(Expression<Func<T, bool>> where) where T : class
        {
            return _dataAccessAdapter.GetCollection(where).FirstOrDefault();
        }
    }
}
