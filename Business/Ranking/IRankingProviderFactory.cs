using Entities;

namespace Business.Ranking
{
    internal interface IRankingProviderFactory
    {
        IRankingProvider GetProvider(Tournament tournament);
    }
}
