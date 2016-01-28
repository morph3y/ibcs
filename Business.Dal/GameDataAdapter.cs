using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Contracts.Business.Dal;

using Dal.Contracts;

using Entities;

namespace Business.Dal
{
    internal sealed class GameDataAdapter : IGameDataAdapter
    {
        private readonly IDataAccessAdapter _dataAccessAdapter;
        public GameDataAdapter(IDataAccessAdapter dataAccessAdapter)
        {
            _dataAccessAdapter = dataAccessAdapter;
        }

        public Game Get(Expression<Func<Game, bool>> @where)
        {
            return _dataAccessAdapter.GetCollection(where).FirstOrDefault();
        }

        public IEnumerable<Game> GetList()
        {
            return _dataAccessAdapter.GetCollection<Game>();
        }

        public IEnumerable<Game> GetCollection(Expression<Func<Game, bool>> @where)
        {
            return _dataAccessAdapter.GetCollection(where);
        }

        public void Save(Game entity)
        {
            _dataAccessAdapter.Save(entity);
        }
    }
}
