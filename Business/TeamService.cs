using System;
using System.Collections.Generic;
using System.Linq;
using Contracts.Business;
using Contracts.Session;
using Entities;

namespace Business
{
    internal sealed class TeamService : ITeamService
    {
        private readonly IObjectService _objectService;
        public TeamService(IObjectService objectService)
        {
            _objectService = objectService;
        }

        public IEnumerable<Player> GetAvailableMembers(int teamId)
        {
            var team = _objectService.Get<Team>(x => x.Id == teamId);
            var captainId = team == null || team.Captain == null ? -1 : team.Captain.Id;
            
            // TODO (CRAP): Need to build join API
            var members = _objectService.GetCollection<Player>(x => x.Id != captainId);
            if (team != null && team.Captain != null)
            {
                members = members.Where(x => !team.Members.Contains(x));
            }

            return members;
        }

        public void AddMember(int teamId, int memberId)
        {
            AddMember(teamId, _objectService.Get<Player>(x => x.Id == memberId));
        }

        public void AddMember(int teamId, Player member)
        {
            var team = _objectService.Get<Team>(x => x.Id == teamId);
            team.Members.Add(member);
            _objectService.Save(team);
        }

        public void RemoveMember(int teamId, int memberId)
        {
            var team = _objectService.Get<Team>(x => x.Id == teamId);
            var memberToRemove = _objectService.Get<Player>(x => x.Id == memberId);
            if (Session.Current.IsAdmin || team.Captain.Id == Session.Current.Id)
            {
                if (team.ContestantIn.Any(x => x.IsRanked && x.Status == TournamentStatus.Active))
                {
                    throw new Exception("Can't remove player from the team - its registered for active tournament");
                }

                team.Members.Remove(memberToRemove);
                _objectService.Save(team);
            }
            else
            {
                throw new Exception("You don't have access to this team");
            }
        }
    }
}
