using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Contracts.Business;
using Entities;

namespace Web.Controllers
{
    public class TournamentController : Controller
    {
        private readonly IObjectService _objectService;
        public TournamentController(IObjectService objectService)
        {
            _objectService = objectService;
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            return View(_objectService.Get<Tournament>(x => x.Status == TournamentStatus.Active));
        }

        public void a()
        {
            _objectService.Save(new Tournament()
            {
                Name = "IBCS 2015",
                Stages = new List<TournamentStage>()
                {
                    new TournamentStage()
                    {
                        Name = "Stage1",
                        Order = 1
                    }
                },
                Status = TournamentStatus.Registration,
                TournamentType = TournamentType.League
            });
            var tournamentStage = _objectService.GetFirst<TournamentStage>(x => x.Id == 1);
            var player = new Player
            {
                FirstName = "alex",
                LastName = "bla",
                Name = "bla bla"
            };
            _objectService.Save(player);
            tournamentStage.Games.Add(new Game
            {
                Participant1 = player,
                Participant2 = player,
                Status = GameStatus.NotStarted,
                TournamentStage = tournamentStage,
                Winner = null
            });


            _objectService.Save(tournamentStage);
            var i = 0;
        }
    }
}
