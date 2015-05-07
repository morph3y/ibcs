﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Contracts.Business;
using Contracts.Business.Dal;
using Contracts.Session;
using Entities;

namespace Business
{
    internal sealed class TeamService : ITeamService
    {
        private readonly ITeamDataAdapter _teamDataAdapter;
        private readonly IPlayerDataAdapter _playerDataAdapter;
        public TeamService(ITeamDataAdapter teamDataAdapter, IPlayerDataAdapter playerDataAdapter)
        {
            _teamDataAdapter = teamDataAdapter;
            _playerDataAdapter = playerDataAdapter;
        }

        public IEnumerable<Team> GetCollection(Expression<Func<Team, bool>> where)
        {
            return _teamDataAdapter.GetCollection(where);
        } 

        public void Save(Team entity)
        {
            _teamDataAdapter.Save(entity);
        }

        public Team Get(Expression<Func<Team, bool>> where)
        {
            return _teamDataAdapter.GetCollection(where).FirstOrDefault();
        }

        public IEnumerable<Team> GetList()
        {
            return _teamDataAdapter.GetCollection();
        }

        public IEnumerable<Player> GetAvailableMembers(int teamId)
        {
            return _teamDataAdapter.GetAvailableMembers(teamId);
        }

        public void AddMember(int teamId, int memberId)
        {
            AddMember(teamId, _playerDataAdapter.Get(x => x.Id == memberId));
        }

        public void AddMember(int teamId, Player member)
        {
            var team = _teamDataAdapter.Get(x => x.Id == teamId);
            team.Members.Add(member);
            _teamDataAdapter.Save(team);
        }

        public void RemoveMember(int teamId, int memberId)
        {
            var team = _teamDataAdapter.Get(x => x.Id == teamId);
            var memberToRemove = _playerDataAdapter.Get(x => x.Id == memberId);
            if (Session.Current.IsAdmin || team.Captain.Id == Session.Current.Id)
            {
                if (team.ContestantIn.Any(x => x.IsRanked && x.Status == TournamentStatus.Active))
                {
                    throw new Exception("Can't remove player from the team - its registered for active tournament");
                }

                team.Members.Remove(memberToRemove);
                _teamDataAdapter.Save(team);
            }
            else
            {
                throw new Exception("You don't have access to this team");
            }
        }
    }
}