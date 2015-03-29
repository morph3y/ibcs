using System;

namespace Contracts.Business
{
    public interface IObjectService
    {
        void Save(object entity, Type entityType);
    }
}
