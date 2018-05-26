// <copyright file="InMemoryStore.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure
{
    // http://www.tugberkugurlu.com/archive/mapping-asp-net-signalr-connections-to-real-application-users
    // https://stackoverflow.com/questions/33031517/prevent-duplicate-users-in-online-users-list-signalr?rq=1
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    /// <inheritdoc cref="IStore{TKey,TValue}" />
    public class InMemoryStore<TKey, TValue> : IStore<TKey, TValue>
    {
        private readonly ConcurrentDictionary<TKey, HashSet<TValue>> data;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryStore{T, G}"/> class.
        /// </summary>
        public InMemoryStore()
        {
            this.data = new ConcurrentDictionary<TKey, HashSet<TValue>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryStore{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="equalityComparer">Instance of IEqualityComparer used to compare keys.</param>
        public InMemoryStore(IEqualityComparer<TKey> equalityComparer)
        {
            this.data = new ConcurrentDictionary<TKey, HashSet<TValue>>(equalityComparer);
        }

        /// <inheritdoc />
        public void Add(TKey key, TValue value)
        {
            this.data.AddOrUpdate(key, new HashSet<TValue> { value }, (k, s) =>
            {
                s.Add(value);
                return s;
            });
        }

        /// <inheritdoc/>
        public int Count()
        {
            return this.data.Count;
        }

        /// <inheritdoc />
        public int Count(TKey key)
        {
            if (this.data.TryGetValue(key, out var values))
            {
                return values.Count;
            }

            return 0;
        }

        /// <inheritdoc />
        public IEnumerable<TValue> GetValues(TKey key)
        {
            return this.data.TryGetValue(key, out var values) ? values : new HashSet<TValue>();
        }

        /// <inheritdoc/>
        public TKey GetKey(Func<TKey, bool> func)
        {
            lock (this.data)
            {
                return this.data.Keys.Single(func);
            }
        }

        /// <inheritdoc/>
        public IEnumerable<TKey> GetKeys(Func<TKey, bool> func)
        {
            lock (this.data)
            {
                return this.data.Keys.Where(func).ToList();
            }
        }

        /// <inheritdoc />
        public void Remove(TKey key, TValue value)
        {
            lock (this.data)
            {
                var values = this.data.Single(k => k.Key.Equals(key)).Value;
                values.Remove(value);
                if (values.Count == 0)
                {
                    this.Remove(key);
                }
            }
        }

        /// <inheritdoc/>
        public void Remove(TKey key)
        {
            this.data.TryRemove(key, out var removedValue);
        }
    }
}