using System.Collections.Generic;

namespace SBICT.Infrastructure.Connection
{
    public interface IConnectionManager<T> where T : IConnection
    {
        Dictionary<string, IConnection> Connections { get; }
        T Get(string name);
        void Set(string name, T connection);
        void Unset(string name);
    }
}