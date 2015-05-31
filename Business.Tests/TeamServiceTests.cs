using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Contracts.Business.Dal;
using Contracts.Session;
using Entities;
using Moq;
using NUnit.Framework;

namespace Business.Tests
{
    [TestFixture]
    internal sealed class TeamServiceTests : BusinessTestBase
    {
        private TeamService _testSubject;
        private Mock<ITeamDataAdapter> _fakeTeamDataAdapter;
        private Mock<IPlayerDataAdapter> _fakePlayerDataAdapter;

        [SetUp]
        public void Setup()
        {
            _fakeTeamDataAdapter = new Mock<ITeamDataAdapter>();
            _fakePlayerDataAdapter = new Mock<IPlayerDataAdapter>();

            Session.Current.IsAdmin = true;

            _testSubject = new TeamService(_fakeTeamDataAdapter.Object, _fakePlayerDataAdapter.Object);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            Session.Current = InitialPrincipal;
        }

        [Test]
        public void VerifyCanAddMemberAdmin()
        {
            // Arrange
            var fakeTeam = new Team { Name = "fakeTeam" };
            var memberToAdd = new Player { Name = "fakeMember" };
            _fakeTeamDataAdapter.Setup(x => x.Get(It.IsAny<Expression<Func<Team, bool>>>())).Returns(fakeTeam);
            _fakeTeamDataAdapter.Setup(x => x.Save(fakeTeam));

            // Act
            _testSubject.AddMember(0, memberToAdd);

            // Assert
            // creates a member request
            Assert.AreEqual(0, fakeTeam.Members.Count);
            _fakeTeamDataAdapter.Verify(x => x.CreateRequest(It.IsAny<TeamMemberRequest>()), Times.Once);
        }

        [Test]
        public void VerifyCannotAddMemberIfNotCaptain()
        {
            // Arrange
            // Id is different from fake session
            var fakeTeam = new Team { Name = "fakeTeam", Captain = new Player { Id = 456 } };
            var memberToAdd = new Player { Name = "fakeMember" };
            Session.Current.IsAdmin = false;
            _fakeTeamDataAdapter.Setup(x => x.Get(It.IsAny<Expression<Func<Team, bool>>>())).Returns(fakeTeam);
            _fakeTeamDataAdapter.Setup(x => x.Save(fakeTeam));

            // Act / Assert
            Assert.Throws<Exception>(() => _testSubject.AddMember(0, memberToAdd));
        }

        [Test]
        public void VerifyCanAcceptMember()
        {
            // Arrange
            var fakeTeam = new Team { Name = "fakeTeam", Members = new List<Player>() };
            var memberToAccept = new Player { Name = "fakeMember" };
            _fakeTeamDataAdapter.Setup(x => x.Get(It.IsAny<Expression<Func<Team, bool>>>())).Returns(fakeTeam);
            _fakeTeamDataAdapter.Setup(x => x.Save(fakeTeam));

            // Act
            _testSubject.AcceptMember(0, memberToAccept);

            // Assert
            Assert.AreEqual(1, fakeTeam.Members.Count);
            _fakeTeamDataAdapter.Verify(x => x.Save(It.IsAny<Team>()), Times.Once);
        }

        [Test]
        public void VerifyCanNotAcceptMemberIfNotLoggedInMemberOrAdmin()
        {
            // Arrange
            // Id is different from logged in user
            var memberToAccept = new Player { Id = 2, Name = "fakeMember" };
            Session.Current.IsAdmin = false;

            // Act / Assert
            Assert.Throws<Exception>(() => _testSubject.AcceptMember(0, memberToAccept));
        }

