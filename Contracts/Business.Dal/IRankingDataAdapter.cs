using System.Collections.Generic;
using Entities;

namespace Contracts.Business.Dal
{
    public interface IRankingDataAdapter
    {
        IEnumerable<Rank> GetRanks(IEnumerable<Subject> subjects);
        Rank GetRank(Subject subject);

        void Save(Rank rank);

        Rank InitRank(Subject subject);
        IEnumerable<Rank> InitRank(IEnumerable<Subject> subjects);
    }
}
