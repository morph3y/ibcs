﻿using System;
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
        private readonly IRankingService _rankingService;
        public GameService(ITournamentStageService tournamentStageService, IGameDataAdapter gameDataAdapter, IRankingService rankingService)
        {
            _tournamentStageService = tournamentStageService;
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

            _rankingService.UpdateRank(
                game.Participant1.Id == game.Winner.Id ? game.Participant1 : game.Participant2,
                game.Participant1.Id == game.Winner.Id ? game.Participant2 : game.Participant1);

            _tournamentStageService.UpdateStages(game.TournamentStage.Tournament);
        }
    }
}
