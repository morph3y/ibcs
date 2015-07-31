using System.Collections.Generic;
using Entities;

namespace Contracts.Business
{
    public interface IRankingService
    {
        IEnumerable<Subject> Rank(IEnumerable<Subject> subjects);
    }
}