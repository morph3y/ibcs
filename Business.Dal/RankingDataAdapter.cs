using System;
using System.Collections.Generic;
using Contracts.Business;
using Contracts.Business.Dal;
using Entities;

namespace Business.Dal
{
    internal sealed class RankingDataAdapter : IRankingAdapter
    {
        public IEnumerable<RankModel> GetRankHistory(Subject subject)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RankModel> GetRanks(IEnumerable<Subject> subjects)
        {
            throw new NotImplementedException();
        }

        public RankModel GetRank(Subject subject)
        {
            throw new NotImplementedException();
        }
    }
}
