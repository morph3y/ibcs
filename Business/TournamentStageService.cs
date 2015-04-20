using System;
using System.Linq;

using Contracts.Business;

using Entities;

namespace Business
{
    internal sealed class TournamentStageService : ITournamentStageService
    {
        public TournamentStage CreateStages(Tournament tournament)
        {
            if (tournament.TournamentType == TournamentType.League)
            {
                // for now just the league
                var stage = new TournamentStage
                {
                    Tournament = tournament,
                    Name = "Round robin",
                    Order = 1
                };
                GenerateGames(tournament);
                return stage;
            }
            return null;
        }

        public void GenerateGames(Tournament tournamen)
        {
            if (tournamen.Status != TournamentStatus.Registration)
            {
                return;
            }

            var pairs = from first in tournamen.Contestants
                      from second in tournamen.Contestants
                      select Tuple.Create(first, second);

            
        }
    }
}
