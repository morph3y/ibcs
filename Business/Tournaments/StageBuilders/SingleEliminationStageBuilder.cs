using Entities;

namespace Business.Tournaments.StageBuilders
{
    internal sealed class SingleEliminationStageBuilder : IStageBuilder
    {
        private readonly Tournament _tournament;
        public SingleEliminationStageBuilder(Tournament tournament)
        {
            _tournament = tournament;
        }

        public void Build()
        {
            // TODO: implement single elimintaion stage generation
            return;
        }
    }
}