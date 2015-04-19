using System;
using System.Collections.Generic;
using System.Linq;

using Contracts.Business;

using Entities;

namespace Business
{
    internal sealed class TournamentStageService : ITournamentStageService
    {
        public TournamentStage CreateFirstStage(Tournament tournament)
        {
            // for now just the first stage
            if (tournament.TournamentType == TournamentType.League)
            {
                // for now just the league
                var stage = new TournamentStage
                {
                    Tournament = tournament, 
                    Name = "Round robin", 
                    Order = 1
                };

                GenerateGames(stage);
                return stage;
            }
            return null;
        }

        public void GenerateGames(TournamentStage stage)
        {
            if (stage.Tournament.Status != TournamentStatus.Registration)
            {
                return;
            }

            stage.Games.Clear();
            var pairs = from first in stage.Tournament.Contestants
                      from second in stage.Tournament.Contestants
                      select Tuple.Create(first, second);

            foreach (var pair in pairs)
            {
                // no game with self
                if (pair.Item1 == pair.Item2)
                {
                    continue;
                }

                stage.Games.Add(new Game
                {
                    Participant1 = pair.Item1,
                    Participant2 = pair.Item2,
                    Status = GameStatus.NotStarted,
                    TournamentStage = stage
                });
            }
        }
    }
}
