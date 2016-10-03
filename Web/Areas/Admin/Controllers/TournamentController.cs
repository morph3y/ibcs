using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Contracts.Business;
using Entities;
using Web.Areas.Admin.Models;
using Web.Models.TournamentModels;
using WebGrease.Css.Extensions;

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
                IsVisible = trn.IsVisible,
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
                    IsVisible = model.IsVisible,
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
                tournamentToSave.IsRanked = model.IsRanked;
                tournamentToSave.IsVisible = model.IsVisible;
                tournamentToSave.IsTeamEvent = model.IsTeamEvent;
                if (tournamentToSave.Contestants.Count == 0)
                {
                    tournamentToSave.TournamentType = model.TournamentType;
                }
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
        /*
        public ActionResult ResetRank(int id)
        {
            _tournamentService.ResetRanks(_tournamentService.Get(id));

            return RedirectToAction("Edit", new { id = id, area = "Admin" });
        }*/

        public ActionResult EditGroups(int id)
        {
            return View("EditGroups", _tournamentService.Get(id));
        }

        public ActionResult AddNewGroup(string groupName, int tournamentId)
        {
            var tournament = _tournamentService.Get(tournamentId);
            if (tournament != null)
            {
                if (tournament.Stages.Count == 0)
                {
                    tournament.Stages.Add(new TournamentStage
                    {
                        Name = "GroupStage",
                        Tournament = tournament,
                        Order = 0,
                    });
                }

                if (tournament.Stages.Count == 1)
                {
                    tournament.Stages[0].Groups.Add(new TournamentGroup
                    {
                        Name = groupName,
                        Stage = tournament.Stages[0]
                    });
                    _tournamentService.Save(tournament);
                    return RedirectToAction("EditGroups", new { id = tournamentId });
                }
                throw new Exception("More than 1 stage");
            }
            throw new Exception("Tournament not found");
        }

        public ActionResult AddGroupContestant(int tournamentId, int groupId, int contestantId)
        {
            var tournament = _tournamentService.Get(tournamentId);
            if (tournament.Stages.Count > 0)
            {
                var groupToAdd = tournament.Stages[0].Groups.FirstOrDefault(x => x.Id == groupId);
                if (groupToAdd != null)
                {
                    var contenstant = tournament.Contestants.FirstOrDefault(x => x.Id == contestantId);
                    if (contenstant != null)
                    {
                        groupToAdd.Contestants.Add(contenstant);
                        _tournamentService.AddContestant(contenstant.Id, tournament);
                        _tournamentService.Save(tournament);
                        return RedirectToAction("EditGroups", new { id = tournamentId });
                    }
                    throw new Exception("Contestant is not in the tournament");
                }
                throw new Exception("Group not found");
            }
            throw new Exception("Has to have 1 stage");
        }

        public ActionResult RemoveGroupContestant(int tournamentId, int groupId, int contestantId)
        {   
            var tournament = _tournamentService.Get(tournamentId);
            if (tournament.Stages.Count > 0)
            {
                var groupToAdd = tournament.Stages[0].Groups.FirstOrDefault(x => x.Id == groupId);
                if (groupToAdd != null)
                {
                    var contenstant = groupToAdd.Contestants.FirstOrDefault(x => x.Id == contestantId);
                    if (contenstant != null)
                    {
                        groupToAdd.Contestants.RemoveAt(groupToAdd.Contestants.IndexOf(contenstant));
                        _tournamentService.RemoveContestant(contenstant.Id, tournament);
                        _tournamentService.Save(tournament);
                        return RedirectToAction("EditGroups", new { id = tournamentId });
                    }
                    throw new Exception("Contestant is not in the group");
                }
                throw new Exception("Group not found");
            }
            throw new Exception("Has to have 1 stage");
        }

        public ActionResult RemoveGroup(int tournamentId, int groupId)
        {
            var tournament = _tournamentService.Get(tournamentId);
            if (tournament.Stages.Count > 0)
            {
                var groupToRemove = tournament.Stages[0].Groups.FirstOrDefault(x => x.Id == groupId);
                if (groupToRemove != null)
                {
                    groupToRemove.Contestants.ForEach(tournament.Contestants.Add);
                    tournament.Stages[0].Groups.RemoveAt(tournament.Stages[0].Groups.IndexOf(groupToRemove));
                    _tournamentService.Save(tournament);
                    return RedirectToAction("EditGroups", new { id = tournamentId });
                }
                throw new Exception("Group not found");
            }
            throw new Exception("Has to have 1 stage");
        }
    }
}
