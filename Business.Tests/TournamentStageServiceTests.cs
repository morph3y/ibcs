using System.Collections.Generic;
using Business.Tournaments;
using Business.Tournaments.StageBuilders;
using Contracts.Business.Dal;
using Contracts.Session;
using Entities;
using Moq;
using NUnit.Framework;

namespace Business.Tests
{
    [TestFixture]
    internal sealed class TournamentStageServiceTests : BusinessTestBase
    {
        private TournamentStageService _testSubject;
        private Mock<ITournamentDataAdapter> _fakeTournamentDataAdapter;
        private Mock<IStageBuilderFactory> _fakeStageBuilderFactory;
        private Mock<IStageBuilder> _fakeStageBuilder;

        [SetUp]
        public void Setup()
        {
            _fakeTournamentDataAdapter = new Mock<ITournamentDataAdapter>();
            _fakeStageBuilderFactory = new Mock<IStageBuilderFactory>();
            _fakeStageBuilder = new Mock<IStageBuilder>();

            _fakeStageBuilderFactory.Setup(x => x.Create(It.IsAny<Tournament>())).Returns(_fakeStageBuilder.Object);

            _testSubject = new TournamentStageService(_fakeTournamentDataAdapter.Object, _fakeStageBuilderFactory.Object);
        }

        [TearDown]
        public void Teardown()
        {
            Session.Current = InitialPrincipal;
        }

        [Test]
        public void CanGenerateGames()
        {
            // Arrange / Act
            _testSubject.GenerateStages(new Tournament());

            // Assert
            _fakeStageBuilderFactory.Verify(x => x.Create(It.IsAny<Tournament>()), Times.Once);
            _fakeStageBuilder.Verify(x=>x.Build(), Times.Once);
        }

        [Test]
        public void CanUpdateStages()
        {
            // Arrange / Act
            _testSubject.UpdateStages(new Tournament());

            // Assert
            _fakeStageBuilderFactory.Verify(x=>x.Create(It.IsAny<Tournament>()), Times.Once);
            _fakeStageBuilder.Verify(x=>x.Update(), Times.Once);
        }

        [Test]
        public void CanRemoveContestant()
        {
            // Arrange
            var subjectToRemove = new Player();
            var tournament = new Tournament
            {
                Contestants = new List<Subject>
                {
                    subjectToRemove
                },
                TournamentType = TournamentType.League,
                Status = TournamentStatus.Registration
            };

            // Act
            _testSubject.RemoveContestant(subjectToRemove, tournament);

            // Assert
            Assert.AreEqual(0, tournament.Contestants.Count);
            _fakeTournamentDataAdapter.Verify(x=>x.Save(tournament), Times.Once);
        }

        [Test]
        public void CanNotRemoveContestantIfTournamentStarted()
        {
            // Arrange
            var subjectToRemove = new Player();
            var tournament = new Tournament
            {
                Contestants = new List<Subject>
                {
                    subjectToRemove
                },
                TournamentType = TournamentType.League,
                Status = TournamentStatus.Active
            };

            // Act
            _testSubject.RemoveContestant(subjectToRemove, tournament);

            // Assert
            Assert.AreEqual(1, tournament.Contestants.Count);
            _fakeTournamentDataAdapter.Verify(x => x.Save(tournament), Times.Never);
        }
    }
}
