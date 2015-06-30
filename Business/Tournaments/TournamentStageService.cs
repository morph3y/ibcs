using Business.Tournaments.StageBuilders;
using Contracts.Business.Dal;
using Contracts.Business.Tournaments;
using Entities;

namespace Business.Tournaments
{
    internal sealed class TournamentStageService : ITournamentStageService
    {
        private readonly ITournamentDataAdapter _tournamentDataAdapter;
        private readonly IStageBuilderFactory _stageBuilderFactory;
        public TournamentStageService(ITournamentDataAdapter tournamentDataAdapter, IStageBuilderFactory stageBuilderFactory = null)
        {
            _tournamentDataAdapter = tournamentDataAdapter;
            _stageBuilderFactory = stageBuilderFactory ?? new StageBuilderFactory();
        }

        public void GenerateStages(Tournament tournament)
        {
            _stageBuilderFactory.Create(tournament).Build();
            _tournamentDataAdapter.Save(tournament);
        }

        public void RemoveContestant(Subject contestant, Tournament tournament)
        {
            if (tournament.Status == TournamentStatus.Closed
                // temp until we figure out BYE player 
                || tournament.Status == TournamentStatus.Active)
            {
                return;
            }

            tournament.Contestants.Remove(contestant);

            GenerateStages(tournament);
        }

        public void UpdateStages(Tournament tournament)
        {
            _stageBuilderFactory.Create(tournament).Update();
            _tournamentDataAdapter.Save(tournament);
        }
    }
}
