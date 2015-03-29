using Business.IoC;
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
            var businessResolver = new BusinessDependencyResolver(new DependencyBinder(_kernel));

            businessResolver.Resolve();
        }
    }
}
