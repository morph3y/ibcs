using System.Collections.Generic;
using System.Linq;
using Business.Tournaments.StageBuilders;
using Contracts.Business;
using Entities;
using Moq;
using NUnit.Framework;

namespace Business.Tests.StageBuilders
{
    [TestFixture]
    internal sealed class SingleEliminationBuilderTests : StageBuilderTestBase
    {
        private Mock<IRankingService> _fakeRankingService;

        [SetUp]
        public void Setup()
        {
            _fakeRankingService = new Mock<IRankingService>();
            _fakeRankingService.Setup(x => x.Rank(It.IsAny<List<Subject>>())).Returns((List<Subject> list) => list);
        }

        [Test]
        public void VerifyCanNotCreateWithOneOrZeroPlayers()
        {
            // Arrange
            var tournament = new Tournament
            {
                Stages = new List<TournamentStage>(),
                Status = TournamentStatus.Registration,
                Contestants = new List<Subject>()
            };
            var testSubject = new SingleEliminationStageBuilder(tournament, _fakeRankingService.Object);

            // Act
            testSubject.Build();

            // Assert
            Assert.AreEqual(0, tournament.Stages.Count);
        }

        [Test]
        public void VerifyCanGenerateOneStageForTwoPlayers()
        {
            // Arrange
            var tournament = new Tournament
            {
                Stages = new List<TournamentStage>(),
                Status = TournamentStatus.Registration,
                Contestants = new List<Subject>(GeneratePlayers(2))
            };
            var testSubject = new SingleEliminationStageBuilder(tournament, _fakeRankingService.Object);

            // Act
            testSubject.Build();

            // Assert
            Assert.AreEqual(1, tournament.Stages.Count);
            Assert.AreEqual(1, tournament.Stages[0].Games.Count);
            Assert.AreEqual(0, tournament.Stages[0].Games[0].Participant1.Id);
            Assert.AreEqual(1, tournament.Stages[0].Games[0].Participant2.Id);
        }

        [Test]
        public void VerifyGenerateCorrectGamesFor7Players()
        {
            // Arrange
            var tournament = new Tournament
            {
                Stages = new List<TournamentStage>(),
                Status = TournamentStatus.Registration,
                Contestants = new List<Subject>(GeneratePlayers(7))
            };
            var testSubject = new SingleEliminationStageBuilder(tournament, _fakeRankingService.Object);

            // Act
            testSubject.Build();

            // Assert
            Assert.AreEqual(1, tournament.Stages.Count);
            Assert.AreEqual(4, tournament.Stages[0].Games.Count);
            Assert.IsTrue(tournament.Stages[0].Games[0].Participant1 == null || tournament.Stages[0].Games[0].Participant2 == null);

            Assert.AreEqual(2, tournament.Stages[0].Games[1].Participant1.Id);
            Assert.AreEqual(5, tournament.Stages[0].Games[1].Participant2.Id);

            Assert.AreEqual(3, tournament.Stages[0].Games[2].Participant1.Id);
            Assert.AreEqual(4, tournament.Stages[0].Games[2].Participant2.Id);
        }

        [Test]
        public void VerifyGenerateCorrectGamesFor33Players()
        {
            // Arrange
            var tournament = new Tournament
            {
                Stages = new List<TournamentStage>(),
                Status = TournamentStatus.Registration,
                Contestants = new List<Subject>(GeneratePlayers(33))
            };
            var testSubject = new SingleEliminationStageBuilder(tournament, _fakeRankingService.Object);

            // Act
            testSubject.Build();

            // Assert
            // Two stages - BYE player lost by default
            Assert.AreEqual(2, tournament.Stages.Count);
            Assert.AreEqual(32, tournament.Stages[0].Games.Count);
            Assert.IsTrue(tournament.Stages[0].Games.All(x => x.Order < 32));

            Assert.IsTrue(tournament.Stages[0].Games[0].Participant1 == null || tournament.Stages[0].Games[0].Participant2 == null);
            Assert.IsTrue(tournament.Stages[0].Games[31].Participant1 == null || tournament.Stages[0].Games[0].Participant2 == null);

            Assert.IsTrue(tournament.Stages[0].Games[1].Participant1 != null && tournament.Stages[0].Games[1].Participant2 != null);
            Assert.AreEqual(31, tournament.Stages[0].Games[1].Participant1.Id);
            Assert.AreEqual(32, tournament.Stages[0].Games[1].Participant2.Id);
        }

