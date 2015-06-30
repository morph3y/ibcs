using System;
using System.Linq.Expressions;

using Contracts.Business;
using Contracts.Business.Dal;
using Contracts.Business.Tournaments;
using Entities;

namespace Business
{
    internal sealed class GameService : IGameService
    {
        private readonly ITournamentStageService _tournamentStageService;
        private readonly IGameDataAdapter _gameDataAdapter;
        public GameService(ITournamentStageService tournamentStageService, IGameDataAdapter gameDataAdapter)
        {
            _tournamentStageService = tournamentStageService;
            _gameDataAdapter = gameDataAdapter;
        }

        public Game Get(Expression<Func<Game, bool>> @where)
        {
            return _gameDataAdapter.Get(where);
        }

        public void EndGame(Game game)
        {
            game.Status = GameStatus.Finished;
            _tournamentStageService.UpdateStages(game.TournamentStage.Tournament);
        }
    }
}
