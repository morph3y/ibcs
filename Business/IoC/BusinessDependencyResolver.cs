using Business.Ranking;
using Business.Tournaments;
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
            _dependencyBinder.Bind<IRankingService, RankingService>();
            _dependencyBinder.Bind<ITournamentService, TournamentService>();
            _dependencyBinder.Bind<ITeamService, TeamService>();
            _dependencyBinder.Bind<IPlayerService, PlayerService>();
            _dependencyBinder.Bind<IGameService, GameService>();
            _dependencyBinder.Bind<ISubjectService, SubjectService>();
        }
    }
}
