using System.Collections.Generic;
using System.Linq;
using Contracts.Business.Dal;
using Entities;

namespace Business.Ranking
{
    internal sealed class GlobalRankingProvider : IRankingProvider
    {
        private readonly IRankingDataAdapter _dataAdapter;
        public GlobalRankingProvider(IRankingDataAdapter dataAdapter)
        {
            _dataAdapter = dataAdapter;
        }

        public IEnumerable<Subject> Rank(IEnumerable<Subject> subjects)
        {
            var rankableSubjects = _dataAdapter.GetRanks(subjects).OrderByDescending(x=>x.Elo).Select(x=>x.Subject);
            return rankableSubjects.Union(subjects.Except(rankableSubjects));
        }
    }
}