        [Test]
        public void VerifyGenerateCorrectGamesFor32Players()
        {
            // Arrange
            var tournament = new Tournament
            {
                Stages = new List<TournamentStage>(),
                Status = TournamentStatus.Registration,
                Contestants = new List<Subject>(GeneratePlayers(32))
            };
            var testSubject = new SingleEliminationStageBuilder(tournament, _fakeRankingService.Object);

            // Act
            testSubject.Build();

            // Assert
            Assert.AreEqual(1, tournament.Stages.Count);
            Assert.AreEqual(16, tournament.Stages[0].Games.Count);

            #region Assert 32 Players
            Assert.IsTrue(tournament.Stages[0].Games[1].Participant1.Id == 15 || tournament.Stages[0].Games[1].Participant2.Id == 15);
            Assert.IsTrue(tournament.Stages[0].Games[1].Participant1.Id == 16 || tournament.Stages[0].Games[1].Participant2.Id == 16);

            Assert.IsTrue(tournament.Stages[0].Games[2].Participant1.Id == 7 || tournament.Stages[0].Games[2].Participant2.Id == 7);
            Assert.IsTrue(tournament.Stages[0].Games[2].Participant1.Id == 24 || tournament.Stages[0].Games[2].Participant2.Id == 24);

            Assert.IsTrue(tournament.Stages[0].Games[3].Participant1.Id == 8 || tournament.Stages[0].Games[3].Participant2.Id == 8);
            Assert.IsTrue(tournament.Stages[0].Games[3].Participant1.Id == 23 || tournament.Stages[0].Games[3].Participant2.Id == 23);

            Assert.IsTrue(tournament.Stages[0].Games[4].Participant1.Id == 3 || tournament.Stages[0].Games[4].Participant2.Id == 3);
            Assert.IsTrue(tournament.Stages[0].Games[4].Participant1.Id == 28 || tournament.Stages[0].Games[4].Participant2.Id == 28);

            Assert.IsTrue(tournament.Stages[0].Games[5].Participant1.Id == 12 || tournament.Stages[0].Games[5].Participant2.Id == 12);
            Assert.IsTrue(tournament.Stages[0].Games[5].Participant1.Id == 19 || tournament.Stages[0].Games[5].Participant2.Id == 19);

            Assert.IsTrue(tournament.Stages[0].Games[6].Participant1.Id == 11 || tournament.Stages[0].Games[6].Participant2.Id == 11);
            Assert.IsTrue(tournament.Stages[0].Games[6].Participant1.Id == 20 || tournament.Stages[0].Games[6].Participant2.Id == 20);

            Assert.IsTrue(tournament.Stages[0].Games[7].Participant1.Id == 4 || tournament.Stages[0].Games[7].Participant2.Id == 4);
            Assert.IsTrue(tournament.Stages[0].Games[7].Participant1.Id == 27 || tournament.Stages[0].Games[7].Participant2.Id == 27);

            Assert.IsTrue(tournament.Stages[0].Games[8].Participant1.Id == 5 || tournament.Stages[0].Games[8].Participant2.Id == 5);
            Assert.IsTrue(tournament.Stages[0].Games[8].Participant1.Id == 26 || tournament.Stages[0].Games[8].Participant2.Id == 26);

            Assert.IsTrue(tournament.Stages[0].Games[9].Participant1.Id == 10 || tournament.Stages[0].Games[9].Participant2.Id == 10);
            Assert.IsTrue(tournament.Stages[0].Games[9].Participant1.Id == 21 || tournament.Stages[0].Games[9].Participant2.Id == 21);

            Assert.IsTrue(tournament.Stages[0].Games[10].Participant1.Id == 13 || tournament.Stages[0].Games[10].Participant2.Id == 13);
            Assert.IsTrue(tournament.Stages[0].Games[10].Participant1.Id == 18 || tournament.Stages[0].Games[10].Participant2.Id == 18);

            Assert.IsTrue(tournament.Stages[0].Games[11].Participant1.Id == 2 || tournament.Stages[0].Games[11].Participant2.Id == 2);
            Assert.IsTrue(tournament.Stages[0].Games[11].Participant1.Id == 29 || tournament.Stages[0].Games[11].Participant2.Id == 29);

            Assert.IsTrue(tournament.Stages[0].Games[12].Participant1.Id == 6 || tournament.Stages[0].Games[12].Participant2.Id == 6);
            Assert.IsTrue(tournament.Stages[0].Games[12].Participant1.Id == 25 || tournament.Stages[0].Games[12].Participant2.Id == 25);

            Assert.IsTrue(tournament.Stages[0].Games[13].Participant1.Id == 9 || tournament.Stages[0].Games[13].Participant2.Id == 9);
            Assert.IsTrue(tournament.Stages[0].Games[13].Participant1.Id == 22 || tournament.Stages[0].Games[13].Participant2.Id == 22);

            Assert.IsTrue(tournament.Stages[0].Games[14].Participant1.Id == 14 || tournament.Stages[0].Games[14].Participant2.Id == 14);
            Assert.IsTrue(tournament.Stages[0].Games[14].Participant1.Id == 17 || tournament.Stages[0].Games[14].Participant2.Id == 17);

            Assert.IsTrue(tournament.Stages[0].Games[15].Participant1.Id == 1 || tournament.Stages[0].Games[15].Participant2.Id == 1);
            Assert.IsTrue(tournament.Stages[0].Games[15].Participant1.Id == 30 || tournament.Stages[0].Games[15].Participant2.Id == 30);
            #endregion
        }

