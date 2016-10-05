using System;
using System.Collections.Generic;

using Business.Ranking;
using Business.Tournaments;
using Business.Tournaments.StageBuilders;

using Contracts.Business;
using Contracts.Business.Dal;

using Entities;

using Moq;

using NUnit.Framework;

namespace Business.Tests
{
    [TestFixture]
    internal sealed class TournamentConversionTests
    {
        private TournamentService _testSubject;
        private Mock<ITournamentDataAdapter> _fakeTournamentDataAdapter;
        private Mock<ISubjectService> _fakeSubjectService;
        private Mock<IRankingDataAdapter> _fakeRankingDataAdapter;
        private Mock<IStageBuilderFactory> _fakeStageBuilderFactory;
        private Mock<IRankingProviderFactory> _fakeRankingProviderFactory;
        private Mock<IRankingProvider> _fakeRankingProvider;
        private Mock<IStageBuilder> _fakeStageBuilder;

        [SetUp]
        public void BeforeEveryTest()
        {
            _fakeTournamentDataAdapter = new Mock<ITournamentDataAdapter>();
            _fakeSubjectService = new Mock<ISubjectService>();
            _fakeRankingDataAdapter = new Mock<IRankingDataAdapter>();
            _fakeStageBuilderFactory = new Mock<IStageBuilderFactory>();
            _fakeRankingProviderFactory = new Mock<IRankingProviderFactory>();
            _fakeRankingProvider = new Mock<IRankingProvider>();
            _fakeStageBuilder = new Mock<IStageBuilder>();

            _fakeRankingProviderFactory.Setup(x => x.GetProvider(It.IsAny<Tournament>())).Returns(_fakeRankingProvider.Object);
            _fakeRankingProvider.Setup(x => x.Rank(It.IsAny<List<Subject>>())).Returns((List<Subject> subjects) => subjects);
            _fakeStageBuilderFactory.Setup(x => x.Create(It.IsAny<Tournament>())).Returns(_fakeStageBuilder.Object);

            _testSubject = new TournamentService(
                _fakeTournamentDataAdapter.Object,
                _fakeSubjectService.Object,
                _fakeRankingDataAdapter.Object,
                _fakeStageBuilderFactory.Object,
                _fakeRankingProviderFactory.Object);
        }

        [Test]
        public void VerifyCanConvertTournamentWithPlayerLimit()
        {
            // Arrange
            var contestants = new List<Subject> { new Player { Id = 1, Name = "bla" }, new Player { Id = 2, Name = "bla2" } };
            var tournament = new Tournament
            {
                Contestants = contestants,
                Status = TournamentStatus.Closed,
                TournamentType = TournamentType.League,
                IsRanked = true,
                IsTeamEvent = false,
                PointsForTie = 123,
                PointsForWin = 2,
                Stages = new List<TournamentStage> { new TournamentStage { Name = "blah" } }
            };

            // Act
            var resultingTournament = _testSubject.Convert(tournament, TournamentType.SingleElimination, 1);

            // Assert
            Assert.AreEqual(1, resultingTournament.Contestants.Count);
            Assert.IsNotNull(resultingTournament.Parent);
            Assert.AreEqual(tournament, resultingTournament.Parent);
            Assert.AreEqual(TournamentType.SingleElimination, resultingTournament.TournamentType);
            Assert.AreEqual(TournamentStatus.Registration, resultingTournament.Status);
            Assert.AreEqual(" (converted)", resultingTournament.Name);
            Assert.IsTrue(resultingTournament.IsRanked);
            Assert.IsFalse(resultingTournament.IsTeamEvent);
            Assert.AreEqual(123, resultingTournament.PointsForTie);
            Assert.AreEqual(2, resultingTournament.PointsForWin);
            Assert.AreEqual(0, resultingTournament.Stages.Count);
        }

        [Test]
        public void VerifyCannotConvertActiveTournament()
        {
            // Arrange
            var tournament = new Tournament
            {
                Status = TournamentStatus.Active,
                TournamentType = TournamentType.League
            };

            // Act / Assert
            Assert.Throws<Exception>(() => _testSubject.Convert(tournament, TournamentType.SingleElimination, 1));
        }

        [Test]
        public void VerifyCannotConvertFromSingleElimination()
        {
            // Arrange
            var tournament = new Tournament
            {
                Status = TournamentStatus.Closed,
                TournamentType = TournamentType.SingleElimination
            };

            // Act / Assert
            Assert.Throws<NotSupportedException>(() => _testSubject.Convert(tournament, TournamentType.SingleElimination, 1));
        }

