using System;
using System.Collections.Generic;
using System.Linq;
using Business.Ranking;
using Business.Tournaments.StageBuilders;
using Contracts.Business;
using Contracts.Business.Dal;
using Contracts.Session;
using Entities;

namespace Business.Tournaments
{
    internal sealed class TournamentService : ITournamentService
    {
        private readonly ITournamentDataAdapter _tournamentDataAdapter;
        private readonly ISubjectService _subjectService;
        private readonly IStageBuilderFactory _stageBuilderFactory;
        private readonly IRankingProviderFactory _rankingProviderFactory;
        public TournamentService(ITournamentDataAdapter tournamentDataAdapter,
            ISubjectService subjectService,
            IRankingDataAdapter rankingDataAdapter,
            IStageBuilderFactory stageBuilderFactory = null,
            IRankingProviderFactory rankingProviderFactory = null)
        {
            _tournamentDataAdapter = tournamentDataAdapter;
            _subjectService = subjectService;
            _rankingProviderFactory = rankingProviderFactory ?? new RankingProviderFactory(rankingDataAdapter);
            _stageBuilderFactory = stageBuilderFactory ?? new StageBuilderFactory();
        }

        public IEnumerable<Tournament> GetList()
        {
            return _tournamentDataAdapter.GetCollection(x => x.Status != TournamentStatus.Closed);
        }

        public Tournament Get(int id)
        {
            return _tournamentDataAdapter.Get(x => x.Id == id);
        }

        public void Save(Tournament tournament)
        {
            tournament.PointsForTie = tournament.PointsForTie < 0 ? 0 : tournament.PointsForTie;
            tournament.PointsForWin = tournament.PointsForWin < 0 ? 0 : tournament.PointsForWin;

            _tournamentDataAdapter.Save(tournament);
        }

        public bool IsInTournament(int tournamentId, int memberId)
        {
            return _tournamentDataAdapter.IsInTournament(memberId, tournamentId);
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
            RankContestants(tournament);

            _stageBuilderFactory.Create(tournament).Build();
            _tournamentDataAdapter.Save(tournament);
        }

        public void RemoveContestant(int contestantId, Tournament tournament)
        {
            var contestant = _subjectService.Get(x => x.Id == contestantId);
            ValidateContestant(contestant, tournament);

            RemoveContestant(contestant, tournament);
        }

        public void RemoveContestant(Subject contestant, Tournament tournament)
        {
            if (tournament.Status == TournamentStatus.Closed
                // temp until we figure out BYE player 
                || tournament.Status == TournamentStatus.Active)
            {
                throw new Exception("Can not remove contestant from an active or closed tournament");
            }

            ValidateContestant(contestant, tournament);

            tournament.Contestants.Remove(contestant);
            RankContestants(tournament);

            _stageBuilderFactory.Create(tournament).Build();
            _tournamentDataAdapter.Save(tournament);
        }


        public void Create(Tournament tournament)
        {
            tournament.Status = TournamentStatus.Registration;
            RankContestants(tournament);

            _stageBuilderFactory.Create(tournament).Build();
            _tournamentDataAdapter.Save(tournament);
        }

        public Tournament Convert(Tournament source, TournamentType targetType, int playerLimit)
        {
            if (source.Status != TournamentStatus.Closed)
            {
                throw new Exception("can only convert closed tournaments");
            }

            if (targetType == TournamentType.SingleElimination && source.TournamentType == TournamentType.League) // refactor later if needed
            {
                var newTournament = new Tournament
                {
                    IsRanked = source.IsRanked,
                    IsTeamEvent = source.IsTeamEvent,
                    Name = source.Name + " (converted)",
                    Parent = source,
                    Contestants = source.Contestants.ToList(),
                    PointsForTie = source.PointsForTie,
                    PointsForWin = source.PointsForWin,
                    Stages = new List<TournamentStage>(),
                    Status = TournamentStatus.Registration,
                    TournamentType = TournamentType.SingleElimination
                };

                RankContestants(newTournament);

                newTournament.Contestants = newTournament.Contestants.Take(playerLimit).ToList();

                _stageBuilderFactory.Create(newTournament).Build();

                _tournamentDataAdapter.Save(newTournament);

                return newTournament;
            }
            
            throw new NotSupportedException("Tournament type converion not supported");
        }

        public void Update(Tournament tournament)
        {
            _stageBuilderFactory.Create(tournament).Update();

            // TODO: Remove SE and derivatives specific code from here
            if (tournament.Stages.Count > 0 && tournament.Stages.Last().Games.Count == 1 && tournament.Stages.Last().Games.First().Status == GameStatus.Finished)
            {
                tournament.Status = TournamentStatus.Closed;
            }

            _tournamentDataAdapter.Save(tournament);
        }

        // TODO: Remove after deploy v2
        /*
        public void ResetRanks(Tournament tournament)
        {
            var stages = tournament.Stages.OrderBy(x => x.Order);

            foreach (var tournamentStage in stages)
            {
                var games = tournamentStage.Games.Where(x => x.Status == GameStatus.Finished && x.Winner != null && x.Participant1 != null && x.Participant2 != null);
                foreach (var game in games)
                {
                    new RankingService(_rankingDataAdapter).UpdateRank(game.Winner, game.Winner.Id == game.Participant1.Id ? game.Participant2 : game.Participant1);
                }
            }
        }*/

        private void RankContestants(Tournament tournament)
        {
            var rankedContestants = _rankingProviderFactory.GetProvider(tournament).Rank(tournament.Contestants.ToList());
            tournament.Contestants.Clear();
            foreach (var rankedContestant in rankedContestants)
            {
                tournament.Contestants.Add(rankedContestant);
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
