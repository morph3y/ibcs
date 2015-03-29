using Contracts.Dal;

namespace Dal.DataAccess
{
    internal sealed class DataAccessAdapter : IDataAccessAdapter
    {
        public IDatabaseSession GetSession()
        {
            throw new System.NotImplementedException();
        }
    }
}
