using System.Collections.Generic;
using Entities;

namespace Contracts.Business.Tournaments
{
    public interface ITournamentService
    {
        IEnumerable<Tournament> GetList();
        Tournament Get(int id);
        void Save(Tournament tournament);
        void Create(Tournament tournament);
        bool IsInTournament(int tournamentId, int memberId);
        void AddContestant(int contestantId, Tournament tournament);
        void RemoveContestant(int contestantId, Tournament tournament);
    }
}
