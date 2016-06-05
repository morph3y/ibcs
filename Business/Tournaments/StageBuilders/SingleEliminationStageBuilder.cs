using System.Linq;
using Entities;

namespace Business.Tournaments.StageBuilders
{
    internal sealed class SingleEliminationStageBuilder : EliminationBuilderBase
    {
        public SingleEliminationStageBuilder(Tournament tournament)
            :base(tournament)
        { }

        public override void Build()
        {
            if (Tournament.Contestants.Count < 2)
            {
                return;
            }

            var contestants = Tournament.Contestants.ToList();
            var numberOfGames = GetNumberOfGamesToStart(contestants);

            Tournament.Stages.Clear();
            Tournament.Stages.Add(new TournamentStage
            {
                Name = "1/" + numberOfGames,
                Order = 0,
                Tournament = Tournament
            });

            var listOfGames = CreateStageGames(contestants, Tournament.Stages[0], Tournament.IsTeamEvent);
            listOfGames.ForEach(x => Tournament.Stages[0].Games.Add(x));

            // Trigger update in case stage 2 is needed
            var numberOfByes = (numberOfGames * 2) - contestants.Count;
            if (numberOfByes > 1)
            {
                Update();
                if (Tournament.Stages.Count > 1)
                {
                    Tournament.Stages[1].Games = Tournament.Stages[1].Games.OrderBy(x => x.Order).ToList();
                }
            }
            Tournament.Stages[0].Games = Tournament.Stages[0].Games.OrderBy(x => x.Order).ToList();
        }

        public override void Update()
        {
            UpdateStages(Tournament.Stages, Tournament.Contestants.Count);
        }
    }
}