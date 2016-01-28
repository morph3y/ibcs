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
                return  new ParentTournamentRankingProvider(tournament);
            }
            return new GlobalRankingProvider(_rankingDataAdapter);
        }
    }
}
