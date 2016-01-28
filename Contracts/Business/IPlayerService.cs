using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Entities;

namespace Contracts.Business
{
    public interface IPlayerService
    {
        Player Get(Expression<Func<Player, bool>> where);
        IEnumerable<Player> GetList();
        IEnumerable<Player> GetCollection(Expression<Func<Player, bool>> where);
        void Save(Player player);
    }
}
