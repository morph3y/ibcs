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
    internal sealed class TournamentDataAdapter : ITournamentDataAdapter
    {
        private readonly IDataAccessAdapter _dataAccessAdapter;
        public TournamentDataAdapter(IDataAccessAdapter dataAccessAdapter)
        {
            _dataAccessAdapter = dataAccessAdapter;
        }

        public IEnumerable<Tournament> GetList()
        {
            return _dataAccessAdapter.GetCollection<Tournament>();
        }

        public IEnumerable<Tournament> GetCollection(Expression<Func<Tournament, bool>> where)
        {
            return _dataAccessAdapter.GetCollection(where);
        }

        public Tournament Get(Expression<Func<Tournament, bool>> where)
        {
            return _dataAccessAdapter.GetCollection(where).FirstOrDefault();
        }

        public void Save(Tournament tournament)
        {
            _dataAccessAdapter.Save(tournament);
        }

        public bool IsInTournament(int memberId, int tournamentId)
        {
            Player captain1 = null;
            Player member1 = null;
            Team teamAlias = null;
            Team teamAliasMembers = null;
            var teamsWhereMember = QueryOver.Of(() => teamAliasMembers)
                .JoinAlias(x => x.Members, () => member1)
                .JoinQueryOver<Tournament>(x=>x.ContestantIn)
                .Where(x=>x.Id == tournamentId)
                .Where(() => member1.Id == memberId)
                .Select(Projections.Distinct(Projections.Property(() => teamAliasMembers.Id)));

            var teamWhereCaptain = QueryOver.Of(() => teamAlias)
                .JoinAlias(x => x.Captain, () => captain1)
                .JoinQueryOver<Tournament>(x => x.ContestantIn)
                .Where(x => x.Id == tournamentId)
                .Where(() => captain1.Id == memberId)
                .Select(Projections.Distinct(Projections.Property(() => teamAlias.Id)));

            return _dataAccessAdapter.GetCollection(
                QueryOver.Of<Team>().Where(Restrictions.Disjunction()
                .Add(Subqueries.WhereProperty<Player>(x => x.Id).In(teamsWhereMember))
                .Add(Subqueries.WhereProperty<Player>(x => x.Id).In(teamWhereCaptain)))
            ).Any();
        }
    }
}
