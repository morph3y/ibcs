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
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            _objectService.Save(new Player
            {
                FirstName = "bla"
            });

            var bla = _objectService.Get<Player>(x => x.FirstName == "bla");

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
