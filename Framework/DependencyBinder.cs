using Contracts.IoC;
using Ninject;

namespace Framework
{
    internal sealed class DependencyBinder : IDependencyBinder
    {
        private readonly IKernel _kernel;

        public DependencyBinder(IKernel kernel)
        {
            _kernel = kernel;
        }

        public void Bind<TInterface, TConcrete>() where TConcrete : TInterface
        {
            _kernel.Bind<TInterface>().To<TConcrete>();
        }

        public void BindSingleton<TInterface, TConcrete>() where TConcrete : TInterface
        {
            _kernel.Bind<TInterface>().To<TConcrete>().InSingletonScope();
        }
    }
}