        [Test]
        public void VerifyCanConvertFromGroupsToSingleElimination()
        {
            // Arrange

            var contestants = new List<Subject>
            {
                new Player {Name = "Player1"},
                new Player {Name = "Player2"},
                new Player {Name = "Player3"},
                new Player {Name = "Player4"},
                new Player {Name = "Player5"},
                new Player {Name = "Player6"},
                new Player {Name = "Player7"},
                new Player {Name = "Player8"},
                new Player {Name = "Player9"},
                new Player {Name = "Player10"},
                new Player {Name = "Player11"},
                new Player {Name = "Player12"},
                new Player {Name = "Player13"},
                new Player {Name = "Player14"},
                new Player {Name = "Player15"},
                new Player {Name = "Player16"},
                new Player {Name = "Player17"},
                new Player {Name = "Player18"},
                new Player {Name = "Player19"}
            };
            var groups = new List<TournamentGroup>
            {
                new TournamentGroup
                {
                    Contestants = new List<Subject>
                    {
                        contestants[0],
                        contestants[1],
                        contestants[2],
                        contestants[3],
                        contestants[4]
                    },
                    QualifiedContestants = new List<TournamentGroupQualifiedContestant>
                    {
                        new TournamentGroupQualifiedContestant { Contestant = contestants[0], Order = 1 },
                        new TournamentGroupQualifiedContestant { Contestant = contestants[4], Order = 0 }
                    }
                },
                new TournamentGroup
                {
                    Contestants = new List<Subject>
                    {
                        contestants[5],
                        contestants[6],
                        contestants[7],
                        contestants[8],
                        contestants[9]
                    },
                    QualifiedContestants = new List<TournamentGroupQualifiedContestant>
                    {
                        new TournamentGroupQualifiedContestant { Contestant = contestants[6], Order = 0 },
                        new TournamentGroupQualifiedContestant { Contestant = contestants[8], Order = 1 }
                    }
                },
                new TournamentGroup
                {
                    Contestants = new List<Subject> 
                    {
                        contestants[10],
                        contestants[11],
                        contestants[12],
                        contestants[13],
                        contestants[14]
                    },
                    QualifiedContestants = new List<TournamentGroupQualifiedContestant>
                    {
                        new TournamentGroupQualifiedContestant { Contestant = contestants[10], Order = 1},
                        new TournamentGroupQualifiedContestant { Contestant = contestants[12], Order = 2}
                    }
                },
                new TournamentGroup
                {
                    Contestants = new List<Subject>
                    {
                        contestants[15], 
                        contestants[16], 
                        contestants[17], 
                        contestants[18]
                    },
                    QualifiedContestants = new List<TournamentGroupQualifiedContestant>
                    {
                        new TournamentGroupQualifiedContestant { Contestant = contestants[17], Order = 0},
                        new TournamentGroupQualifiedContestant { Contestant = contestants[18], Order = 1}
                    }
                }
            };

            var tournament = new Tournament
            {
                Status = TournamentStatus.Closed,
                TournamentType = TournamentType.Group,
                Stages = new List<TournamentStage>
                {
                    new TournamentStage
                    {
                        Groups = groups
                    }
                },
                Contestants = contestants
            };

            _fakeRankingProviderFactory.Setup(x => x.GetProvider(It.IsAny<Tournament>())).Returns(new GroupParentTournamentRankingProvider(new Tournament { Parent = tournament }));
            _fakeStageBuilderFactory.Setup(x => x.Create(It.IsAny<Tournament>())).Returns<Tournament>(trn => new GroupToSingleElimintaionStageBuilder(trn));

            var result = _testSubject.Convert(tournament, TournamentType.SingleElimination, 8);

            Assert.AreEqual(8, result.Contestants.Count);
            Assert.AreEqual(TournamentType.SingleElimination, result.TournamentType);
            Assert.AreEqual(1, result.Stages.Count);
            Assert.AreEqual(4, result.Stages[0].Games.Count);
            Assert.AreEqual("Player5", result.Stages[0].Games[0].Participant1.Name);
            Assert.AreEqual("Player9", result.Stages[0].Games[0].Participant2.Name);
            Assert.AreEqual("Player13", result.Stages[0].Games[3].Participant1.Name);
            Assert.AreEqual("Player18", result.Stages[0].Games[3].Participant2.Name);
        }
    }
}
