using System.Collections.Generic;
using Entities;

namespace Contracts.Business
{
    public interface ITournamentService
    {
        IEnumerable<Tournament> GetList();
        Tournament Get(int id);
    }
}
