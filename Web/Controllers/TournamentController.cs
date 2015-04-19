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
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            return View(_tournamentService.GetList());
        }

        public ActionResult Detail(int id)
        {
            return View(_tournamentService.Get(id));
        }

        public ActionResult Register(int tournamentId, int contestantId)
        {
            var tournament = _tournamentService.Get(tournamentId);
            if (tournament == null)
            {
                throw new Exception("Tournament was not found");
            }

            _tournamentService.AddContestant(_objectService.GetFirst<Subject>(x => x.Id == contestantId), tournament);
            _tournamentService.Save(tournament);

            return RedirectToAction("Detail", new { id = tournament.Id});
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

            _tournamentService.AddContestant(new Player
            {
                FirstName = "a",
                LastName = "b",
                UserName = "a",
                Passsword = "a"
            }, trn);

            _tournamentService.AddContestant(new Player
            {
                FirstName = "c",
                LastName = "d",
                UserName = "b",
                Passsword = "a"
            }, trn);

            _tournamentService.AddContestant(new Player
            {
                FirstName = "e",
                LastName = "f",
                UserName = "c",
                Passsword = "a"
            }, trn);

            _tournamentService.Save(trn);
        }
    }
}
