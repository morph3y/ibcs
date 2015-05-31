using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Business.Tournaments;
using Contracts.Business;
using Contracts.Business.Dal;
using Contracts.Business.Tournaments;
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
        private Mock<ITournamentStageService> _fakeTournamentStageService;
        private Mock<ISubjectService> _fakeSubjectService;

        [SetUp]
        public void Setup()
        {
            _fakeTournamentDataAdapter = new Mock<ITournamentDataAdapter>();
            _fakeTournamentStageService = new Mock<ITournamentStageService>();
            _fakeSubjectService = new Mock<ISubjectService>();

            _testSubject = new TournamentService(_fakeTournamentDataAdapter.Object, _fakeTournamentStageService.Object, _fakeSubjectService.Object);
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
            _fakeTournamentStageService.Setup(x => x.GenerateStages(tournamentToCreate));

            // Act
            _testSubject.Create(tournamentToCreate);

            // Assert
            Assert.AreEqual(TournamentStatus.Registration, tournamentToCreate.Status);
            _fakeTournamentStageService.Verify(x => x.GenerateStages(tournamentToCreate), Times.Once);
        }

        [Test]
        public void VerifyCanAddContestant()
        {
            // Arrange
            var contestantToAdd = new Player();
            var tournament = new Tournament { Status = TournamentStatus.Registration, Contestants = new List<Subject>(), IsTeamEvent = false };
            _fakeSubjectService.Setup(x => x.Get(It.IsAny<Expression<Func<Subject, bool>>>())).Returns(contestantToAdd);
            _fakeTournamentStageService.Setup(x => x.GenerateStages(tournament));

            // Act
            _testSubject.AddContestant(0, tournament);

            // Assert
            Assert.AreEqual(1, tournament.Contestants.Count);
            _fakeTournamentStageService.Verify(x => x.GenerateStages(tournament), Times.Once);
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
            var tournament = new Tournament { Status = TournamentStatus.Registration, IsTeamEvent = false };
            _fakeSubjectService.Setup(x => x.Get(It.IsAny<Expression<Func<Subject, bool>>>())).Returns(contestant);
            _fakeTournamentStageService.Setup(x => x.RemoveContestant(It.IsAny<Subject>(), It.IsAny<Tournament>()));

            // Act
            _testSubject.RemoveContestant(0, tournament);

            // Assert
            _fakeTournamentStageService.Verify(x => x.RemoveContestant(It.IsAny<Subject>(), It.IsAny<Tournament>()), Times.Once);
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
    }
}
