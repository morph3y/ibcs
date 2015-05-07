using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Contracts.Business;
using Entities;
using Web.Models;

namespace Web.Controllers
{
    [AllowAnonymous]
    public class TournamentController : Controller
    {
        private readonly ITournamentService _tournamentService;
        private readonly ITeamService _teamService;
        private readonly ISubjectService _subjectService;
        public TournamentController(ITournamentService tournamentService, ITeamService teamService, ISubjectService subjectService)
        {
            _tournamentService = tournamentService;
            _teamService = teamService;
            _subjectService = subjectService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            return PartialView(_tournamentService.GetList());
        }

        public ActionResult Detail(int id)
        {
            var tournament = _tournamentService.Get(id);
            var viewModel = new TournamentDetailViewModel
            {
                UserInTournament = User.Identity.IsAuthenticated && (tournament.Contestants.Any(x => x.Id == Contracts.Session.Session.Current.Id)
                    || _tournamentService.IsInTournament(tournament.Id, Contracts.Session.Session.Current.Id)),
                Tournament = tournament,
                MyTeams = User.Identity.IsAuthenticated && tournament.IsTeamEvent ? _teamService.GetCollection(x => x.Captain.Id == Contracts.Session.Session.Current.Id) : new List<Team>()
            };

            return View(viewModel);
        }

        public ActionResult Bracket(int id)
        {
            return View(_tournamentService.Get(id));
        }

        [Authorize]
        public ActionResult Register(int tournamentId, int contestantId)
        {
            var tournament = _tournamentService.Get(tournamentId);
            if (tournament == null)
            {
                throw new Exception("Tournament was not found");
            }

            _tournamentService.AddContestant(_subjectService.Get(x => x.Id == contestantId), tournament);
            _tournamentService.Save(tournament);

            return RedirectToAction("Detail", new { id = tournament.Id});
        }

        [Authorize]
        public ActionResult Unregister(int tournamentId, int contestantId)
        {
            var tournament = _tournamentService.Get(tournamentId);
            if (tournament == null)
            {
                throw new Exception("Tournament was not found");
            }

            _tournamentService.RemoveContestant(_subjectService.Get(x => x.Id == contestantId), tournament);
            _tournamentService.Save(tournament);

            return RedirectToAction("Detail", new { id = tournament.Id });
        }

        public void cr()
        {
            var trn = new Tournament
            {
                Name = "IBCS 2015",
                Status = TournamentStatus.Registration,
                TournamentType = TournamentType.League
            };

            _tournamentService.Create(trn);
            _tournamentService.Save(trn);
        }
    }
}
