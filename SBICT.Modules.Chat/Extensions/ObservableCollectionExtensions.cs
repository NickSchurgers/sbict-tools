using System;
using System.Collections.ObjectModel;
using System.Linq;
using SBICT.Infrastructure.Chat;

namespace SBICT.Modules.Chat.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static IChatChannel ByName(this ObservableCollection<IChatChannel> collection, string name)
        {
            return collection.First(c => c.Name == name);
        }
    }
}