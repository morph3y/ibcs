using System.Web.Mvc;
using Contracts.Business;
using Entities;

namespace Web.Controllers
{
    public class TeamController : Controller
    {
        private readonly IObjectService _objectService;
        public TeamController(IObjectService objectService)
        {
            _objectService = objectService;
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            return View(_objectService.GetCollection<Team>());
        }

        public ActionResult Edit(int id)
        {
            return View(_objectService.Get<Team>(x=>x.Id == id));
        }
    }
}
