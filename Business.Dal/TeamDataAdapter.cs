using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Contracts.Business.Dal;

using Dal.Contracts;

using Entities;

using NHibernate.Criterion;

namespace Business.Dal
{
    internal sealed class TeamDataAdapter : ITeamDataAdapter
    {
        private readonly IDataAccessAdapter _dataAccessAdapter;
        public TeamDataAdapter(IDataAccessAdapter dataAccessAdapter)
        {
            _dataAccessAdapter = dataAccessAdapter;
        }

        public Team Get(Expression<Func<Team, bool>> where)
        {
            return _dataAccessAdapter.GetCollection(where).FirstOrDefault();
        }

        public IEnumerable<Team> GetCollection(Expression<Func<Team, bool>> @where)
        {
            return _dataAccessAdapter.GetCollection(where);
        }

        public void Save(Team entity)
        {
            _dataAccessAdapter.Save(entity);
        }

        public IEnumerable<Team> GetCollection()
        {
            return _dataAccessAdapter.GetCollection<Team>();
        }

        public IEnumerable<TeamMemberRequest> GetTeamRequests(int memberId)
        {
            return _dataAccessAdapter.GetCollection<TeamMemberRequest>(x => x.Member.Id == memberId);
        }

        public IEnumerable<TeamMemberRequest> GetMembersRequests(int teamId)
        {
            return _dataAccessAdapter.GetCollection<TeamMemberRequest>(x => x.Team.Id == teamId);
        }

        public TeamMemberRequest GetRequest(int teamId, int memberId)
        {
            return _dataAccessAdapter.GetCollection<TeamMemberRequest>(x => x.Member.Id == memberId && x.Team.Id == teamId).FirstOrDefault();
        }

        public void RemoveRequest(TeamMemberRequest request)
        {
            _dataAccessAdapter.Delete(request);
        }

        public void CreateRequest(TeamMemberRequest request)
        {
            _dataAccessAdapter.Save(request);
        }

        public IEnumerable<Player> GetAvailableMembers(int teamId)
        {
            Player captainAlias = null;
            Player requestMember = null;
            Team requestTeam = null;
            // TODO: No support for unions
            var members = QueryOver.Of<Player>()
                .JoinQueryOver<Team>(x => x.Teams)
                .Where(x => x.Id == teamId);
            var captain = QueryOver.Of<Team>()
                .JoinAlias(x => x.Captain, () => captainAlias)
                .Where(x => x.Id == teamId)
                .Select(Projections.Distinct(Projections.Property(() => captainAlias.Id)));
            var requests = QueryOver.Of<TeamMemberRequest>()
                .JoinAlias(x=>x.Member, () => requestMember)
                .JoinAlias(x=>x.Team, () => requestTeam)
                .Where(x => requestTeam.Id == teamId)
                .Select(Projections.Distinct(Projections.Property(() => requestMember.Id)));

            return _dataAccessAdapter.GetCollection(QueryOver.Of<Player>().Where(Restrictions.Conjunction()
                .Add(Subqueries.WhereProperty<Player>(x => x.Id).NotIn(members.Select(x => x.Id)))
                .Add(Subqueries.WhereProperty<Player>(x => x.Id).NotIn(captain))
                .Add(Subqueries.WhereProperty<Player>(x => x.Id).NotIn(requests))));
        } 
    }
}
