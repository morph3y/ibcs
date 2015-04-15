namespace Contracts.Framework
{
    public interface ISession
    {
        bool IsNullSession { get; set; }
        int PlayerId { get; set; }
    }
}
