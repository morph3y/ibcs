using System.Collections.Generic;
using System.Linq;
using Contracts.Business;
using Entities;

namespace Business
{
    internal sealed class TournamentService : ITournamentService
    {
        private readonly IObjectService _objectService;

        public TournamentService(IObjectService objectService)
        {
            _objectService = objectService;
        }

        public IEnumerable<Tournament> GetList()
        {
            return _objectService.Get<Tournament>(x => x.Status != TournamentStatus.Closed);
        }

        public Tournament Get(int id)
        {
            return _objectService.Get<Tournament>(x => x.Id == id).FirstOrDefault();
        }

        public void Save(Tournament tournament)
        {
            _objectService.Save(tournament);
        }

        public void Create(Tournament entity)
        {
            entity.Status = TournamentStatus.Registration;
            if (entity.TournamentType == TournamentType.League)
            {
                GenerateGames(entity);
            }
        }

        public void AddContestant(Subject contestant, Tournament tournament)
        {
            tournament.Contestants.Add(contestant);
            GenerateGames(tournament);
        }

        private void GenerateGames(Tournament tournament)
        {
            if (tournament.Status != TournamentStatus.Registration || tournament.Contestants.Count < 2)
            {
                return;
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

                    if (!stageToSubject[stage].Contains(game.Participant1) && !stageToSubject[stage].Contains(game.Participant2))
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
                        Name = "Stage " + stages.Count,
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

            tournament.Stages = stages;
        }
    }
}
