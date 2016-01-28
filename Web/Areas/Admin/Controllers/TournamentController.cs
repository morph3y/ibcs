using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Contracts.Business;
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
        private readonly ITeamService _teamService;
        public TournamentController(
            ITournamentService tournamentService,
            IPlayerService playerService,
            IGameService gameService,
            ISubjectService subjectService,
            ITeamService teamService)
        {
            _tournamentService = tournamentService;
            _playerService = playerService;
            _gameService = gameService;
            _subjectService = subjectService;
            _teamService = teamService;
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

            var oldTournament = _tournamentService.Get(model.Id);
            var tournamentToSave = oldTournament;
            if (oldTournament == null)
            {
                tournamentToSave = new Tournament
                {
                    IsRanked = model.IsRanked,
                    Name = model.Name,
                    PointsForTie = model.PointsForTie,
                    PointsForWin = model.PointsForWin,
                    Status = model.Status,
                    TournamentType = model.TournamentType,
                    IsTeamEvent = model.IsTeamEvent
                };
            }
            else
            {
                tournamentToSave.Name = model.Name;
                tournamentToSave.PointsForTie = model.PointsForTie;
                tournamentToSave.PointsForWin = model.PointsForWin;
                tournamentToSave.Status = model.Status;
            }

            _tournamentService.Save(tournamentToSave);

            return RedirectToAction("Edit", "Tournament", new { area = "Admin", id = tournamentToSave.Id });
        }

        [HttpPost]
        public ActionResult Convert(TournamentType targetType, int id, int playerLimit)
        {
            var tournamentToConvert = _tournamentService.Get(id);
            if (tournamentToConvert == null)
            {
                throw new Exception("Tournament does not exist");
            }

            var newTournament = _tournamentService.Convert(tournamentToConvert, targetType, playerLimit);

            return RedirectToAction("Edit", "Tournament", new { area = "Admin", id = newTournament.Id });
        }

        public ActionResult Contestants(int id)
        {
            var tournament = _tournamentService.Get(id);

            var otherContestants = new List<Subject>();
            if (tournament.IsTeamEvent)
            {
                var teams = _teamService.GetList().Where(x => !tournament.Contestants.Contains(x)).ToList();
                otherContestants.AddRange(teams);
            }
            else
            {
                var players = _playerService.GetList().Where(x => !tournament.Contestants.Contains(x)).ToList();
                otherContestants.AddRange(players);
            }
            return View(new ContestantsListModel
            {
                Contestants = tournament.Contestants.ToList(),
                OtherSubjects = otherContestants,
                TournamentId = id
            });
        }

        public ActionResult RemoveContestant(int id, int tournamentId)
        {
            var tournament = _tournamentService.Get(tournamentId);
            _tournamentService.RemoveContestant(id, tournament);
            return RedirectToAction("Contestants", new { id = tournamentId });
        }

        public ActionResult AddContestant(int id, int tournamentId)
        {
            var tournament = _tournamentService.Get(tournamentId);
            _tournamentService.AddContestant(id, tournament);
            return RedirectToAction("Contestants", new { id = tournamentId });

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
            game.Participant1Score = updateModel.Participant1Score;
            game.Participant2Score = updateModel.Participant2Score;
            game.Winner = _subjectService.Get(x => x.Id == updateModel.WinnerId);
            _gameService.EndGame(game);

            return RedirectToAction("Edit", new { id = game.TournamentStage.Tournament.Id, area = "Admin" });
        }

        public ActionResult ResetRank(int id)
        {
            _tournamentService.ResetRanks(_tournamentService.Get(id));

            return RedirectToAction("Edit", new { id = id, area = "Admin" });
        }
    }
}
