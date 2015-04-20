using Entities;

namespace Contracts.Business
{
    public interface ITournamentStageService
    {
        TournamentStage CreateStages(Tournament tournament);
        void GenerateGames(Tournament tournamen);
    }
}
