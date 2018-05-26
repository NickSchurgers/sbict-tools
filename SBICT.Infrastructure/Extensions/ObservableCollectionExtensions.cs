// <copyright file="ObservableCollectionExtensions.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Extensions
{
    using System;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Extensions for ObservableCollection.
    /// </summary>
    public static class ObservableCollectionExtensions
    {
        /// <summary>
        /// Remove all items by predicate.
        /// </summary>
        /// <typeparam name="T">Type of the keys.</typeparam>
        /// <param name="collection">Collection to remove the items from.</param>
        /// <param name="condition">Predicate used to get the items to remove.</param>
        public static void RemoveAll<T>(this ObservableCollection<T> collection, Func<T, bool> condition)
        {
            for (var i = collection.Count - 1; i >= 0; i--)
            {
                if (condition(collection[i]))
                {
                    collection.RemoveAt(i);
                }
            }
        }
    }
}