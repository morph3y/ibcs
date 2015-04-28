using System.Web.Mvc;
using Contracts.Business;
using Entities;
using Web.Areas.Admin.Models;

namespace Web.Areas.Admin.Controllers
{
    [Authorize]
    public class TournamentController : Controller
    {
        private readonly IObjectService _objectService;
        private readonly ITournamentService _tournamentService;
        public TournamentController(IObjectService objectService, ITournamentService tournamentService)
        {
            _objectService = objectService;
            _tournamentService = tournamentService;
        }

        public ActionResult Edit(int id)
        {
            return View(_objectService.GetFirst<Tournament>(x => x.Id == id));
        }

        public ActionResult UpdateGame(UpdateGameViewModel updateModel)
        {
            var loggedInUser = _objectService.GetFirst<Player>(x => x.Id == Framework.Session.Session.Current.Id);
            if (updateModel == null || !loggedInUser.IsAdmin)
            {
                return RedirectToAction("Index", "Tournament", new { area="" });
            }

            var game = _objectService.GetFirst<Game>(x => x.Id == updateModel.GameId);
            game.Status = GameStatus.Finished;
            game.Participant1Score = updateModel.Participant1Score;
            game.Participant2Score = updateModel.Participant2Score;
            game.Winner = _objectService.GetFirst<Player>(x => x.Id == updateModel.WinnerId);
            _tournamentService.Save(game.TournamentStage.Tournament);

            return RedirectToAction("Edit", new { id = game.TournamentStage.Tournament.Id });
        }
    }
}
