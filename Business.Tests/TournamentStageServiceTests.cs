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
    }
}
