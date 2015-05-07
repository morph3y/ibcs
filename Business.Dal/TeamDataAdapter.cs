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

        public IEnumerable<Player> GetAvailableMembers(int teamId)
        {
            Player captainAlias = null;
            // TODO: No support for unions
            var members = QueryOver.Of<Player>()
                .JoinQueryOver<Team>(x => x.Teams)
                .Where(x => x.Id == teamId);
            var captain = QueryOver.Of<Team>()
                .JoinAlias(x => x.Captain, () => captainAlias)
                .Where(x => x.Id == teamId)
                .Select(Projections.Distinct(Projections.Property(() => captainAlias.Id)));

            return _dataAccessAdapter.GetCollection(QueryOver.Of<Player>().Where(Restrictions.Conjunction()
                .Add(Subqueries.WhereProperty<Player>(x => x.Id).NotIn(members.Select(x => x.Id)))
                .Add(Subqueries.WhereProperty<Player>(x => x.Id).NotIn(captain))));
        } 
    }
}
