﻿using System;
using Contracts.Business;
using Contracts.Business.Dal;
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
        private Mock<ITournamentService> _fakeTournamentStageService;
        private Mock<IGameDataAdapter> _fakeGameDataAdapter;
        private Mock<IRankingService> _fakeRankingService;

        [SetUp]
        public void Setup()
        {
            _fakeTournamentStageService = new Mock<ITournamentService>();
            _fakeGameDataAdapter = new Mock<IGameDataAdapter>();
            _fakeRankingService = new Mock<IRankingService>();

            _testSubject = new GameService(_fakeTournamentStageService.Object, _fakeGameDataAdapter.Object, _fakeRankingService.Object);
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
            _fakeTournamentStageService.Setup(x => x.Update(It.IsAny<Tournament>()));

            // Act
            _testSubject.EndGame(game);

            // Assert
            Assert.AreEqual(GameStatus.Finished, game.Status);
            _fakeTournamentStageService.Verify(x => x.Update(It.IsAny<Tournament>()), Times.Once);
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
