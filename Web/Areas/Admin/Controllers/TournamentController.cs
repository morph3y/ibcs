using System.Web.Mvc;
using Contracts.Business;
using Entities;
using Web.Areas.Admin.Models;
using Web.Models.Dto;

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
            return View(_tournamentService.Get(id));
        }

        [HttpPost]
        public ActionResult Edit(TournamentDto model)
        {
            var loggedInUser = _objectService.Get<Player>(x => x.Id == Framework.Session.Session.Current.Id);
            if (model == null || !loggedInUser.IsAdmin)
            {
                return RedirectToAction("Index", "Tournament", new { area = "" });
            }

            var tournament = _tournamentService.Get(model.Id);
            tournament.IsRanked = model.IsRanked;
            tournament.Name = model.Name;
            tournament.PointsForTie = model.PointsForTie < 0 ? 0 : model.PointsForTie;
            tournament.PointsForWin = model.PointsForWin < 0 ? 0 : model.PointsForWin;
            tournament.Status = model.Status;
            tournament.TournamentType = model.TournamentType;
            _tournamentService.Save(tournament);

            return RedirectToAction("Detail", "Tournament", new {area = "", id = tournament.Id});
        }

        public ActionResult EditGames(int id)
        {
            return View(_objectService.Get<Tournament>(x=>x.Id == id));
        }

        public ActionResult UpdateGame(UpdateGameViewModel updateModel)
        {
            var loggedInUser = _objectService.Get<Player>(x => x.Id == Framework.Session.Session.Current.Id);
            if (updateModel == null || !loggedInUser.IsAdmin)
            {
                return RedirectToAction("Index", "Tournament", new { area="" });
            }

            var game = _objectService.Get<Game>(x => x.Id == updateModel.GameId);
            game.Status = GameStatus.Finished;
            game.Participant1Score = updateModel.Participant1Score;
            game.Participant2Score = updateModel.Participant2Score;
            game.Winner = _objectService.Get<Player>(x => x.Id == updateModel.WinnerId);
            _tournamentService.Save(game.TournamentStage.Tournament);

            return RedirectToAction("Edit", new { id = game.TournamentStage.Tournament.Id });
        }
    }
}
