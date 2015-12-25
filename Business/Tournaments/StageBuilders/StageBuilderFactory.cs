using System;
using System.Collections.Generic;
using Entities;

namespace Business.Tournaments.StageBuilders
{
    internal sealed class StageBuilderFactory : IStageBuilderFactory
    {
        private readonly IDictionary<TournamentType, Func<Tournament, IStageBuilder>> _map = new Dictionary<TournamentType, Func<Tournament, IStageBuilder>>
        {
            { TournamentType.League, trn => new LeagueStageBuilder(trn) },
            { TournamentType.SingleElimination, trn => new SingleEliminationStageBuilder(trn) }
        }; 

        public IStageBuilder Create(Tournament tournament)
        {
            Func<Tournament, IStageBuilder> toReturn;
            if (_map.TryGetValue(tournament.TournamentType, out toReturn))
            {
                return toReturn(tournament);
            }
            throw new Exception("Tournament type is unknown");
        }
    }
}