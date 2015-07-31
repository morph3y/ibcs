using System.Collections.Generic;
using Entities;

namespace Contracts.Business.Dal
{
    public interface IRankingAdapter
    {
        IEnumerable<RankModel> GetRankHistory(Subject subject);
        IEnumerable<RankModel> GetRanks(IEnumerable<Subject> subjects);

        RankModel GetRank(Subject subject);
    }
}
