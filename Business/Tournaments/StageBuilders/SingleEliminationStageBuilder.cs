using System;
using System.Collections.Generic;
using System.Linq;
using Contracts.Business;
using Entities;

namespace Business.Tournaments.StageBuilders
{
    internal sealed class SingleEliminationStageBuilder : IStageBuilder
    {
        private readonly Tournament _tournament;
        private readonly IRankingService _rankingService;
        public SingleEliminationStageBuilder(Tournament tournament, IRankingService rankingService = null)
        {
            _tournament = tournament;
            _rankingService = rankingService ?? new RankingService();
        }

        public void Build()
        {
            if (_tournament.Contestants.Count < 2)
            {
                return;
            }

            var contestants = _rankingService.Rank(_tournament.Contestants).ToList();

            int numberOfGames;
            if ((contestants.Count & (contestants.Count - 1)) != 0)
            {
                var binLog = Math.Ceiling(Math.Log(contestants.Count, 2));
                var nextPowerOfTwo = (int)Math.Pow(2, binLog);
                numberOfGames = nextPowerOfTwo / 2;
            }
            else
            {
                numberOfGames = contestants.Count / 2;
            }

            _tournament.Stages = new List<TournamentStage>
            {
                new TournamentStage
                {
                    Name = "1/" + numberOfGames,
                    Order = 0,
                    Tournament = _tournament
                }
            };

            var numberOfByes = (numberOfGames * 2) - contestants.Count;
            for (int i = 0; i < numberOfByes; i++)
            {
                contestants.Add(_tournament.IsTeamEvent
                  ? (Subject)new Team
                {
                    Name = "BYE",
                    Id = -1
                } : (Subject)new Player
                {
                    Name = "BYE",
                    Id = -1
                });
            }

            var upper = contestants.Take(contestants.Count / 2).ToList();
            var lower = contestants.Skip(contestants.Count / 2).Reverse().ToList();
            for (int i = 0; i < numberOfGames; i++)
            {
                var even = i % 2 == 0;

                var contestant1 = upper[i];
                var contestant2 = lower[i];

                if (!even)
                {
                    contestant1 = upper[upper.Count - i];
                    contestant2 = lower[upper.Count - i];
                }

                var bye1 = contestant1.Id == -1;
                var bye2 = contestant2.Id == -1;

                _tournament.Stages[0].Games.Add(new Game
                {
                    Order = even ? i : numberOfGames - (i - 1),
                    Participant1 = bye1 ? null : contestant1,
                    Participant2 = bye2 ? null : contestant2,
                    Participant1Score = bye2 ? 1 : 0,
                    Participant2Score = bye1 ? 1 : 0,
                    Status = bye1 || bye2 ? GameStatus.Finished : GameStatus.Pending,
                    TournamentStage = _tournament.Stages[0],
                    Winner = bye1 ? contestant2 : bye2 ? contestant1 : null
                });
            }

            // Trigger update in case stage 2 is needed
            if (numberOfByes > 1)
            {
                Update();
                _tournament.Stages[1].Games = _tournament.Stages[1].Games.OrderBy(x => x.Order).ToList();
            }

            _tournament.Stages[0].Games = _tournament.Stages[0].Games.OrderBy(x => x.Order).ToList();
        }

        public void Update()
        {
            foreach (var stage in _tournament.Stages.OrderBy(x => x.Order))
            {
                var games = stage.Games.OrderBy(x => x.Order).ToList();
                for (var i = 0; i < games.Count; i++)
                {
                    // Process only half of all games (pairs)
                    if (i % 2 != 0)
                    {
                        continue;
                    }

                    // Last stage
                    if ((GetNumberOfStages() - 1) == stage.Order)
                    {
                        // Last game
                        if (games[i].Status == GameStatus.Finished)
                        {
                            _tournament.Status = TournamentStatus.Closed;
                            return;
                        }
                    }

                    if (games[i].Status == GameStatus.Finished && games[i + 1].Status == GameStatus.Finished)
                    {
                        var nextStage = _tournament.Stages.FirstOrDefault(x => x.Order == stage.Order + 1);
                        if (nextStage == null)
                        {
                            nextStage = new TournamentStage
                            {
                                Order = stage.Order + 1,
                                Name = "",
                                Tournament = _tournament,
                                Games = new List<Game>()
                            };

                            _tournament.Stages.Add(nextStage);
                        }
                        else if (NextStageHasAppropriateGame(stage, i))
                        {
                            continue;
                        }

                        nextStage.Games.Add(new Game
                        {
                            Order = i / 2,
                            Participant1 = games[i].Winner,
                            Participant2 = games[i + 1].Winner,
                            Status = GameStatus.Pending,
                            TournamentStage = nextStage
                        });
                    }
                }
            }
        }

        private bool NextStageHasAppropriateGame(TournamentStage stage, int gameIndex)
        {
            var nextStage = _tournament.Stages.FirstOrDefault(x => x.Order == stage.Order + 1);
            return nextStage != null && nextStage.Games.Any(x => x.Order == (gameIndex / 2));
        }

        private int GetNumberOfStages()
        {
            return (int)Math.Ceiling(Math.Log((_tournament.Stages[0].Games.Count * 2), 2));
        }
    }
}