using System;

using Entities;

namespace Contracts.Business
{
    public interface ITournamentStageService
    {
        TournamentStage CreateFirstStage(Tournament tournament);
        void GenerateGames(TournamentStage stage);
    }
}
