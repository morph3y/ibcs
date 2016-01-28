using System.Collections.Generic;
using Entities;

namespace Contracts.Business.Dal
{
    public interface ITeamDataAdapter : IDataAdapter<Team>
    {
        IEnumerable<Player> GetAvailableMembers(int teamId);
        IEnumerable<TeamMemberRequest> GetTeamRequests(int memberId);
        IEnumerable<TeamMemberRequest> GetMembersRequests(int teamId);

        TeamMemberRequest GetRequest(int teamId, int memberId);
        void RemoveRequest(TeamMemberRequest request);
        void CreateRequest(TeamMemberRequest request);
    }
}
