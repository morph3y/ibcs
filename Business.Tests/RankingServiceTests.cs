using System.Collections.Generic;
using System.Linq;
using Business.Ranking;
using Contracts.Business;
using Contracts.Business.Dal;
using Entities;
using Moq;
using NUnit.Framework;

namespace Business.Tests
{
    [TestFixture]
    internal sealed class RankingServiceTests : BusinessTestBase
    {
        private IRankingService _testSubject;
        private Mock<IRankingDataAdapter> _fakeRankingAdapter;

        [SetUp]
        public void Setup()
        {
            _fakeRankingAdapter = new Mock<IRankingDataAdapter>();

            _testSubject = new RankingService(_fakeRankingAdapter.Object);
        }

        [Test]
        public void VerifyCanRankPlayers()
        {
            // Arrange
            var subjectsToRank = new List<Subject>
            {
                new Player {Name = "Player 1"},
                new Player {Name = "Player 2"},
                new Player {Name = "Player 3"}
            };

            var resultRanks = new List<Rank>
            {
                new Rank { Elo = 2100, Subject = subjectsToRank[0] },
                new Rank { Elo = 2005, Subject = subjectsToRank[1] },
                new Rank { Elo = 2090, Subject = subjectsToRank[2] }
            };
            _fakeRankingAdapter.Setup(x => x.GetRanks(It.IsAny<IEnumerable<Subject>>())).Returns(resultRanks);

            // Act
            var rankingResult = _testSubject.Rank(subjectsToRank);

            // Assert
            Assert.AreEqual(3, rankingResult.Count());
            Assert.AreEqual("Player 1", rankingResult.Skip(0).Take(1).First().Name);
            Assert.AreEqual("Player 3", rankingResult.Skip(1).Take(1).First().Name);
            Assert.AreEqual("Player 2", rankingResult.Skip(2).Take(1).First().Name);
        }

        [Test]
        public void CanInitializeRank()
        {
            // Arrange
            var subjectsToInitialize = new List<Subject>
            {
                new Player {Name = "Player 1"},
                new Player {Name = "Player 2"},
                new Player {Name = "Player 3"}
            };
            _fakeRankingAdapter.Setup(x => x.GetRanks(It.IsAny<IEnumerable<Subject>>())).Returns(new List<Rank>
            {
                new Rank { Elo = 2500, Subject = subjectsToInitialize[0] }
            });

            _fakeRankingAdapter.Setup(x => x.Save(It.IsAny<Rank>())).Callback((Rank r) =>
            {
                if (r.Elo != RankingService.StartingElo)
                {
                    Assert.Fail("failed to initialize rank to starting");
                }
            });

            // Act
            var rankingResult = _testSubject.Rank(subjectsToInitialize); // calls two times
            var initResult = _testSubject.InitRank(subjectsToInitialize); // initializes all

            // Assert
            Assert.AreEqual(3, rankingResult.Count());
            Assert.AreEqual(3, initResult.Count());
            _fakeRankingAdapter.Verify(x=>x.Save(It.IsAny<Rank>()), Times.Exactly(5));
        }

        [Test]
        public void VerifyCanMaintainRanks()
        {
            // Arrange
            var player1 = new Rank {Subject = new Player {Name = "Player 1"}, Elo = RankingService.StartingElo + 1};
            var player2 = new Rank {Subject = new Player {Name = "Player 2"}, Elo = RankingService.StartingElo + 100};
            var team1 = new Rank {Subject = new Team {Name = "Team 1"}, Elo = RankingService.StartingElo + 100};
            var punishableRanks = new List<Rank>
            {
                player1, player2, team1
            };

            _fakeRankingAdapter.Setup(x => x.GetRanksToPunish(It.IsAny<int>(), It.IsAny<int>())).Returns(punishableRanks);

            _fakeRankingAdapter.Setup(x => x.Save(player1)).Callback((Rank rank) =>
            {
                if (rank.Elo != RankingService.StartingElo)
                {
                    Assert.Fail("not equal to minimum");
                }
            });

            _fakeRankingAdapter.Setup(x => x.Save(player2)).Callback((Rank rank) =>
            {
                if (rank.Elo != (RankingService.StartingElo + 100) - RankingService.PunishElo)
                {
                    Assert.Fail("was not able to punish");
                }
            });

            _fakeRankingAdapter.Setup(x => x.Save(team1)).Callback((Rank rank) =>
            {
                if (rank.Elo != (RankingService.StartingElo + 100) - RankingService.PunishElo)
                {
                    Assert.Fail("was not able to punish team");
                }
            });

            // Act
            _testSubject.MaintainRanks();

            // Assert
            _fakeRankingAdapter.VerifyAll();
        }
    }
}
