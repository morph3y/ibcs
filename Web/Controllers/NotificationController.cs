using System.Web.Mvc;
using Contracts.Business;
using Web.Models.Notifications;

namespace Web.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly ITeamService _teamService;
        public NotificationController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            var viewModel = new NotificationsViewModel();
            var requests = _teamService.GetTeamsRequests(Contracts.Session.Session.Current.Id);

            foreach (var request in requests)
            {
                viewModel.MembershipRequests.Add(new TeamMemberRequestViewModel
                {
                    MemberId = request.Member.Id,
                    MemberName = request.Member.Name,
                    TeamId = request.Team.Id,
                    TeamName = request.Team.Name
                });
            }

            return PartialView(viewModel);
        }
    }
}
