﻿using System;
using System.Web.Mvc;
using Contracts.Business;
using Entities;

namespace Web.Controllers
{
    [AllowAnonymous]
    public class TournamentController : Controller
    {
        private readonly ITournamentService _tournamentService;
        private readonly IObjectService _objectService;
        public TournamentController(ITournamentService tournamentService, IObjectService objectService)
        {
            _tournamentService = tournamentService;
            _objectService = objectService;
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
            return View(_tournamentService.Get(id));
        }

        public ActionResult Bracket(int id)
        {
            return View(_tournamentService.Get(id));
        }

        [Authorize]
        public ActionResult Register(int tournamentId, int contestantId)
        {
            if (Framework.Session.Session.Current.Id != contestantId)
            {
                throw new Exception("Not you =)");
            }

            var tournament = _tournamentService.Get(tournamentId);
            if (tournament == null)
            {
                throw new Exception("Tournament was not found");
            }

            _tournamentService.AddContestant(_objectService.Get<Subject>(x => x.Id == contestantId), tournament);
            _tournamentService.Save(tournament);

            return RedirectToAction("Detail", new { id = tournament.Id});
        }

        [Authorize]
        public ActionResult Unregister(int tournamentId, int contestantId)
        {
            if (Framework.Session.Session.Current.Id != contestantId)
            {
                throw new Exception("Not you =)");
            }

            var tournament = _tournamentService.Get(tournamentId);
            if (tournament == null)
            {
                throw new Exception("Tournament was not found");
            }

            _tournamentService.RemoveContestant(_objectService.Get<Subject>(x => x.Id == contestantId), tournament);
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
