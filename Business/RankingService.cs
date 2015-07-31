using System.Collections.Generic;
using System.Linq;
using Contracts.Business;
using Contracts.Business.Dal;
using Entities;

namespace Business
{
    internal sealed class RankingService : IRankingService
    {
        private readonly IRankingAdapter _rankingAdapter;
        public RankingService(IRankingAdapter rankingAdapter)
        {
            _rankingAdapter = rankingAdapter;
        }

        public IEnumerable<Subject> Rank(IEnumerable<Subject> subjects)
        {
            var rankInfo = _rankingAdapter.GetRanks(subjects);

            return rankInfo.OrderByDescending(x => x.Elo).Select(x => x.Subject);
        }
    }
}
