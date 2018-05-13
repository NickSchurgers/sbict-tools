using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SBICT.Infrastructure.Chat;

namespace SBICT.Modules.Chat.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static IChatChannel ByName(this IEnumerable<IChatChannel> collection, string name)
        {
            return collection.Single(c => c.Name == name);
        }

        public static IChat ById(this IEnumerable<IChat> collection, Guid id)
        {
            return collection.Single(c => c.Recipient.Id == id);
        }

        public static IChatGroup ById(this IEnumerable<IChatGroup> collection, Guid id)
        {
            return collection.Single(c => c.Id == id);
        }
    }
}