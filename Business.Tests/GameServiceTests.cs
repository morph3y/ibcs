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
                }
            };
            _fakeTournamentStageService.Setup(x => x.UpdateStages(It.IsAny<Tournament>()));

            // Act
            _testSubject.EndGame(game);

            // Assert
            Assert.AreEqual(GameStatus.Finished, game.Status);
            _fakeTournamentStageService.Verify(x => x.UpdateStages(It.IsAny<Tournament>()), Times.Once);
        }
    }
}
