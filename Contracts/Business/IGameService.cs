using System;
using System.Linq.Expressions;

using Entities;

namespace Contracts.Business
{
    public interface IGameService
    {
        Game Get(Expression<Func<Game, bool>> where);
        void EndGame(Game game);
    }
}
