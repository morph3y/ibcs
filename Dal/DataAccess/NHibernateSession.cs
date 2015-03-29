using System;
using Contracts.Dal;

namespace Dal.DataAccess
{
    internal sealed class NHibernateSession : IDatabaseSession
    {
        public NHibernateSession()
        {
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IDatabaseTransaction BeginTransaction()
        {
            throw new NotImplementedException();
        }
    }
}
