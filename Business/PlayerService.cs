using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Contracts.Business;
using Contracts.Business.Dal;

using Entities;

namespace Business
{
    internal sealed class PlayerService : IPlayerService
    {
        private readonly IPlayerDataAdapter _playerDataAdapter;
        public PlayerService(IPlayerDataAdapter playerDataAdapter)
        {
            _playerDataAdapter = playerDataAdapter;
        }

        public void Save(Player player)
        {
            _playerDataAdapter.Save(player);
        }

        public IEnumerable<Player> GetList()
        {
            return _playerDataAdapter.GetList();
        }

        public IEnumerable<Player> GetCollection(Expression<Func<Player, bool>> where)
        {
            return _playerDataAdapter.GetCollection(where);
        } 

        public Player Get(Expression<Func<Player, bool>> where)
        {
            return _playerDataAdapter.Get(where);
        }
    }
}
