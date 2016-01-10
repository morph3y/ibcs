using System.Collections.Generic;
using Entities;

namespace Contracts.Business.Dal
{
    public interface IRankingDataAdapter
    {
        IEnumerable<Rank> GetRanks(IEnumerable<Subject> subjects);
        Rank GetRank(Subject subject);
        IEnumerable<Rank> GetRanks<T>(int? limit = null) where T : Subject;

        IEnumerable<Rank> GetRanksToPunish(int allowedMonths, int punishLimit);

        void Save(Rank rank);
    }
}
