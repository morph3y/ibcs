using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Business.Tournaments;
using Business.Tournaments.StageBuilders;
using Contracts.Business;
using Contracts.Business.Dal;
using Contracts.Session;
using Entities;
using Moq;
using NUnit.Framework;

namespace Business.Tests
{
    [TestFixture]
    internal sealed class TournamentServiceTests : BusinessTestBase
    {
        private TournamentService _testSubject;
        private Mock<ITournamentDataAdapter> _fakeTournamentDataAdapter;
        private Mock<ISubjectService> _fakeSubjectService;
        private Mock<IRankingService> _fakeRankingService;
        private Mock<IStageBuilderFactory> _fakeStageBuilderFactory;
        private Mock<IStageBuilder> _fakeStageBuilder;

        [SetUp]
        public void Setup()
        {
            _fakeTournamentDataAdapter = new Mock<ITournamentDataAdapter>();
            _fakeSubjectService = new Mock<ISubjectService>();
            _fakeRankingService = new Mock<IRankingService>();
            _fakeStageBuilderFactory = new Mock<IStageBuilderFactory>();
            _fakeStageBuilder = new Mock<IStageBuilder>();

            _fakeStageBuilderFactory.Setup(x => x.Create(It.IsAny<Tournament>())).Returns(_fakeStageBuilder.Object);

            _testSubject = new TournamentService(_fakeTournamentDataAdapter.Object, _fakeSubjectService.Object, _fakeRankingService.Object, _fakeStageBuilderFactory.Object);
        }

        [TearDown]
        public void TearDown()
        {
            Session.Current = InitialPrincipal;
        }

        [Test]
        public void VerifyCanCreateTournament()
        {
            // Arrange
            var tournamentToCreate = new Tournament { Status = TournamentStatus.Closed };

            // Act
            _testSubject.Create(tournamentToCreate);

            // Assert
            Assert.AreEqual(TournamentStatus.Registration, tournamentToCreate.Status);
        }

        [Test]
        public void VerifyCanAddContestant()
        {
            // Arrange
            var contestantToAdd = new Player();
            var tournament = new Tournament { Status = TournamentStatus.Registration, Contestants = new List<Subject>(), IsTeamEvent = false };
            _fakeSubjectService.Setup(x => x.Get(It.IsAny<Expression<Func<Subject, bool>>>())).Returns(contestantToAdd);

            // Act
            _testSubject.AddContestant(0, tournament);

            // Assert
            Assert.AreEqual(1, tournament.Contestants.Count);
        }

        [Test]
        public void VerifyCanNotAddContestantStartedTournament()
        {
            // Arrange
            var contestantToAdd = new Player() { Id = 1 };
            var tournament = new Tournament { Status = TournamentStatus.Active, Contestants = new List<Subject>(), IsTeamEvent = false };
            _fakeSubjectService.Setup(x => x.Get(It.IsAny<Expression<Func<Subject, bool>>>())).Returns(contestantToAdd);

            // Act / Assert
            Assert.Throws<Exception>(() => _testSubject.AddContestant(0, tournament));
        }

        [Test]
        public void VerifyCanNotAddContestantInTeamTournament()
        {
            // Arrange
            var contestantToAdd = new Player();
            var tournament = new Tournament { Status = TournamentStatus.Registration, Contestants = new List<Subject>(), IsTeamEvent = true };
            _fakeSubjectService.Setup(x => x.Get(It.IsAny<Expression<Func<Subject, bool>>>())).Returns(contestantToAdd);

            // Act / Assert
            Assert.Throws<Exception>(() => _testSubject.AddContestant(0, tournament));
        }

        [Test]
        public void VerifyCanNotAddContestantTeamInPlayerTournament()
        {
            // Arrange
            var contestantToAdd = new Team();
            var tournament = new Tournament { Status = TournamentStatus.Registration, Contestants = new List<Subject>(), IsTeamEvent = false };
            _fakeSubjectService.Setup(x => x.Get(It.IsAny<Expression<Func<Subject, bool>>>())).Returns(contestantToAdd);

            // Act / Assert
            Assert.Throws<Exception>(() => _testSubject.AddContestant(0, tournament));
        }

        [Test]
        public void VerifyCanNotAddContestantIfNotAdminOrLoggedUserIsContestant()
        {
            // Arrange
            var contestantToAdd = new Player { Id = 3 };
            var tournament = new Tournament { Status = TournamentStatus.Registration, Contestants = new List<Subject>(), IsTeamEvent = false };
            _fakeSubjectService.Setup(x => x.Get(It.IsAny<Expression<Func<Subject, bool>>>())).Returns(contestantToAdd);
            Session.Current.IsAdmin = false;

            // Act / Assert
            Assert.Throws<Exception>(() => _testSubject.AddContestant(0, tournament));
        }

