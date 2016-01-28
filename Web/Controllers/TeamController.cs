using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Contracts.Business;
using Entities;
using Web.Infrastructure;
using Web.Models.TeamModels;
using Web.Models.TournamentModels.SubjectModels;

namespace Web.Controllers
{
    [Authorize]
    public class TeamController : Controller
    {
        private readonly ITeamService _teamService;
        private readonly IPlayerService _playerService;
        private readonly IRankingService _rankingService;
        public TeamController(ITeamService teamService, IPlayerService playerService, IRankingService rankingService)
        {
            _teamService = teamService;
            _playerService = playerService;
            _rankingService = rankingService;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        [AllowAnonymous]
        public ActionResult List()
        {
            return View(_teamService.GetList());
        }

        [HttpPost]
        public ActionResult Edit(TournamentTeamViewModel teamModel)
        {
            var team = _teamService.Get(x => x.Id == teamModel.Id);
            var isNew = team == null;
            if (isNew)
            {
                team = new Team
                {
                    Captain = _playerService.Get(x => x.Id == Contracts.Session.Session.Current.Id),
                    Name = teamModel.Name
                };
            }
            else
            {
                team.Name = teamModel.Name;
            }

            _teamService.Save(team);
            if (isNew)
            {
                _rankingService.InitRank(team);
            }

            return RedirectToAction("Edit", new { id = team.Id });
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            var team = _teamService.Get(x => x.Id == id);
            TournamentPlayerViewModel captain;
            var viewModel = new EditTeamViewModel
            {
                IsAuthorized = User.Identity.IsAuthenticated && (Contracts.Session.Session.Current.IsAdmin || (team != null && team.Captain != null && Contracts.Session.Session.Current.Id == team.Captain.Id)),
                Members = new List<TeamMemberViewModel>()
            };
            if (id.HasValue)
            {
                if (team != null)
                {
                    captain = (TournamentPlayerViewModel)SubjectViewModel.Build(team.Captain);
                    viewModel.Name = team.Name;
                    viewModel.Id = team.Id;
                    foreach (var member in team.Members)
                    {
                        viewModel.Members.Add(new TeamMemberViewModel
                        {
                            Accepted = true,
                            Id = member.Id,
                            Name = member.Name
                        });
                    }
                    if (viewModel.IsAuthorized)
                    {
                        var requests = _teamService.GetMembersRequests(id.Value);

                        foreach (var memberRequest in requests)
                        {
                            viewModel.Members.Add(new TeamMemberViewModel
                            {
                                Accepted = false,
                                Id = memberRequest.Member.Id,
                                Name = memberRequest.Member.Name
                            });
                        }
                    }
                }
                else
                {
                    captain = (TournamentPlayerViewModel)SubjectViewModel.Build(_playerService.Get(x => x.Id == Contracts.Session.Session.Current.Id));
                }
            }
            else
            {
                captain = (TournamentPlayerViewModel)SubjectViewModel.Build(_playerService.Get(x => x.Id == Contracts.Session.Session.Current.Id));
            }

            viewModel.Captain = captain;

            return View(viewModel);
        }

        public JsonResult AddMember(int teamId, int memberId)
        {
            var member = _playerService.Get(x => x.Id == memberId);
            _teamService.AddMember(teamId, member);
            return Json(new GenericDataContractJsonSerializer<TeamMemberViewModel>().Serialize(new TeamMemberViewModel
            {
                Accepted = false,
                Id = member.Id,
                Name = member.Name
            }));
        }

        public JsonResult AcceptRequest(int teamId, int memberId)
        {
            _teamService.AcceptMember(teamId, _playerService.Get(x=>x.Id == memberId));
            return Json(new { success = true });
        }

        public JsonResult RemoveMember(int teamId, int memberId)
        {
            _teamService.RemoveMember(teamId, memberId);
            return Json(new { success = true });
        }

        public JsonResult GetAvailableMembers(int teamId)
        {
            var members = _teamService.GetAvailableMembers(teamId);
            return Json(new GenericDataContractJsonSerializer<IList<SubjectViewModel>>().Serialize(members.Select(SubjectViewModel.Build).ToList()));
        }
    }
}
