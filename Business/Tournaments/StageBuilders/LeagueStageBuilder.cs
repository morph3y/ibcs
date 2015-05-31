using System.Collections.Generic;
using Entities;

namespace Business.Tournaments.StageBuilders
{
    internal sealed class LeagueStageBuilder : IStageBuilder
    {
        private readonly Tournament _tournament;
        public LeagueStageBuilder(Tournament tournament)
        {
            _tournament = tournament;
        }

        public void Build()
        {
            if (_tournament.Contestants.Count < 2)
            {
                _tournament.Stages.Clear();
            }

            var games = new List<Game>();
            foreach (var contestant1 in _tournament.Contestants)
            {
                foreach (var contestant2 in _tournament.Contestants)
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
                        Tournament = _tournament
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
            _tournament.Stages.Clear();
            foreach (var tournamentStage in stages)
            {
                _tournament.Stages.Add(tournamentStage);
            }
        }
    }
}