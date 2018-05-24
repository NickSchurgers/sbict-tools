using System;

namespace SBICT.Infrastructure
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for storage providers.
    /// </summary>
    /// <typeparam name="T">Type to key by.</typeparam>
    /// <typeparam name="G">Type of value.</typeparam>
    public interface IStore<T, G>
    {
        /// <summary>
        /// Add a value to the specified key. If key doesn't exist, it gets inserted.
        /// </summary>
        /// <param name="key">Key to add to.</param>
        /// <param name="value">Value to add.</param>
        void Add(T key, G value);

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
        int Count(T key);

        /// <summary>
        /// Get values belonging to the key.
        /// </summary>
        /// <param name="key">Key to get values of.</param>
        /// <returns>List of values.</returns>
        IEnumerable<G> GetValues(T key);

        /// <summary>
        /// Get a key by predicate.
        /// </summary>
        /// <param name="func">Predicate to find the key with.</param>
        /// <returns></returns>
        T GetKey(Func<T, bool> func);

        /// <summary>
        /// Get a list of keys.
        /// </summary>
        /// <param name="func">Predicate to find the key with.</param>
        /// <returns></returns>
        IEnumerable<T> GetKeys(Func<T, bool> func);

        /// <summary>
        /// Remove a value from the specified key. If a key no longer has any values, it gets deleted.
        /// </summary>
        /// <param name="key">Key to remove from.</param>
        /// <param name="value">Value to remove.</param>
        void Remove(T key, G value);

        /// <summary>
        /// Remove the specified key and all it's values.
        /// </summary>
        /// <param name="key">Key to remove from.</param>
        void Remove(T key);
    }
}