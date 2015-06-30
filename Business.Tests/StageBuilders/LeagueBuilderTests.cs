using System.Collections.Generic;
using System.Linq;
using Business.Tournaments.StageBuilders;
using Entities;
using NUnit.Framework;

namespace Business.Tests.StageBuilders
{
    [TestFixture]
    internal sealed class LeagueBuilderTests : StageBuilderTestBase
    {
        [Test]
        public void VerifyDoesNotGenerateAnyGamesWhenOneContestant()
        {
            // Arrange
            var tournament = new Tournament
            {
                Stages = new List<TournamentStage>(),
                Status = TournamentStatus.Registration,
                TournamentType = TournamentType.League,
                Contestants = new List<Subject>()
            };
            var testSubject = new LeagueStageBuilder(tournament);

            // Act
            testSubject.Build();

            // Assert
            Assert.AreEqual(0, tournament.Stages.Count);
        }

        [Test]
        public void VerifyCreatesOneStageWithMinimumTwoPlayers()
        {
            // Arrange
            var player1 = new Player { Id = 1 };
            var player2 = new Player { Id = 2 };
            var tournament = new Tournament
            {
                Stages = new List<TournamentStage>(),
                Status = TournamentStatus.Registration,
                TournamentType = TournamentType.League,
                Contestants = new List<Subject>
                {
                    player1, player2
                }
            };
            var testSubject = new LeagueStageBuilder(tournament);

            // Act
            testSubject.Build();

            // Assert
            Assert.AreEqual(1, tournament.Stages.Count);
            Assert.AreEqual(1, tournament.Stages[0].Games.Count);
            Assert.IsTrue(StageHasUniquePlayers(tournament.Stages[0]));
        }

        [Test]
        public void VerifyGeneratesCorrectStagesAndGames13Players()
        {
            // Arrange
            var tournament = new Tournament
            {
                Stages = new List<TournamentStage>(),
                Status = TournamentStatus.Registration,
                TournamentType = TournamentType.League,
                Contestants = new List<Subject>(GeneratePlayers(13))
            };
            var testSubject = new LeagueStageBuilder(tournament);

            // Act
            testSubject.Build();

            // Assert
            Assert.AreEqual(15, tournament.Stages.Count);
            Assert.AreEqual(6, tournament.Stages[0].Games.Count);
            Assert.AreEqual(5, tournament.Stages[7].Games.Count);
            Assert.AreEqual(5, tournament.Stages[14].Games.Count);
            foreach (var tournamentStage in tournament.Stages)
            {
                Assert.IsTrue(StageHasUniquePlayers(tournamentStage));
            }
        }

        [Test]
        public void VerifyGeneratesCorrectStagesAndGames26Players()
        {
            // Arrange
            var tournament = new Tournament
            {
                Stages = new List<TournamentStage>(),
                Status = TournamentStatus.Registration,
                TournamentType = TournamentType.League,
                Contestants = new List<Subject>(GeneratePlayers(26))
            };
            var testSubject = new LeagueStageBuilder(tournament);

            // Act
            testSubject.Build();

            // Assert
            Assert.AreEqual(31, tournament.Stages.Count);
            Assert.AreEqual(13, tournament.Stages[0].Games.Count);
            Assert.AreEqual(10, tournament.Stages[15].Games.Count);
            Assert.AreEqual(10, tournament.Stages[28].Games.Count);
            foreach (var tournamentStage in tournament.Stages)
            {
                Assert.IsTrue(StageHasUniquePlayers(tournamentStage));
            }
        }
    }
}
