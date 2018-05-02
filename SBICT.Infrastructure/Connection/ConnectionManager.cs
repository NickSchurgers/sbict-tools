using System.Collections.Generic;
using System.Linq;

namespace SBICT.Infrastructure.Connection
{
    public class ConnectionManager : IConnectionManager<Connection>
    {
        public Dictionary<string, Connection> Connections { get; } = new Dictionary<string, Connection>();

        public Connection Get(string name)
        {
            return Connections.ContainsKey(name) ? Connections.First(c => c.Key == name).Value : null;
        }

        public void Set(string name, Connection connection)
        {
            if (!Connections.ContainsKey(name))
            {
                Connections.Add(name, connection);
            }
        }

        public void Unset(string name)
        {
            if (Connections.ContainsKey(name))
            {
                Connections.Remove(name);
            }
        }
    }
}