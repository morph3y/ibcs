using System.Collections.Generic;
using Entities;

namespace Contracts.Business
{
    public interface IRankingService
    {
        IEnumerable<Subject> Rank(IEnumerable<Subject> subjects);
        void UpdateRank(Subject winner, Subject player2);
    }
}