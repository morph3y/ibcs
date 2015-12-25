using Contracts.Business.Dal;
using Contracts.IoC;

namespace Business.Dal.IoC
{
    public class BusinessToDalDependencyResolver : IDependencyResolver
    {
        private readonly IDependencyBinder _dependencyBinder;
        public BusinessToDalDependencyResolver(IDependencyBinder dependencyBinder)
        {
            _dependencyBinder = dependencyBinder;
        }

        public void Resolve()
        {
            _dependencyBinder.Bind<IRankingDataAdapter, RankingDataAdapter>();
            _dependencyBinder.Bind<ITournamentDataAdapter, TournamentDataAdapter>();
            _dependencyBinder.Bind<ITeamDataAdapter, TeamDataAdapter>();
            _dependencyBinder.Bind<IPlayerDataAdapter, PlayerDataAdapter>();
            _dependencyBinder.Bind<ISubjectDataAdapter, SubjectDataAdapter>();
            _dependencyBinder.Bind<IGameDataAdapter, GameDataAdapter>();
        }
    }
}
