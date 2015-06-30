using Entities;

namespace Contracts.Business.Tournaments
{
    public interface ITournamentStageService
    {
        void GenerateStages(Tournament tournament);
        void RemoveContestant(Subject contestant, Tournament tournament);

        void UpdateStages(Tournament tournament);
    }
}