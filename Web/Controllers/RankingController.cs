using System.Linq;
using System.Web.Mvc;
using Contracts.Business;

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

        public ActionResult Top10()
        {
            return PartialView("Top10", _rankingService.Get(10).ToList());
        }

        public ActionResult AllRanks()
        {
            return View("ShowAllRanks", _rankingService.Get().ToList());
        }
    }
}