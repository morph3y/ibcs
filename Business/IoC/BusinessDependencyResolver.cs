using Contracts.Business;
using Contracts.IoC;

namespace Business.IoC
{
    public class BusinessDependencyResolver : IDependencyResolver
    {
        private readonly IDependencyBinder _dependencyBinder;

        public BusinessDependencyResolver(IDependencyBinder dependencyBinder)
        {
            _dependencyBinder = dependencyBinder;
        }

        public void Resolve()
        {
            _dependencyBinder.Bind<IObjectService, ObjectService>();
            _dependencyBinder.Bind<ITournamentService, TournamentService>();
            _dependencyBinder.Bind<ITeamService, TeamService>();
        }
    }
}
