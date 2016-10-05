using System;
using System.Collections.Generic;
using System.Linq;

using Entities;

namespace Business.Tournaments.StageBuilders
{
    internal sealed class GroupToSingleElimintaionStageBuilder : EliminationBuilderBase
    {
        public GroupToSingleElimintaionStageBuilder(Tournament tournament)
            : base(tournament)
        { }

        public override void Build()
        {
            if (Tournament.Contestants.Count < 2)
            {
                return;
            }

            var contestants = Tournament.Contestants.ToList();
            var numberOfGames = GetNumberOfGamesToStart(contestants);
            var groups = Tournament.Parent.Stages[0].Groups;
            var games = new List<Game>();

            var qualifiedPerGroup = groups[0].QualifiedContestants.Count;
            if (((qualifiedPerGroup * groups.Count) / 2) != numberOfGames)
            {
                throw new Exception("Misaligned tournament");
            }

            Tournament.Stages.Clear();
            Tournament.Stages.Add(new TournamentStage
            {
                Name = "1/" + numberOfGames,
                Order = 0,
                Tournament = Tournament
            });

            for (var i = 0; i < groups.Count; i++)
            {
                // Process only half of all groups (pairs)
                if (i % 2 != 0)
                {
                    continue;
                }

                var group1 = groups[i];
                var group2 = groups[i + 1];

                var contestants1 = group1.QualifiedContestants.OrderBy(x => x.Order).Select(x => x.Contestant).ToList();
                var contestants2 = group2.QualifiedContestants.OrderBy(x => x.Order).Select(x => x.Contestant).Reverse().ToList();

                for (var j = 0; j < qualifiedPerGroup; j++)
                {
                    games.Add(CreateGameBetween(contestants1[j], contestants2[j], Tournament.Stages[0], games.Count - 1));
                }
            }
            games.ForEach(x => Tournament.Stages[0].Games.Add(x));
        }

        public override void Update()
        {
            UpdateStages(Tournament.Stages, Tournament.Contestants.Count);
        }
    }
}