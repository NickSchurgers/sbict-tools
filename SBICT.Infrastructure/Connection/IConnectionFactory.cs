namespace SBICT.Infrastructure.Connection
{
    public interface IConnectionFactory
    {
        IConnection Create(string url, string hubName);
    }
}