namespace SBICT.Modules.Chat.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SBICT.Infrastructure.Chat;

    /// <summary>
    /// Set of extensions methods for ObservableCollection.
    /// </summary>
    public static class ObservableCollectionExtensions
    {
        /// <summary>
        /// Find an IChatChannel by name.
        /// </summary>
        /// <param name="collection">Collection of IChatChannels to search in.</param>
        /// <param name="name">Name of the IChatChannel to find.</param>
        /// <returns>IChatChannel.</returns>
        public static IChatChannel ByName(this IEnumerable<IChatChannel> collection, string name)
        {
            return collection.Single(c => c.Name == name);
        }

        /// <summary>
        /// Find an IChat by ID of the recipient. 
        /// </summary>
        /// <param name="collection">Collection of IChats.</param>
        /// <param name="id">Id of the recipient.</param>
        /// <returns>IChat.</returns>
        public static IChat ById(this IEnumerable<IChat> collection, Guid id)
        {
            return collection.Single(c => c.Recipient.Id == id);
        }

        /// <summary>
        /// Find an IChatGroup by ID. 
        /// </summary>
        /// <param name="collection">Collection of IChatGroups.</param>
        /// <param name="id">Id of the IChatGroup.</param>
        /// <returns>IChatGroup.</returns>
        public static IChatGroup ById(this IEnumerable<IChatGroup> collection, Guid id)
        {
            return collection.Single(c => c.Id == id);
        }
    }
}