using System;
using System.Linq.Expressions;

using Contracts.Business;
using Contracts.Business.Dal;

using Entities;

namespace Business
{
    internal sealed class GameService : IGameService
    {
        private readonly IGameDataAdapter _gameDataAdapter;
        public GameService(IGameDataAdapter gameDataAdapter)
        {
            _gameDataAdapter = gameDataAdapter;
        }

        public Game Get(Expression<Func<Game, bool>> @where)
        {
            return _gameDataAdapter.Get(where);
        }
    }
}
