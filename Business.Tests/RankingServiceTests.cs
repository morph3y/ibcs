using System.Collections.Generic;
using System.Linq;
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
        private Mock<IRankingAdapter> _fakeRankingAdapter;

        [SetUp]
        public void Setup()
        {
            _fakeRankingAdapter = new Mock<IRankingAdapter>();

            _testSubject = new RankingService(_fakeRankingAdapter.Object);
        }

        [Test]
        public void VerifyCanRankPlayers()
        {
            // Arrange
            var resultRanks = new List<RankModel>
            {
                new RankModel { Elo = 2100, Subject = new Player { Name = "Player 1" } },
                new RankModel { Elo = 2005, Subject = new Player { Name = "Player 2" } },
                new RankModel { Elo = 2090, Subject = new Player { Name = "Player 3" } }
            };
            _fakeRankingAdapter.Setup(x => x.GetRanks(It.IsAny<IEnumerable<Subject>>())).Returns(resultRanks);

            // Act
            var rankingResult = _testSubject.Rank(null);

            // Assert
            Assert.AreEqual(3, rankingResult.Count());
            Assert.AreEqual("Player 1", rankingResult.Skip(0).Take(1).First().Name);
            Assert.AreEqual("Player 3", rankingResult.Skip(1).Take(1).First().Name);
            Assert.AreEqual("Player 2", rankingResult.Skip(2).Take(1).First().Name);
        }
    }
}
