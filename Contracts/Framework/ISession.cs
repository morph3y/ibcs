using Entities;

namespace Contracts.Framework
{
    public interface ISession
    {
        bool IsNullSession { get; set; }
        Player Player { get; set; }
        bool IsAdmin();
    }
}
