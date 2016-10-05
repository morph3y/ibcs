using System.Collections.Generic;
using System.Linq;
using Entities;

namespace Business.Ranking
{
    internal sealed class LeagueParentTournamentRankingProvider : IRankingProvider
    {
        private readonly Tournament _tournament;
        public LeagueParentTournamentRankingProvider(Tournament tournament)
        {
            _tournament = tournament;
        }

        public IEnumerable<Subject> Rank(IEnumerable<Subject> subjects)
        {
            var standings = GetStandings(_tournament)
                        .OrderByDescending(x => x.Wins)
                        .ThenByDescending(x => x.Ties)
                        .ThenBy(x => x.Losses).ToList();

            var toReturn = new List<Subject>();
            standings.ForEach(x=> { if(subjects.Contains(x.Subject)) { toReturn.Add(x.Subject); } });

            return toReturn;
        }

        private IList<Standing> GetStandings(Tournament tournament)
        {
            var standings = new Dictionary<int, Standing>();
            var stages = tournament.Parent.Stages.OrderBy(x => x.Order);
            foreach (var stage in stages)
            {
                var games = stage.Games.OrderBy(x => x.Order);
                foreach (var game in games)
                {
                    if (game.Winner == null)
                    {
                        continue;
                    }

                    if (standings.ContainsKey(game.Winner.Id))
                    {
                        standings[game.Winner.Id].Wins += 1;
                    }
                    else
                    {
                        standings.Add(game.Winner.Id, new Standing { Subject = game.Winner, Wins = 1, Losses = 0, Ties = 0 });
                    }

                    if (game.Participant1 == null || game.Participant2 == null)
                    {
                        continue;
                    }

                    var loser = game.Winner.Id == game.Participant1.Id ? game.Participant2 : game.Participant1;
                    if (standings.ContainsKey(loser.Id))
                    {
                        standings[loser.Id].Losses += 1;
                    }
                    else
                    {
                        standings.Add(loser.Id, new Standing { Subject = loser, Wins = 0, Losses = 1, Ties = 0 });
                    }
                }
            }
            return standings.Values.ToList();
        }

        private class Standing
        {
            public Subject Subject { get; set; }
            public int Wins { get; set; }
            public int Losses { get; set; }
            public int Ties { get; set; }
        }
    }
}
