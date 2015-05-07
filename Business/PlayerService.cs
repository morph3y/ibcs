using System;
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

        public Player Get(Expression<Func<Player, bool>> where)
        {
            return _playerDataAdapter.Get(where);
        }
    }
}
