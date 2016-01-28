using System;
using System.Linq.Expressions;

using Contracts.Business;
using Contracts.Business.Dal;
using Entities;

namespace Business
{
    internal sealed class GameService : IGameService
    {
        private readonly ITournamentService _tournamentService;
        private readonly IGameDataAdapter _gameDataAdapter;
        private readonly IRankingService _rankingService;
        public GameService(ITournamentService tournamentService, IGameDataAdapter gameDataAdapter, IRankingService rankingService)
        {
            _tournamentService = tournamentService;
            _gameDataAdapter = gameDataAdapter;
            _rankingService = rankingService;
        }

        public Game Get(Expression<Func<Game, bool>> @where)
        {
            return _gameDataAdapter.Get(where);
        }

        public void EndGame(Game game)
        {
            game.Status = GameStatus.Finished;
            if (game.Winner == null)
            {
                throw new Exception("Can not end a game without a winner");
            }

            if ((game.Participant1.Id == game.Winner.Id && game.Participant1Score <= game.Participant2Score)
                || 
                (game.Participant2.Id == game.Winner.Id && game.Participant2Score <= game.Participant1Score))
            {
                throw new Exception("Winner cannot have less points. 1:0 maybe?");
            }

            _tournamentService.Update(game.TournamentStage.Tournament);

            _rankingService.UpdateRank(
                game.Participant1.Id == game.Winner.Id ? game.Participant1 : game.Participant2,
                game.Participant1.Id == game.Winner.Id ? game.Participant2 : game.Participant1);
        }
    }
}
