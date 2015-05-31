using System.Collections.Generic;
using System.Web.Mvc;
using Contracts.Business;
using Contracts.Business.Tournaments;
using Entities;
using Web.Areas.Admin.Models;
using Web.Models.TournamentModels;

namespace Web.Areas.Admin.Controllers
{
    [Authorize]
    public class TournamentController : Controller
    {
        private readonly ITournamentService _tournamentService;
        private readonly IPlayerService _playerService;
        private readonly IGameService _gameService;
        private readonly ISubjectService _subjectService;
        public TournamentController(
            ITournamentService tournamentService,
            IPlayerService playerService,
            IGameService gameService,
            ISubjectService subjectService)
        {
            _tournamentService = tournamentService;
            _playerService = playerService;
            _gameService = gameService;
            _subjectService = subjectService;
        }

        public ActionResult Edit(int id)
        {
            var trn = _tournamentService.Get(id);
            return View(new TournamentBracketViewModel
            {
                Id = trn.Id,
                IsRanked = trn.IsRanked,
                IsTeamEvent = trn.IsTeamEvent,
                Name = trn.Name,
                PointsForTie = trn.PointsForTie,
                PointsForWin = trn.PointsForWin,
                Status = trn.Status,
                TournamentType = trn.TournamentType,
                Stages = new List<TournamentStageViewModel>()
            });
        }

        [HttpPost]
        public ActionResult Edit(TournamentBracketViewModel model)
        {
            if (model == null || !Contracts.Session.Session.Current.IsAdmin)
            {
                return RedirectToAction("Index", "Tournament", new { area = "" });
            }

            var tournament = new Tournament
            {
                Id = model.Id,
                IsRanked = model.IsRanked,
                Name = model.Name,
                PointsForTie = model.PointsForTie,
                PointsForWin = model.PointsForWin,
                Status = model.Status,
                TournamentType = model.TournamentType,
                IsTeamEvent = model.IsTeamEvent
            };
            _tournamentService.Save(tournament);

            return RedirectToAction("Edit", "Tournament", new { area = "Admin", id = tournament.Id });
        }

        public ActionResult EditGames(int id)
        {
            return View(_tournamentService.Get(id));
        }

        public ActionResult UpdateGame(UpdateGameViewModel updateModel)
        {
            var loggedInUser = _playerService.Get(x => x.Id == Contracts.Session.Session.Current.Id);
            if (updateModel == null || !loggedInUser.IsAdmin)
            {
                return RedirectToAction("Index", "Tournament", new { area = "" });
            }

            var game = _gameService.Get(x => x.Id == updateModel.GameId);
            game.Status = GameStatus.Finished;
            game.Participant1Score = updateModel.Participant1Score;
            game.Participant2Score = updateModel.Participant2Score;
            game.Winner = _subjectService.Get(x => x.Id == updateModel.WinnerId);
            _tournamentService.Save(game.TournamentStage.Tournament);

            return RedirectToAction("Edit", new { id = game.TournamentStage.Tournament.Id });
        }
    }
}
