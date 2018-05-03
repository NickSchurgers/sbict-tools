using System.Collections.Generic;
using System.Linq;

namespace SBICT.Infrastructure.Connection
{
    /// <summary>
    /// Class used to store connections by client
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConnectionStore<T>
    {
        #region Fields

        private readonly Dictionary<T, HashSet<string>> _connections = new Dictionary<T, HashSet<string>>();

        #endregion


        #region Methods

        /// <summary>
        /// Return the amount of connections stored
        /// </summary>
        public int Count
        {
            get
            {
                lock (_connections)
                {
                    return _connections.Count;
                }
            }
        }

        /// <summary>
        /// Add a connection to the store
        /// </summary>
        /// <param name="key"></param>
        /// <param name="connectionId"></param>
        public void Add(T key, string connectionId)
        {
            lock (_connections)
            {
                if (!_connections.TryGetValue(key, out var connections))
                {
                    connections = new HashSet<string>();
                    _connections.Add(key, connections);
                }

                lock (connections)
                {
                    connections.Add(connectionId);
                }
            }
        }

        /// <summary>
        /// Return all collections belonging to a single client
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<string> GetConnections(T key)
        {
            lock (_connections)
            {
                if (_connections.TryGetValue(key, out var connections))
                {
                    return connections;
                }
            }

            return Enumerable.Empty<string>();
        }

        /// <summary>
        /// Return a list of all connected clients
        /// </summary>
        /// <returns></returns>
        public List<T> GetConnections()
        {
            lock (_connections)
            {
                return _connections.Keys.ToList();
            }
        }

        /// <summary>
        /// Remove a connection with a client from the store
        /// </summary>
        /// <param name="key"></param>
        /// <param name="connectionId"></param>
        public void Remove(T key, string connectionId)
        {
            lock (_connections)
            {
                if (!_connections.TryGetValue(key, out var connections))
                {
                    return;
                }

                lock (connections)
                {
                    connections.Remove(connectionId);

                    if (connections.Count == 0)
                    {
                        _connections.Remove(key);
                    }
                }
            }
        }

        #endregion
    }
}