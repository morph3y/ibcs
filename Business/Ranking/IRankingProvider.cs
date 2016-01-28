using System.Collections.Generic;
using Entities;

namespace Business.Ranking
{
    public interface IRankingProvider
    {
        IEnumerable<Subject> Rank(IEnumerable<Subject> subjects);
    }
}