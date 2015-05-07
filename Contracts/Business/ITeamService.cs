using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Entities;

namespace Contracts.Business
{
    public interface ITeamService
    {
        IEnumerable<Team> GetList();
        IEnumerable<Team> GetCollection(Expression<Func<Team, bool>> where);
        IEnumerable<Player> GetAvailableMembers(int teamId);
        Team Get(Expression<Func<Team, bool>> where);
        void Save(Team entity);

        void AddMember(int teamId, int memberId);
        void AddMember(int teamId, Player member);
        void RemoveMember(int teamId, int memberId);
    }
}
