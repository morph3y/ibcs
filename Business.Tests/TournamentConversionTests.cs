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
    }
}
