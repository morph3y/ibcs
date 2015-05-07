using System;
using System.Collections.Generic;

using Contracts.Business;
using Contracts.Business.Dal;

using Entities;

namespace Business
{
    internal sealed class TournamentService : ITournamentService
    {
        private readonly ITournamentDataAdapter _tournamentDataAdapter;
        private readonly ITeamDataAdapter _teamDataAdapter;
        public TournamentService(ITournamentDataAdapter tournamentDataAdapter, ITeamDataAdapter teamDataAdapter)
        {
            _tournamentDataAdapter = tournamentDataAdapter;
            _teamDataAdapter = teamDataAdapter;
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
            var oldTournament = _tournamentDataAdapter.Get(x => x.Id == tournament.Id) ?? new Tournament();
            // TODO: Move this somewhere else
            if (tournament.TournamentType == TournamentType.SingleElimination)
            {
                // do magic
            }
            else if (tournament.TournamentType == TournamentType.League)
            {
                // nothing for now
            }

            if (oldTournament.IsTeamEvent != tournament.IsTeamEvent)
            {
                oldTournament.Contestants.Clear();
                oldTournament.Stages.Clear();
            }

            oldTournament.IsRanked = tournament.IsRanked;
            oldTournament.IsTeamEvent = tournament.IsTeamEvent;
            oldTournament.Name = tournament.Name;
            oldTournament.PointsForTie = tournament.PointsForTie;
            oldTournament.PointsForWin = tournament.PointsForWin;
            oldTournament.Status = tournament.Status;
            oldTournament.TournamentType = tournament.TournamentType;

            _tournamentDataAdapter.Save(oldTournament);
        }

        public void Create(Tournament entity)
        {
            if (entity == null || string.IsNullOrWhiteSpace(entity.Name))
            {
                throw new Exception("Tournament name is empty");
            }

            entity.Status = TournamentStatus.Registration;
            if (entity.TournamentType == TournamentType.League)
            {
                GenerateGames(entity);
            }
        }

        public void AddContestant(Subject contestant, Tournament tournament)
        {
            if (contestant == null || tournament == null)
            {
                return;
            }

            tournament.Contestants.Add(contestant);
            GenerateGames(tournament);
        }

        public void RemoveContestant(Subject contestant, Tournament tournament)
        {
            if (Contracts.Session.Session.Current == null || tournament.Status == TournamentStatus.Closed
                // temp until we figure out BYE player 
                || tournament.Status == TournamentStatus.Active)
            {
                return;
            }

            if (tournament.IsTeamEvent)
            {
                var contestantId = contestant.Id;
                contestant = _teamDataAdapter.Get(x => x.Id == contestantId);
                if (contestant == null)
                {
                    return;
                }
            }

            tournament.Contestants.Remove(contestant);
            if (tournament.Status == TournamentStatus.Registration)
            {
                GenerateGames(tournament);
                return;
            }

            // do the magic / or team
            if (tournament.Status == TournamentStatus.Active)
            {
                var bye = new Player
                {
                    Name = "BYE",
                    FirstName = "BYE",
                    LastName = "",
                    UserName = "BYE",

                };
            }
        }

        private void GenerateGames(Tournament tournament)
        {
            if (tournament.Status != TournamentStatus.Registration)
            {
                return;
            }

            if (tournament.Contestants.Count < 2)
            {
                tournament.Stages.Clear();
            }

            var games = new List<Game>();
            foreach (var contestant1 in tournament.Contestants)
            {
                foreach (var contestant2 in tournament.Contestants)
                {
                    if (contestant1.Equals(contestant2))
                    {
                        continue;
                    }

                    var game = new Game
                    {
                        Participant1 = contestant1,
                        Participant2 = contestant2,
                        Status = GameStatus.NotStarted,
                        TournamentStage = null,
                        Winner = null
                    };

                    if (!games.Contains(game))
                    {
                        games.Add(game);
                    }
                }
            }

            var stages = new List<TournamentStage>();
            var stageToSubject = new Dictionary<TournamentStage, HashSet<Subject>>();
            foreach (var game in games)
            {
                var added = false;
                foreach (var stage in stages)
                {
                    if (!stageToSubject.ContainsKey(stage))
                    {
                        stageToSubject.Add(stage, new HashSet<Subject>());
                    }

                    if (!stageToSubject[stage].Contains(game.Participant1) &&
                        !stageToSubject[stage].Contains(game.Participant2))
                    {
                        stage.Games.Add(game);
                        stageToSubject[stage].Add(game.Participant1);
                        stageToSubject[stage].Add(game.Participant2);
                        added = true;
                        break;
                    }
                }

                if (!added)
                {
                    var stageInQuestion = new TournamentStage
                    {
                        Order = stages.Count,
                        Name = "Stage " + (stages.Count + 1),
                        Tournament = tournament
                    };

                    stages.Add(stageInQuestion);
                    if (!stageToSubject.ContainsKey(stageInQuestion))
                    {
                        stageToSubject.Add(stageInQuestion, new HashSet<Subject>());
                    }

                    stageInQuestion.Games.Add(game);
                    stageToSubject[stageInQuestion].Add(game.Participant1);
                    stageToSubject[stageInQuestion].Add(game.Participant2);
                }
            }
            tournament.Stages.Clear();
            foreach (var tournamentStage in stages)
            {
                tournament.Stages.Add(tournamentStage);
            }
        }
    }
}
