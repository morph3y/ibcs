using System;
using System.Collections.Generic;
using System.Linq;
using Entities;

namespace Business.Tournaments.StageBuilders
{
    internal sealed class SingleEliminationStageBuilder : IStageBuilder
    {
        private readonly Tournament _tournament;
        public SingleEliminationStageBuilder(Tournament tournament)
        {
            _tournament = tournament;
        }

        public void Build()
        {
            if (_tournament.Contestants.Count < 2)
            {
                return;
            }

            var contestants = _tournament.Contestants.ToList();
            var numberOfGames = GetNumberOfGamesToStart(contestants);

            _tournament.Stages.Clear();
            _tournament.Stages.Add(new TournamentStage
            {
                Name = "1/" + numberOfGames,
                Order = 0,
                Tournament = _tournament
            });

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

            var finalListOfGames = new List<Game>();
            var currentStage = 1;
            while (currentStage <= numberOfGames)
            {
                if (currentStage == 1)
                {
                    finalListOfGames.Add(new Game
                    {
                        Order = 0,
                        Participant1 = contestants[0],
                        Participant2 = contestants[1],
                        Status = GameStatus.Pending,
                        TournamentStage = _tournament.Stages[0]
                    });
                }
                else
                {
                    var newList = new List<Game>();
                    var stageModifier = (currentStage * 2) + 1;
                    foreach (var game in finalListOfGames)
                    {
                        var contestant1 = game.Participant1;
                        var contestant2 = contestants[(stageModifier - (contestants.IndexOf(contestant1) + 1)) - 1];

                        newList.Add(CreateGameBetween(contestant1, contestant2, newList.Count));

                        contestant2 = game.Participant2;
                        contestant1 = contestants[(stageModifier - (contestants.IndexOf(contestant2) + 1)) - 1];

                        newList.Add(CreateGameBetween(contestant1, contestant2, newList.Count));
                    }
                    finalListOfGames = newList;
                }

                currentStage = currentStage * 2;
            }

            finalListOfGames.ForEach(x => _tournament.Stages[0].Games.Add(x));

            // Trigger update in case stage 2 is needed
            if (numberOfByes > 1)
            {
                Update();
                if (_tournament.Stages.Count > 1)
                {
                    _tournament.Stages[1].Games = _tournament.Stages[1].Games.OrderBy(x => x.Order).ToList();
                }
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
                    if ((GetNumberOfStagesInTournament() - 1) == stage.Order)
                    {
                        // Last game
                        if (games[i].Status == GameStatus.Finished)
                        {
                            _tournament.Status = TournamentStatus.Closed;
                            return;
                        }
                    }

                    if (games[i].Status == GameStatus.Finished && games.Count > (i + 1) && games[i + 1].Status == GameStatus.Finished)
                    {
                        var nextStage = _tournament.Stages.FirstOrDefault(x => x.Order == stage.Order + 1);
                        // need new stage
                        if (nextStage == null)
                        {
                            var numberOfGamesInNewStage = ((int)Math.Pow(2, (GetNumberOfStagesInTournament() - 1) - (stage.Order + 1)));
                            nextStage = new TournamentStage
                            {
                                Order = stage.Order + 1,
                                Name = numberOfGamesInNewStage == 1 ? "Final" : "1/" + numberOfGamesInNewStage,
                                Tournament = _tournament
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

        private Game CreateGameBetween(Subject contestant1, Subject contestant2, int order)
        {
            var bye1 = contestant1.Id == -1;
            var bye2 = contestant2.Id == -1;

            return new Game
            {
                Order = order,
                Participant1 = bye1 ? null : contestant1,
                Participant2 = bye2 ? null : contestant2,
                Participant1Score = bye2 ? 1 : 0,
                Participant2Score = bye1 ? 1 : 0,
                Status = bye1 || bye2 ? GameStatus.Finished : GameStatus.Pending,
                TournamentStage = _tournament.Stages[0],
                Winner = bye1 ? contestant2 : bye2 ? contestant1 : null
            };
        }

        private bool NextStageHasAppropriateGame(TournamentStage stage, int gameIndex)
        {
            var nextStage = _tournament.Stages.FirstOrDefault(x => x.Order == stage.Order + 1);
            return nextStage != null && nextStage.Games.Any(x => x.Order == (gameIndex / 2));
        }

        private static int GetNumberOfGamesToStart(List<Subject> contestants)
        {
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
            return numberOfGames;
        }

        private int GetNumberOfStagesInTournament()
        {
            var numberOfStages = (int)Math.Ceiling(Math.Log(_tournament.Contestants.Count, 2));
            return numberOfStages == 0 ? 1 : numberOfStages;
        }
    }
}