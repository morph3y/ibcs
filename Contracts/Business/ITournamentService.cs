using System.Collections.Generic;
using Entities;

namespace Contracts.Business
{
    public interface ITournamentService
    {
        IEnumerable<Tournament> GetList();
        Tournament Get(int id);
        void Save(Tournament tournament);

        // Contestants menagement
        bool IsInTournament(int tournamentId, int memberId);
        void AddContestant(int contestantId, Tournament tournament);
        void RemoveContestant(int contestantId, Tournament tournament);
        void RemoveContestant(Subject contestant, Tournament tournament);

        // Stages management
        void Create(Tournament tournament);
        Tournament Convert(Tournament source, TournamentType targetType, int playerLimit);
        void Update(Tournament tournament);

        //void ResetRanks(Tournament tournament);
    }
}
