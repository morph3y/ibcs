using System.Linq;
using System.Web.Mvc;
using Contracts.Business;
using Entities;
using Web.Models;

namespace Web.Controllers
{
    [AllowAnonymous]
    public class RankingController : Controller
    {
        private readonly IRankingService _rankingService;
        public RankingController(IRankingService rankingService)
        {
            _rankingService = rankingService;
        }

        public ActionResult List()
        {
            return View("RankingInventory");
        }

        public ActionResult Top10Players()
        {
            return PartialView("Top10", new RankingViewModel
            {
                Ranks = _rankingService.Get<Player>(10).ToList(),
                SubjectType = typeof(Player)
            });
        }

        public ActionResult AllPlayerRanks()
        {
            return View("ShowAllRanks", new RankingViewModel
            {
                Ranks = _rankingService.Get<Player>(10).ToList(),
                SubjectType = typeof(Player)
            });
        }

        public ActionResult Top10Teams()
        {
            return PartialView("Top10", new RankingViewModel
            {
                Ranks = _rankingService.Get<Team>(10).ToList(),
                SubjectType = typeof(Team)
            });
        }

        public ActionResult AllTeamRanks()
        {
            return View("ShowAllRanks", new RankingViewModel
            {
                Ranks = _rankingService.Get<Team>().ToList(),
                SubjectType = typeof(Team)
            });
        }
    }
}