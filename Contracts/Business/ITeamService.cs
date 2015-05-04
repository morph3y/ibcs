using System.Collections.Generic;
using Entities;

namespace Contracts.Business
{
    public interface ITeamService
    {
        IEnumerable<Player> GetAvailableMembers(int teamId);
        void AddMember(int teamId, int memberId);
        void AddMember(int teamId, Player member);
        void RemoveMember(int teamId, int memberId);
    }
}
