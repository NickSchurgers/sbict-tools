// <copyright file="IStore.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Interface for storage providers.
    /// </summary>
    /// <typeparam name="TKey">Type to key by.</typeparam>
    /// <typeparam name="TValue">Type of value.</typeparam>
    public interface IStore<TKey, TValue>
    {
        /// <summary>
        /// Add a value to the specified key. If key doesn't exist, it gets inserted.
        /// </summary>
        /// <param name="key">Key to add to.</param>
        /// <param name="value">Value to add.</param>
        void Add(TKey key, TValue value);

        /// <summary>
        /// Count all keys.
        /// </summary>
        /// <returns>Amount of keys.</returns>
        int Count();

        /// <summary>
        /// Count values belonging to the key.
        /// </summary>
        /// <param name="key">Key to count values of.</param>
        /// <returns>Amount of values.</returns>
        int Count(TKey key);

        /// <summary>
        /// Get values belonging to the key.
        /// </summary>
        /// <param name="key">Key to get values of.</param>
        /// <returns>List of values.</returns>
        IEnumerable<TValue> GetValues(TKey key);

        /// <summary>
        /// Get a key by predicate.
        /// </summary>
        /// <param name="func">Predicate to find the key with.</param>
        /// <returns>Found key.</returns>
        TKey GetKey(Func<TKey, bool> func);

        /// <summary>
        /// Get a list of keys by predicate.
        /// </summary>
        /// <param name="func">Predicate to filter the list with.</param>
        /// <returns>List of keys matching the predicate.</returns>
        IEnumerable<TKey> GetKeys(Func<KeyValuePair<TKey, HashSet<TValue>>, bool> func);

        /// <summary>
        /// Get a KeyValuePair by predicate.
        /// </summary>
        /// <param name="func">Predicate to find the KeyValuePair with.</param>
        /// <returns>Single KeyValuePair matchin the predicate.</returns>
        KeyValuePair<TKey, HashSet<TValue>> GetKeyValuePair(Func<KeyValuePair<TKey, HashSet<TValue>>, bool> func);

        /// <summary>
        /// Remove a value from the specified key. If a key no longer has any values, it gets deleted.
        /// </summary>
        /// <param name="key">Key to remove from.</param>
        /// <param name="value">Value to remove.</param>
        void Remove(TKey key, TValue value);

        /// <summary>
        /// Remove the specified key and all it's values.
        /// </summary>
        /// <param name="key">Key to remove from.</param>
        void Remove(TKey key);
    }
}