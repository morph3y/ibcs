namespace Contracts.Dal
{
    public interface IDataAccessAdapter
    {
        IDatabaseSession GetSession();
    }
}
