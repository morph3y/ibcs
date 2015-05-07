using System;
using System.Linq.Expressions;

using Entities;

namespace Contracts.Business
{
    public interface IPlayerService
    {
        Player Get(Expression<Func<Player, bool>> where);
        void Save(Player player);
    }
}
