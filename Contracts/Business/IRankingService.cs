using System.Collections.Generic;
using Entities;

namespace Contracts.Business
{
    public interface IRankingService
    {
        IEnumerable<Rank> Get<T>(int? limit = null) where T : Subject;
        IEnumerable<Subject> Rank(IEnumerable<Subject> subjects);
        
        void Save(Rank rank);

        void UpdateRank(Subject winner, Subject player2);
        Rank InitRank(Subject subject);
        IEnumerable<Rank> InitRank(IEnumerable<Subject> subjects);
    }
}