        [Test]
        public void VerifyCanNotAddContestantIfNonAdminOrLoggedUserIfTeamCaptain()
        {
            // Arrange
            var contestantToAdd = new Team { Id = 3, Captain = new Player { Id = 3 } };
            var tournament = new Tournament { Status = TournamentStatus.Registration, Contestants = new List<Subject>(), IsTeamEvent = true };
            _fakeSubjectService.Setup(x => x.Get(It.IsAny<Expression<Func<Subject, bool>>>())).Returns(contestantToAdd);
            Session.Current.IsAdmin = false;

            // Act / Assert
            Assert.Throws<Exception>(() => _testSubject.AddContestant(0, tournament));
        }

        [Test]
        public void VerifyCanRemoveContestant()
        {
            // Arrange
            var contestant = new Player { Id = 1 };
            var tournament = new Tournament { Status = TournamentStatus.Registration, IsTeamEvent = false, Contestants = new List<Subject> { contestant }};
            _fakeSubjectService.Setup(x => x.Get(It.IsAny<Expression<Func<Subject, bool>>>())).Returns(contestant);

            // Act
            _testSubject.RemoveContestant(0, tournament);

            // Assert
            Assert.AreEqual(0, tournament.Contestants.Count);
        }

        [Test]
        public void VerifyCanNotRemovePlayerContestantIfTeamEvent()
        {
            // Arrange
            var contestantToRemove = new Player();
            var tournament = new Tournament { Status = TournamentStatus.Active, Contestants = new List<Subject>(), IsTeamEvent = true };
            _fakeSubjectService.Setup(x => x.Get(It.IsAny<Expression<Func<Subject, bool>>>())).Returns(contestantToRemove);

            // Act / Assert
            Assert.Throws<Exception>(() => _testSubject.RemoveContestant(0, tournament));
        }

        [Test]
        public void VerifyCanNotRemoveTeamContestantIfPlayerEvent()
        {
            // Arrange
            var contestantToRemove = new Team();
            var tournament = new Tournament { Status = TournamentStatus.Registration, Contestants = new List<Subject>(), IsTeamEvent = false };
            _fakeSubjectService.Setup(x => x.Get(It.IsAny<Expression<Func<Subject, bool>>>())).Returns(contestantToRemove);

            // Act / Assert
            Assert.Throws<Exception>(() => _testSubject.RemoveContestant(0, tournament));
        }

        [Test]
        public void VerifyCanNotRemoveContestantIfNotAdminOrLoggedUserIsContestant()
        {
            // Arrange
            var contestantToRemove = new Player { Id = 4 };
            var tournament = new Tournament { Status = TournamentStatus.Registration, Contestants = new List<Subject>(), IsTeamEvent = false };
            _fakeSubjectService.Setup(x => x.Get(It.IsAny<Expression<Func<Subject, bool>>>())).Returns(contestantToRemove);
            Session.Current.IsAdmin = false;

            // Act / Assert
            Assert.Throws<Exception>(() => _testSubject.RemoveContestant(0, tournament));
        }

        [Test]
        public void VerifyCanNotRemoveContestantIfNonAdminOrLoggedUserNotTeamCaptain()
        {
            // Arrange
            var contestantToRemove = new Team { Id = 4, Captain = new Player { Id = 3 } };
            var tournament = new Tournament { Status = TournamentStatus.Registration, Contestants = new List<Subject>(), IsTeamEvent = true };
            _fakeSubjectService.Setup(x => x.Get(It.IsAny<Expression<Func<Subject, bool>>>())).Returns(contestantToRemove);
            Session.Current.IsAdmin = false;

            // Act / Assert
            Assert.Throws<Exception>(() => _testSubject.RemoveContestant(0, tournament));
        }

        [Test]
        public void CanGenerateGames()
        {
            // Arrange / Act
            _testSubject.GenerateStages(new Tournament());

            // Assert
            _fakeStageBuilderFactory.Verify(x => x.Create(It.IsAny<Tournament>()), Times.Once);
            _fakeStageBuilder.Verify(x => x.Build(), Times.Once);
        }

        [Test]
        public void CanUpdateStages()
        {
            // Arrange / Act
            _testSubject.UpdateStages(new Tournament());

            // Assert
            _fakeStageBuilderFactory.Verify(x => x.Create(It.IsAny<Tournament>()), Times.Once);
            _fakeStageBuilder.Verify(x => x.Update(), Times.Once);
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
            _fakeTournamentDataAdapter.Verify(x => x.Save(tournament), Times.Once);
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
