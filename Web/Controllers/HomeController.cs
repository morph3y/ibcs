using System.Web.Mvc;
using Contracts.Business;
using Entities;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IObjectService _objectService;

        public HomeController(IObjectService objectService)
        {
            _objectService = objectService;
        }

        public ActionResult Index()
        {
            _objectService.Save(new Player
            {
                FirstName = "bla"
            });

            var bla = _objectService.Get<Player>(x => x.FirstName == "bla");

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}
