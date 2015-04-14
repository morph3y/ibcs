using System.Web.Mvc;
using Contracts.Business;

namespace Web.Controllers
{
    [AllowAnonymous]
    public class TournamentController : Controller
    {
        private readonly ITournamentService _tournamentService;
        public TournamentController(ITournamentService tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            return View(_tournamentService.GetList());
        }

        public ActionResult Detail(int id)
        {
            return View(_tournamentService.Get(id));
        }
    }
}
