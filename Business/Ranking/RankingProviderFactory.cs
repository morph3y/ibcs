using System;

using Contracts.Business.Dal;
using Entities;

namespace Business.Ranking
{
    internal sealed class RankingProviderFactory : IRankingProviderFactory
    {
        private readonly IRankingDataAdapter _rankingDataAdapter;
        public RankingProviderFactory(IRankingDataAdapter rankingDataAdapter)
        {
            _rankingDataAdapter = rankingDataAdapter;
        }

        public IRankingProvider GetProvider(Tournament tournament)
        {
            if (tournament.Parent != null)
            {
                if (tournament.Parent.TournamentType == TournamentType.League)
                {
                    return new LeagueParentTournamentRankingProvider(tournament);
                }
                if (tournament.Parent.TournamentType == TournamentType.Group)
                {
                    return new GroupParentTournamentRankingProvider(tournament);
                }
                throw new Exception("Parent tournament type is not rankable");
            }
            return new GlobalRankingProvider(_rankingDataAdapter);
        }
    }
}
