using Business.IoC;
using Contracts.Framework;
using Dal.IoC;
using Ninject;
using Ninject.Modules;

namespace Framework
{
    public class FrameworkInjectionModule : NinjectModule
    {
        private readonly IKernel _kernel;

        public FrameworkInjectionModule(IKernel kernel)
        {
            _kernel = kernel;
        }

        public override void Load()
        {
            _kernel.Bind<ISessionManager>().To<SessionManager>();

            var businessResolver = new BusinessDependencyResolver(new DependencyBinder(_kernel));
            var dalResolver = new DalDependencyResolver(new DependencyBinder(_kernel));

            businessResolver.Resolve();
            dalResolver.Resolve();
        }
    }
}
