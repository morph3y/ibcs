using System;

using Entities;

namespace Business.Tournaments.StageBuilders
{
    internal sealed class StageBuilderFactory : IStageBuilderFactory
    {
        public IStageBuilder Create(Tournament tournament)
        {
            if (tournament.Parent != null)
            {
                if (tournament.Parent.TournamentType == TournamentType.Group)
                {
                    return new GroupToSingleElimintaionStageBuilder(tournament);
                }
            }

            if (tournament.TournamentType == TournamentType.SingleElimination)
            {
                return new SingleEliminationStageBuilder(tournament);
            }
            if (tournament.TournamentType == TournamentType.League)
            {
                return new LeagueStageBuilder(tournament);
            }
            if (tournament.TournamentType == TournamentType.Group)
            {
                return new GroupStageBuilder(tournament);
            }
            throw new Exception("Tournament type is not supported!");
        }
    }
}