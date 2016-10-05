using System;
using System.Collections.Generic;
using System.Linq;

using Entities;

namespace Business.Ranking
{
    internal sealed class GroupParentTournamentRankingProvider : IRankingProvider
    {
        private readonly Tournament _tournament;
        public GroupParentTournamentRankingProvider(Tournament tournament)
        {
            _tournament = tournament;
        }

        public IEnumerable<Subject> Rank(IEnumerable<Subject> subjects)
        {
            return _tournament.Parent.Stages[0].Groups.SelectMany(x=>x.QualifiedContestants).Select(x=>x.Contestant);
        }
    }
}