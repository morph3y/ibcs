using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Entities;

namespace Business.Tournaments.StageBuilders
{
    internal sealed class GroupStageBuilder : LeagueStageBuilder
    {
        private readonly Tournament _tournament;

        public GroupStageBuilder(Tournament tournament) 
            : base(tournament)
        {
            _tournament = tournament;
        }

        public override void Build()
        {
            if (_tournament.Stages.Count < 1)
            {
                _tournament.Stages.Add(new TournamentStage
                {
                    Games = new List<Game>(), 
                    Groups = new List<TournamentGroup>(),
                    Name = "Group Stage",
                    Tournament = _tournament
                });
            }

            foreach (var group in _tournament.Stages[0].Groups)
            {
                var games = CreateGames(group.Contestants).ToList();
                group.Games.Clear();
                games.ForEach(x =>
                {
                    x.Group = group;
                    group.Games.Add(x);
                    x.TournamentStage = _tournament.Stages[0];
                });
            }
        }

        public override void Update()
        { }
    }
}
