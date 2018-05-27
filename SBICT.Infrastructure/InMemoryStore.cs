// <copyright file="InMemoryStore.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure
{
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
            this.data.AddOrUpdate(key, new HashSet<TValue> {value}, (k, s) =>
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
            return this.data.TryGetValue(key, out var values) ? values.Count : 0;
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
                return this.data.Keys.SingleOrDefault(func);
            }
        }

        /// <inheritdoc/>
        public IEnumerable<TKey> GetKeys(Func<KeyValuePair<TKey, HashSet<TValue>>, bool> func)
        {
            lock (this.data)
            {
                return this.data.Where(func).Select(x => x.Key).ToList();
            }
        }

        /// <inheritdoc />
        public KeyValuePair<TKey, HashSet<TValue>> GetKeyValuePair(Func<KeyValuePair<TKey, HashSet<TValue>>, bool> func)
        {
            lock (this.data)
            {
                return this.data.SingleOrDefault(func);
            }
        }

        /// <inheritdoc />
        public void Remove(TKey key, TValue value)
        {
            lock (this.data)
            {
                if (!this.data.ContainsKey(key))
                {
                    return;
                }

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