using System.Collections.Generic;
using Contracts.Business;
using Entities;

namespace Business
{
    internal sealed class RankingService : IRankingService
    {
        public IEnumerable<Subject> Rank(IEnumerable<Subject> subjects)
        {
            return subjects;
        }
    }
}
