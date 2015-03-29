using System;

namespace Contracts.Dal
{
    public interface IDatabaseSession : IDisposable
    {
        IDatabaseTransaction BeginTransaction();
    }
}