        [Test]
        public void VerifyCanGenerateCorrectGamesByRanking()
        {
            // Arrange
            var rankedPlayers = GeneratePlayers(8);
            // Arrange
            var tournament = new Tournament
            {
                Stages = new List<TournamentStage>(),
                Status = TournamentStatus.Registration,
                Contestants = rankedPlayers
            };
            var testSubject = new SingleEliminationStageBuilder(tournament, _fakeRankingService.Object);

            // Act
            testSubject.Build();

            // Assert
            Assert.AreEqual(1, tournament.Stages.Count);
            Assert.AreEqual(4, tournament.Stages[0].Games.Count);

            Assert.IsTrue(tournament.Stages[0].Games[0].Participant1.Id == 0 || tournament.Stages[0].Games[0].Participant2.Id == 0);
            Assert.IsTrue(tournament.Stages[0].Games[0].Participant1.Id == 7 || tournament.Stages[0].Games[0].Participant2.Id == 7);

            Assert.IsTrue(tournament.Stages[0].Games[1].Participant1.Id == 2 || tournament.Stages[0].Games[1].Participant2.Id == 2);
            Assert.IsTrue(tournament.Stages[0].Games[1].Participant1.Id == 5 || tournament.Stages[0].Games[1].Participant2.Id == 5);

            Assert.IsTrue(tournament.Stages[0].Games[2].Participant1.Id == 3 || tournament.Stages[0].Games[2].Participant2.Id == 3);
            Assert.IsTrue(tournament.Stages[0].Games[2].Participant1.Id == 4 || tournament.Stages[0].Games[2].Participant2.Id == 4);

            Assert.IsTrue(tournament.Stages[0].Games[3].Participant1.Id == 1 || tournament.Stages[0].Games[3].Participant2.Id == 1);
            Assert.IsTrue(tournament.Stages[0].Games[3].Participant1.Id == 6 || tournament.Stages[0].Games[3].Participant2.Id == 6);
        }

