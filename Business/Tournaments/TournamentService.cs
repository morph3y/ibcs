using System;
using System.Collections.Generic;
using System.Linq;
using Contracts.Business;
using Contracts.Business.Dal;
using Contracts.Business.Tournaments;
using Contracts.Session;
using Entities;

namespace Business.Tournaments
{
    internal sealed class TournamentService : ITournamentService
    {
        private readonly ITournamentDataAdapter _tournamentDataAdapter;
        private readonly ITournamentStageService _tournamentStageService;
        private readonly ISubjectService _subjectService;
        private readonly IRankingService _rankingService;
        public TournamentService(ITournamentDataAdapter tournamentDataAdapter,
            ITournamentStageService tournamentStageService,
            ISubjectService subjectService,
            IRankingService rankingService)
        {
            _tournamentDataAdapter = tournamentDataAdapter;
            _tournamentStageService = tournamentStageService;
            _subjectService = subjectService;
            _rankingService = rankingService;
        }

        public IEnumerable<Tournament> GetList()
        {
            return _tournamentDataAdapter.GetCollection(x => x.Status != TournamentStatus.Closed);
        }

        public Tournament Get(int id)
        {
            return _tournamentDataAdapter.Get(x => x.Id == id);
        }

        public bool IsInTournament(int tournamentId, int memberId)
        {
            return _tournamentDataAdapter.IsInTournament(memberId, tournamentId);
        }

        public void Save(Tournament tournament)
        {
            tournament.PointsForTie = tournament.PointsForTie < 0 ? 0 : tournament.PointsForTie;
            tournament.PointsForWin = tournament.PointsForWin < 0 ? 0 : tournament.PointsForWin;

            _tournamentDataAdapter.Save(tournament);
        }

        public void Create(Tournament tournament)
        {
            tournament.Status = TournamentStatus.Registration;
            _tournamentStageService.GenerateStages(tournament);
        }

        public void AddContestant(int contestantId, Tournament tournament)
        {
            if (tournament.Status != TournamentStatus.Registration)
            {
                throw new Exception("Tournament is in progress");
            }

            var contestant = _subjectService.Get(x => x.Id == contestantId);
            ValidateContestant(contestant, tournament);

            tournament.Contestants.Add(contestant);
            _tournamentStageService.GenerateStages(tournament);
        }

        public void RemoveContestant(int contestantId, Tournament tournament)
        {
            var contestant = _subjectService.Get(x => x.Id == contestantId);
            ValidateContestant(contestant, tournament);

            _tournamentStageService.RemoveContestant(contestant, tournament);
        }

        public void ResetRanks(Tournament tournament)
        {
            var stages = tournament.Stages.OrderBy(x => x.Order);

            foreach (var tournamentStage in stages)
            {
                var games = tournamentStage.Games.Where(x => x.Status == GameStatus.Finished && x.Winner != null && x.Participant1 != null && x.Participant2 != null);
                foreach (var game in games)
                {
                    _rankingService.UpdateRank(game.Winner, game.Winner.Id == game.Participant1.Id ? game.Participant2 : game.Participant1);
                }
            }
        }

        private void ValidateContestant(Subject contestant, Tournament tournament)
        {
            var playerContestant = contestant as Player;
            var teamContestant = contestant as Team;

            if ((tournament.IsTeamEvent && playerContestant != null) || (!tournament.IsTeamEvent && teamContestant != null))
            {
                throw new Exception("This contestant doesn't fit into the tournament");
            }

            if (!Session.Current.IsAdmin && (
                (tournament.IsTeamEvent && teamContestant.Captain.Id != Session.Current.Id)
                    ||
                (!tournament.IsTeamEvent && playerContestant.Id != Session.Current.Id)))
            {
                throw new Exception("You don't have rights to change this contestant");
            }
        }
    }
}
