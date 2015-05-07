using Contracts.Dal;
using Contracts.IoC;

using Dal.Contracts;
using Dal.DataAccess;

namespace Dal.IoC
{
    public class DalDependencyResolver : IDependencyResolver
    {
        private readonly IDependencyBinder _dependencyBinder;
        public DalDependencyResolver(IDependencyBinder dependencyBinder)
        {
            _dependencyBinder = dependencyBinder;
        }

        public void Resolve()
        {
            _dependencyBinder.BindSingleton<IDataAccessAdapter, DataAccessAdapter>();
            _dependencyBinder.Bind<IDbSessionManager, DbSessionManager>();
        }
    }
}