        [Test]
        public void VerifyCanGenerateCorrectGamesWithByeByRanking()
        {
            // Arrange
            var rankedPlayers = GeneratePlayers(6);
            // Arrange
            var tournament = new Tournament
            {
                Stages = new List<TournamentStage>(),
                Status = TournamentStatus.Registration,
                Contestants = rankedPlayers
            };
            var testSubject = new SingleEliminationStageBuilder(tournament, _fakeRankingService.Object);

            // Act
            testSubject.Build();

            // Assert
            // One stage - we don't have a game there yet
            Assert.AreEqual(1, tournament.Stages.Count);
            Assert.AreEqual(4, tournament.Stages[0].Games.Count);

            Assert.IsTrue(tournament.Stages[0].Games[0].Participant1.Id == 0 || tournament.Stages[0].Games[0].Participant2.Id == 0);
            Assert.IsTrue(tournament.Stages[0].Games[0].Winner.Id == 0);

            Assert.IsTrue(tournament.Stages[0].Games[1].Participant1.Id == 2 || tournament.Stages[0].Games[1].Participant2.Id == 2);
            Assert.IsTrue(tournament.Stages[0].Games[1].Participant1.Id == 5 || tournament.Stages[0].Games[1].Participant2.Id == 5);

            Assert.IsTrue(tournament.Stages[0].Games[2].Participant1.Id == 3 || tournament.Stages[0].Games[2].Participant2.Id == 3);
            Assert.IsTrue(tournament.Stages[0].Games[2].Participant1.Id == 4 || tournament.Stages[0].Games[2].Participant2.Id == 4);

            Assert.IsTrue(tournament.Stages[0].Games[3].Participant1.Id == 1 || tournament.Stages[0].Games[3].Participant2.Id == 1);
            Assert.IsTrue(tournament.Stages[0].Games[3].Winner.Id == 1);
        }

        [Test]
        public void VerifyCanUpdateGameWithoutAdvancing()
        {
            // Arrange
            var tournament = new Tournament
            {
                Stages = new List<TournamentStage>(),
                Status = TournamentStatus.Registration,
                Contestants = new List<Subject>(GeneratePlayers(4))
            };
            var testSubject = new SingleEliminationStageBuilder(tournament, _fakeRankingService.Object);
            testSubject.Build();

            // Act
            tournament.Stages[0].Games[0].Participant1Score = 1;
            tournament.Stages[0].Games[0].Participant2Score = 0;
            tournament.Stages[0].Games[0].Winner = tournament.Stages[0].Games[0].Participant1;
            tournament.Stages[0].Games[0].Status = GameStatus.Finished;
            testSubject.Update();

            // Assert
            Assert.AreEqual(1, tournament.Stages.Count);
            Assert.AreEqual(2, tournament.Stages[0].Games.Count);
        }

        [Test]
        public void VerifyCanUpdateGameWithAdvancing()
        {
            // Arrange
            var tournament = new Tournament
            {
                Stages = new List<TournamentStage>(),
                Status = TournamentStatus.Registration,
                Contestants = new List<Subject>(GeneratePlayers(4))
            };
            var testSubject = new SingleEliminationStageBuilder(tournament, _fakeRankingService.Object);
            testSubject.Build();

            // Act
            tournament.Stages[0].Games[0].Participant1Score = 1;
            tournament.Stages[0].Games[0].Participant2Score = 0;
            tournament.Stages[0].Games[0].Winner = tournament.Stages[0].Games[0].Participant1;
            tournament.Stages[0].Games[0].Status = GameStatus.Finished;
            testSubject.Update();

            Assert.AreEqual(1, tournament.Stages.Count);

            tournament.Stages[0].Games[1].Participant1Score = 1;
            tournament.Stages[0].Games[1].Participant2Score = 0;
            tournament.Stages[0].Games[1].Winner = tournament.Stages[0].Games[0].Participant1;
            tournament.Stages[0].Games[1].Status = GameStatus.Finished;
            testSubject.Update();

            // Assert
            Assert.AreEqual(2, tournament.Stages.Count);
            Assert.AreEqual(1, tournament.Stages[1].Games.Count);
        }

        [Test]
        public void VerifyCanFinishTournament()
        {
            // Arrange
            var tournament = new Tournament
            {
                Stages = new List<TournamentStage>(),
                Status = TournamentStatus.Registration,
                Contestants = new List<Subject>(GeneratePlayers(2))
            };
            var testSubject = new SingleEliminationStageBuilder(tournament, _fakeRankingService.Object);
            testSubject.Build();

            // Act
            tournament.Stages[0].Games[0].Participant1Score = 1;
            tournament.Stages[0].Games[0].Participant2Score = 0;
            tournament.Stages[0].Games[0].Winner = tournament.Stages[0].Games[0].Participant1;
            tournament.Stages[0].Games[0].Status = GameStatus.Finished;
            testSubject.Update();

            // Assert
            Assert.AreEqual(1, tournament.Stages.Count);
            Assert.AreEqual(TournamentStatus.Closed, tournament.Status);
        }
    }
}
