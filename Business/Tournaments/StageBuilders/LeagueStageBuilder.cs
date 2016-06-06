using System.Collections.Generic;
using Entities;

namespace Business.Tournaments.StageBuilders
{
    internal class LeagueStageBuilder : LeagueBuilderBase
    {
        private readonly Tournament _tournament;
        public LeagueStageBuilder(Tournament tournament)
        {
            _tournament = tournament;
        }

        public override void Build()
        {
            if (_tournament.Contestants.Count < 2)
            {
                _tournament.Stages.Clear();
            }

            var games = CreateGames(_tournament.Contestants);
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

        public override void Update()
        {
            // League goes on until all games are done
            return;
        }
    }
}