using System.Collections.Generic;
using System.Linq;
using Business.Ranking;
using Contracts.Business.Dal;
using Entities;
using Moq;
using NUnit.Framework;

namespace Business.Tests.Ranking
{
    [TestFixture]
    internal sealed class RankingProvidersTests
    {
        private IRankingProvider _testSubject;
        private readonly Mock<IRankingDataAdapter> _fakeRankingDataAdapter;

        public RankingProvidersTests()
        {
            _fakeRankingDataAdapter = new Mock<IRankingDataAdapter>();
        }

        [Test]
        public void VerifyCanRankNewTournament()
        {
            // Arrange
            var listOfSubjects = new List<Subject> { new Team { Name = "Team1" }, new Team { Name = "Team2" } };
            _testSubject = new GlobalRankingProvider(_fakeRankingDataAdapter.Object);

            _fakeRankingDataAdapter.Setup(x => x.GetRanks(listOfSubjects)).Returns(new List<Rank>
            {
                new Rank { Subject = listOfSubjects[0], Elo = 1},
                new Rank { Subject = listOfSubjects[1], Elo = 2}
            });

            // Act
            var result = _testSubject.Rank(listOfSubjects);

            // Assert
            Assert.AreEqual("Team2", result.First().Name);
        }

        [Test]
        public void VerifyCanCorrectlyRankChildTournament()
        {
            // Arrange
            var contestants = new List<Subject>
            {
                new Player {Name = "Player1", Id = 0},
                new Player {Name = "Player2", Id = 1},
                new Player {Name = "Player3", Id = 2},
                new Player {Name = "Player4", Id = 3},
                new Player {Name = "Player5", Id = 4},
                new Player {Name = "Player6", Id = 5},
                new Player {Name = "Player7", Id = 6},
                new Player {Name = "Player8", Id = 7}
            };
            var tournament = new Tournament
            {
                Contestants = contestants,
                TournamentType = TournamentType.League,
                Stages = new List<TournamentStage>
                {
                    new TournamentStage
                    {
                        Games = new List<Game>
                        {
                            CreateGame(contestants.First(), contestants[7]),
                            CreateGame(contestants[1], contestants[6]),
                            CreateGame(contestants[2], contestants[5]),
                            CreateGame(contestants[3], contestants[4])
                        }
                    },
                    new TournamentStage
                    {
                        Games = new List<Game>
                        {
                            CreateGame(contestants.First(), contestants[6]),
                            CreateGame(contestants[1], contestants[5]),
                            CreateGame(contestants[2], contestants[4]),
                            CreateGame(contestants[3], contestants[7])
                        }
                    },
                    new TournamentStage
                    {
                        Games = new List<Game>
                        {
                            CreateGame(contestants.First(), contestants[5]),
                            CreateGame(contestants[1], contestants[4]),
                            CreateGame(contestants[2], contestants[7]),
                            CreateGame(contestants[3], contestants[6])
                        }
                    },
                    new TournamentStage
                    {
                        Games = new List<Game>
                        {
                            CreateGame(contestants.First(), contestants[4]),
                            CreateGame(contestants[1], contestants[7]),
                            CreateGame(contestants[2], contestants[6]),
                            CreateGame(contestants[3], contestants[5])
                        }
                    },
                    new TournamentStage
                    {
                        Games = new List<Game>
                        {
                            CreateGame(contestants.First(), contestants[3]),
                            CreateGame(contestants[6], contestants[4]),
                            CreateGame(contestants[2], contestants[1]),
                            CreateGame(contestants[5], contestants[7])
                        }
                    },new TournamentStage
                    {
                        Games = new List<Game>
                        {
                            CreateGame(contestants.First(), contestants[2]),
                            CreateGame(contestants[6], contestants[7]),
                            CreateGame(contestants[1], contestants[3]),
                            CreateGame(contestants[4], contestants[5])
                        }
                    },new TournamentStage
                    {
                        Games = new List<Game>
                        {
                            CreateGame(contestants.First(), contestants[1]),
                            CreateGame(contestants[1], contestants[7]),
                            CreateGame(contestants[2], contestants[6]),
                            CreateGame(contestants[3], contestants[5])
                        }
                    }
                }
            };
            var tournamentToRank = new Tournament
            {
                Contestants = contestants,
                Parent = tournament,
                TournamentType = TournamentType.SingleElimination
            };

            _testSubject = new ParentTournamentRankingProvider(tournamentToRank);

            // Act
            var rankedSubjects = _testSubject.Rank(contestants).ToList();

            // Assert
            Assert.AreEqual(0, rankedSubjects[0].Id);
            Assert.AreEqual(2, rankedSubjects[1].Id);
            Assert.AreEqual(1, rankedSubjects[2].Id);
            Assert.AreEqual(3, rankedSubjects[3].Id);
            Assert.AreEqual(6, rankedSubjects[4].Id);
            Assert.AreEqual(4, rankedSubjects[5].Id);
            Assert.AreEqual(5, rankedSubjects[6].Id);
            Assert.AreEqual(7, rankedSubjects[7].Id);
        }

        private static Game CreateGame(Subject winner, Subject loser)
        {
            return new Game
            {
                Participant1 = winner,
                Participant2 = loser,
                Participant1Score = 1,
                Participant2Score = 0,
                Status = GameStatus.Finished,
                Winner = winner
            };
        }
    }
}
