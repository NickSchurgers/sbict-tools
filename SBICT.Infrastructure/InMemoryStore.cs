namespace SBICT.Infrastructure
{
    //http://www.tugberkugurlu.com/archive/mapping-asp-net-signalr-connections-to-real-application-users
    //https://stackoverflow.com/questions/33031517/prevent-duplicate-users-in-online-users-list-signalr?rq=1
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    /// <inheritdoc cref="IStore{T,G}" />
    public class InMemoryStore<T, G> : IStore<T, G>
    {
        private ConcurrentDictionary<T, HashSet<G>> data;

        public InMemoryStore()
        {
            data = new ConcurrentDictionary<T, HashSet<G>>();
        }

        public InMemoryStore(IEqualityComparer<T> equalityComparer)
        {
            data = new ConcurrentDictionary<T, HashSet<G>>(equalityComparer);
        }

        /// <inheritdoc />
        public void Add(T key, G value)
        {
            throw new System.NotImplementedException();
        }

        public int Count()
        {
            return data.Count;
        }

        /// <inheritdoc />
        public int Count(T key)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public IEnumerable<G> GetValues(T key)
        {
            throw new System.NotImplementedException();
        }

        public T GetKey(Func<T, bool> func)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetKeys(Func<T, bool> func)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Remove(T key, G value)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(T key)
        {
            throw new System.NotImplementedException();
        }
    }
}