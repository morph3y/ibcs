using System.Collections.Generic;
using Entities;

namespace Contracts.Business
{
    public interface ITournamentService
    {
        IEnumerable<Tournament> GetList();
        Tournament Get(int id);
        void Save(Tournament tournament);
        void Create(Tournament entity);
        bool IsInTournament(int tournamentId, int memberId);
        void AddContestant(Subject contestant, Tournament tournament);
        void RemoveContestant(Subject contestant, Tournament tournament);
    }
}
