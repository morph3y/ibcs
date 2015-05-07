using System;
using System.Collections.Generic;

using Entities;

namespace Contracts.Business.Dal
{
    public interface ITeamDataAdapter : IDataAdapter<Team>
    {
        IEnumerable<Team> GetCollection();
        IEnumerable<Player> GetAvailableMembers(int teamId);
    }
}
