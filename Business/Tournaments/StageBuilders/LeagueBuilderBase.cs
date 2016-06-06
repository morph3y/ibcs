using System.Collections.Generic;
using System.Linq;
using Entities;

namespace Business.Tournaments.StageBuilders
{
    internal abstract class LeagueBuilderBase : IStageBuilder
    {
        public abstract void Build();
        public abstract void Update();

        protected IEnumerable<Game> CreateGames(IEnumerable<Subject> contestants)
        {
            var games = new List<Game>();
            foreach (var contestant1 in contestants.ToList())
            {
                foreach (var contestant2 in contestants.ToList())
                {
                    if (contestant1.Equals(contestant2))
                    {
                        continue;
                    }

                    var game = new Game
                    {
                        Participant1 = contestant1,
                        Participant2 = contestant2,
                        Status = GameStatus.NotStarted,
                        TournamentStage = null,
                        Winner = null,
                        Order = 0
                    };

                    if (!games.Contains(game))
                    {
                        games.Add(game);
                    }
                }
            }
            return games;
        }
    }
}