        [Test]
        public void VerifyCanRemoveMemberFromTeam()
        {
            // Arrange
            var memberToRemove = new Player { Name = "fakeMember" };
            var fakeTeam = new Team { Name = "fakeTeam", Members = new List<Player> { memberToRemove }, ContestantIn = new List<Tournament>() };
            _fakeTeamDataAdapter.Setup(x => x.Get(It.IsAny<Expression<Func<Team, bool>>>())).Returns(fakeTeam);
            _fakePlayerDataAdapter.Setup(x => x.Get(It.IsAny<Expression<Func<Player, bool>>>())).Returns(memberToRemove);
            _fakeTeamDataAdapter.Setup(x => x.Save(fakeTeam));

            // Act
            _testSubject.RemoveMember(0, 1);

            // Assert
            Assert.AreEqual(0, fakeTeam.Members.Count);
            _fakeTeamDataAdapter.Verify(x => x.Save(It.IsAny<Team>()), Times.Once);
        }

        [Test]
        public void VerifyCanNotRemoveMemberFromTeamIfEnlistedInActiveTournament()
        {
            // Arrange
            var memberToRemove = new Player { Name = "fakeMember" };
            var rankedAndActiveTournament = new Tournament { Status = TournamentStatus.Active, IsRanked = true };
            var fakeTeam = new Team { Name = "fakeTeam", Members = new List<Player> { memberToRemove }, ContestantIn = new List<Tournament> { rankedAndActiveTournament } };
            _fakeTeamDataAdapter.Setup(x => x.Get(It.IsAny<Expression<Func<Team, bool>>>())).Returns(fakeTeam);
            _fakePlayerDataAdapter.Setup(x => x.Get(It.IsAny<Expression<Func<Player, bool>>>())).Returns(memberToRemove);
            _fakeTeamDataAdapter.Setup(x => x.Save(fakeTeam));

            // Act / Assert
            Assert.Throws<Exception>(() => _testSubject.RemoveMember(0, 2));
        }

        [Test]
        public void VerifyCanRemoveMemberRequestFromTeam()
        {
            // Arrange
            var memberToRemove = new Player { Name = "fakeMember" };
            var fakeTeam = new Team { Name = "fakeTeam", Members = new List<Player>(), ContestantIn = new List<Tournament>() };
            var fakeMemberRequest = new TeamMemberRequest { Member = memberToRemove, Team = fakeTeam };
            _fakeTeamDataAdapter.Setup(x => x.Get(It.IsAny<Expression<Func<Team, bool>>>())).Returns(fakeTeam);
            _fakeTeamDataAdapter.Setup(x => x.GetRequest(It.IsAny<int>(), It.IsAny<int>())).Returns(fakeMemberRequest);
            _fakeTeamDataAdapter.Setup(x => x.RemoveRequest(fakeMemberRequest));
            _fakePlayerDataAdapter.Setup(x => x.Get(It.IsAny<Expression<Func<Player, bool>>>())).Returns(memberToRemove);
            _fakeTeamDataAdapter.Setup(x => x.Save(fakeTeam));

            // Act
            _testSubject.RemoveMember(0, 1);

            // Assert
            Assert.AreEqual(0, fakeTeam.Members.Count);
            _fakeTeamDataAdapter.Verify(x => x.RemoveRequest(It.IsAny<TeamMemberRequest>()), Times.Once);
            _fakeTeamDataAdapter.Verify(x => x.Save(It.IsAny<Team>()), Times.Once);
        }

        [Test]
        public void VarifyCanNotRemoveMemberIfNotAdminOrCaptain()
        {
            // Arrange
            var memberToRemove = new Player { Name = "fakeMember" };
            // Different Id from logged in user
            var fakeTeam = new Team { Captain = new Player { Id = 456 }, Name = "fakeTeam", Members = new List<Player> { memberToRemove }, ContestantIn = new List<Tournament>() };
            _fakeTeamDataAdapter.Setup(x => x.Get(It.IsAny<Expression<Func<Team, bool>>>())).Returns(fakeTeam);
            _fakeTeamDataAdapter.Setup(x => x.Save(fakeTeam));
            Session.Current.IsAdmin = false;

            // Act / Assert
            Assert.Throws<Exception>(() => _testSubject.RemoveMember(0, 2));
        }
    }
}
