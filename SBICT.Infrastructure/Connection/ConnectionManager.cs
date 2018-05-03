using System.Collections.Generic;
using System.Linq;

namespace SBICT.Infrastructure.Connection
{
    /// <summary>
    /// Class to store the active connections and retrieve them as needed
    /// </summary>
    public class ConnectionManager : IConnectionManager<Connection>
    {
        #region Properties

        public Dictionary<string, Connection> Connections { get; } = new Dictionary<string, Connection>();

        #endregion

        #region Methods

        /// <summary>
        /// Retrieve a connection
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Connection Get(string name)
        {
            return Connections.ContainsKey(name) ? Connections.First(c => c.Key == name).Value : null;
        }

        /// <summary>
        /// Add a connection to store
        /// </summary>
        /// <param name="name"></param>
        /// <param name="connection"></param>
        public void Set(string name, Connection connection)
        {
            if (!Connections.ContainsKey(name))
            {
                Connections.Add(name, connection);
            }
        }

        /// <summary>
        /// Remove a connection from the store
        /// </summary>
        /// <param name="name"></param>
        public void Unset(string name)
        {
            if (Connections.ContainsKey(name))
            {
                Connections.Remove(name);
            }
        }

        #endregion
    }
}