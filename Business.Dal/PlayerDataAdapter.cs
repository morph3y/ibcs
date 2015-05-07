using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Contracts.Business.Dal;

using Dal.Contracts;

using Entities;

namespace Business.Dal
{
    internal sealed class PlayerDataAdapter : IPlayerDataAdapter
    {
        private readonly IDataAccessAdapter _dataAccessAdapter;
        public PlayerDataAdapter(IDataAccessAdapter dataAccessAdapter)
        {
            _dataAccessAdapter = dataAccessAdapter;
        }

        public Player Get(Expression<Func<Player, bool>> where)
        {
            return _dataAccessAdapter.GetCollection(where).FirstOrDefault();
        }

        public IEnumerable<Player> GetCollection(Expression<Func<Player, bool>> where)
        {
            return _dataAccessAdapter.GetCollection(where);
        }

        public void Save(Player entity)
        {
            _dataAccessAdapter.Save(entity);
        }
    }
}
