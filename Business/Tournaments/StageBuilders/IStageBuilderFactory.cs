using Entities;

namespace Business.Tournaments.StageBuilders
{
    internal interface IStageBuilderFactory
    {
        IStageBuilder Create(Tournament tournament);
    }
}
