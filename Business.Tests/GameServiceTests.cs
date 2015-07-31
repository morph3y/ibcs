using System;
using Contracts.Business.Dal;
using Contracts.Business.Tournaments;
using Contracts.Session;
using Entities;
using Moq;
using NUnit.Framework;

namespace Business.Tests
{
    [TestFixture]
    internal sealed class GameServiceTests : BusinessTestBase
    {
        private GameService _testSubject;
        private Mock<ITournamentStageService> _fakeTournamentStageService;
        private Mock<IGameDataAdapter> _fakeGameDataAdapter;

        [SetUp]
        public void Setup()
        {
            _fakeTournamentStageService = new Mock<ITournamentStageService>();
            _fakeGameDataAdapter = new Mock<IGameDataAdapter>();

            _testSubject = new GameService(_fakeTournamentStageService.Object, _fakeGameDataAdapter.Object);
        }

        [TearDown]
        public void Teardown()
        {
            Session.Current = InitialPrincipal;
        }

        [Test]
        public void VerifyCanEndGame()
        {
            // Arrange
            var game = new Game
            {
                TournamentStage = new TournamentStage
                {
                    Tournament = new Tournament()
                },
                Participant1 = new Player { Id = 1 },
                Participant1Score = 1,
                Participant2 = new Player { Id = 2 },
                Participant2Score = 2,
                Winner = new Player { Id = 2 }
            };
            _fakeTournamentStageService.Setup(x => x.UpdateStages(It.IsAny<Tournament>()));

            // Act
            _testSubject.EndGame(game);

            // Assert
            Assert.AreEqual(GameStatus.Finished, game.Status);
            _fakeTournamentStageService.Verify(x => x.UpdateStages(It.IsAny<Tournament>()), Times.Once);
        }

        [Test]
        public void VerifyCanNotEndGame()
        {
            // Arrange
            var game = new Game
            {
                TournamentStage = new TournamentStage
                {
                    Tournament = new Tournament()
                }
            };
            // Act / Assert
            Assert.Throws<Exception>(() => _testSubject.EndGame(game));
        }
    }
}
