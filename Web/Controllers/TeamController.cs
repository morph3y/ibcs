using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Contracts.Business;
using Entities;
using Web.Infrastructure;
using Web.Models.Dto;

namespace Web.Controllers
{
    [Authorize]
    public class TeamController : Controller
    {
        private readonly ITeamService _teamService;
        private readonly IPlayerService _playerService;
        public TeamController(ITeamService teamService, IPlayerService playerService)
        {
            _teamService = teamService;
            _playerService = playerService;
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
        public ActionResult Edit(TeamDto teamModel)
        {
            var team = _teamService.Get(x => x.Id == teamModel.Id);
            if (team == null)
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

            return RedirectToAction("Edit", new { id = team.Id });
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            return View(id.HasValue ? _teamService.Get(x => x.Id == id) : new Team()
            {
                Captain = _playerService.Get(x=>x.Id == Contracts.Session.Session.Current.Id)
            });
        }

        public JsonResult AddMember(int teamId, int memberId)
        {
            var member = _playerService.Get(x => x.Id == memberId);
            _teamService.AddMember(teamId, member);
            return Json(new GenericDataContractJsonSerializer<SubjectDto>().Serialize(DtoBuilder.BuildSubject(member)));
        }

        public JsonResult GetAvailableMembers(int teamId)
        {
            var members = _teamService.GetAvailableMembers(teamId);
            return Json(new GenericDataContractJsonSerializer<IList<SubjectDto>>().Serialize(members.Select(DtoBuilder.BuildSubject).ToList()));
        }

        public JsonResult RemoveMember(int teamId, int memberId)
        {
            _teamService.RemoveMember(teamId, memberId);
            return Json(new { success = true });
        }
    }
}
