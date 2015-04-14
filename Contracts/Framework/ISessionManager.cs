using Entities;

namespace Contracts.Framework
{
    public interface ISessionManager
    {
        void CreateOrValidate(Player player);
        void Destroy();
    }
}
