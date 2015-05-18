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
        Team Get(Expression<Func<Team, bool>> where);
        void Save(Team entity);

        IEnumerable<Player> GetAvailableMembers(int teamId);

        IEnumerable<TeamMemberRequest> GetMembersRequests(int teamId);
        IEnumerable<TeamMemberRequest> GetTeamsRequests(int memberId);

        void AddMember(int teamId, int memberId);
        void AddMember(int teamId, Player member);
        void AcceptMember(int teamId, Player member);
        void RemoveMember(int teamId, int memberId);
    }
}
