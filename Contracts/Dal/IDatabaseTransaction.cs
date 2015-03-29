using System;

namespace Contracts.Dal
{
    public interface IDatabaseTransaction : IDisposable
    {
        void Save(object entity, Type entityType);
    }
}
