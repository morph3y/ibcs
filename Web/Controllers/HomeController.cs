using System.Web.Mvc;
using Contracts.Business;
using Entities;

namespace Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IObjectService _objectService;

        public HomeController(IObjectService objectService)
        {
            _objectService = objectService;
        }

        public ActionResult Index()
        {
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
