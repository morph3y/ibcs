namespace Contracts.IoC
{
    public interface IDependencyBinder
    {
        void Bind<TInterface, TConcrete>() where TConcrete : TInterface;
        void BindSingleton<TInterface, TConcrete>() where TConcrete : TInterface;
    }
}
