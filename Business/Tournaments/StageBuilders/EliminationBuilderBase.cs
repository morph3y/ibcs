using System;
using System.Collections.Generic;
using System.Linq;
using Entities;

namespace Business.Tournaments.StageBuilders
{
    internal abstract class EliminationBuilderBase : IStageBuilder
    {
        private readonly Tournament _tournament;
        protected EliminationBuilderBase(Tournament tournament)
        {
            _tournament = tournament;
        }

        protected Tournament Tournament
        {
            get { return _tournament; }
        }

        public abstract void Build();
        public abstract void Update();

        protected void UpdateStages(IEnumerable<TournamentStage> stages, int numberOfContestants)
        {
            var stagesList = stages as IList<TournamentStage>;
            foreach (var stage in stagesList.OrderBy(x => x.Order))
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
                    if ((GetNumberOfStagesInTournament(numberOfContestants) - 1) == stage.Order)
                    {
                        // Last game
                        if (games[i].Status == GameStatus.Finished)
                        {
                            return;
                        }
                    }

                    if (games[i].Status == GameStatus.Finished && games.Count > (i + 1) && games[i + 1].Status == GameStatus.Finished)
                    {
                        var nextStage = stagesList.FirstOrDefault(x => x.Order == stage.Order + 1);
                        // need new stage
                        if (nextStage == null)
                        {
                            var numberOfGamesInNewStage = ((int)Math.Pow(2, (GetNumberOfStagesInTournament(numberOfContestants) - 1) - (stage.Order + 1)));
                            nextStage = new TournamentStage
                            {
                                Order = stage.Order + 1,
                                Name = numberOfGamesInNewStage == 1 ? "Final" : "1/" + numberOfGamesInNewStage,
                                Tournament = Tournament
                            };

                            stagesList.Add(nextStage);
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

        protected List<Game> CreateStageGames(IList<Subject> contestants, TournamentStage stage, bool isTeamEvent)
        {
            var numberOfGames = GetNumberOfGamesToStart(contestants);
            var numberOfByes = (numberOfGames * 2) - contestants.Count;
            var internalContestants = contestants.ToList();

            for (int i = 0; i < numberOfByes; i++)
            {
                internalContestants.Add(isTeamEvent
                    ? (Subject)new Team
                    {
                        Name = "BYE",
                        Id = -1
                    }
                    : (Subject)new Player
                    {
                        Name = "BYE",
                        Id = -1
                    });
            }

            var finalListOfGames = new List<Game>();
            var currentStage = 1;
            while (currentStage <= GetNumberOfGamesToStart(internalContestants))
            {
                if (currentStage == 1)
                {
                    finalListOfGames.Add(new Game
                    {
                        Order = 0,
                        Participant1 = internalContestants[0],
                        Participant2 = internalContestants[1],
                        Status = GameStatus.Pending,
                        TournamentStage = stage
                    });
                }
                else
                {
                    var newList = new List<Game>();
                    var stageModifier = (currentStage * 2) + 1;
                    foreach (var game in finalListOfGames)
                    {
                        var contestant1 = game.Participant1;
                        var contestant2 = internalContestants[(stageModifier - (internalContestants.IndexOf(contestant1) + 1)) - 1];

                        newList.Add(CreateGameBetween(contestant1, contestant2, stage, newList.Count));

                        contestant2 = game.Participant2;
                        contestant1 = internalContestants[(stageModifier - (internalContestants.IndexOf(contestant2) + 1)) - 1];

                        newList.Add(CreateGameBetween(contestant1, contestant2, stage, newList.Count));
                    }
                    finalListOfGames = newList;
                }

                currentStage = currentStage * 2;
            }
            return finalListOfGames;
        }

        protected Game CreateGameBetween(Subject contestant1, Subject contestant2, TournamentStage stage, int order)
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
                TournamentStage = stage,
                Winner = bye1 ? contestant2 : bye2 ? contestant1 : null
            };
        }

        protected static int GetNumberOfGamesToStart(IList<Subject> contestants)
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

        protected int GetNumberOfStagesInTournament(int numberOfContestants)
        {
            var numberOfStages = (int)Math.Ceiling(Math.Log(numberOfContestants, 2));
            return numberOfStages == 0 ? 1 : numberOfStages;
        }

        private bool NextStageHasAppropriateGame(TournamentStage stage, int gameIndex)
        {
            var nextStage = Tournament.Stages.FirstOrDefault(x => x.Order == stage.Order + 1);
            return nextStage != null && nextStage.Games.Any(x => x.Order == (gameIndex / 2));
        }
    }
}
