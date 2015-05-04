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
        private readonly IObjectService _objectService;
        private readonly ITeamService _teamService;
        public TeamController(IObjectService objectService, ITeamService teamService)
        {
            _objectService = objectService;
            _teamService = teamService;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        [AllowAnonymous]
        public ActionResult List()
        {
            return View(_objectService.GetCollection<Team>());
        }

        [HttpPost]
        public ActionResult Edit(TeamDto teamModel)
        {
            var team = _objectService.Get<Team>(x => x.Id == teamModel.Id);
            if (team == null)
            {
                team = new Team
                {
                    Captain = _objectService.Get<Player>(x => x.Id == Contracts.Session.Session.Current.Id),
                    Name = teamModel.Name
                };
            }
            else
            {
                team.Name = teamModel.Name;
            }
            _objectService.Save(team);

            return RedirectToAction("Edit", new { id = team.Id });
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            return View(id.HasValue ? _objectService.Get<Team>(x => x.Id == id) : new Team()
            {
                Captain = _objectService.Get<Player>(x=>x.Id == Contracts.Session.Session.Current.Id)
            });
        }

        public JsonResult AddMember(int teamId, int memberId)
        {
            var member = _objectService.Get<Player>(x => x.Id == memberId);
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
