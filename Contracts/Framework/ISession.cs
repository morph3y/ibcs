using System;

namespace Contracts.Framework
{
    public interface ISession
    {
        bool IsLoggedIn { get; set; }
        bool IsAdmin();
    }
}
