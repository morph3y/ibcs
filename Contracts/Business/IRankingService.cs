using System.Collections.Generic;
using System.Linq;
using Entities;

namespace Contracts.Business
{
    public interface IRankingService
    {
        IEnumerable<Subject> Rank(IEnumerable<Subject> subjects);
    }
}