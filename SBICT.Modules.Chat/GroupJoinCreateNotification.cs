using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Interactivity.InteractionRequest;
using SBICT.Data;

namespace SBICT.Modules.Chat
{
    public class GroupJoinCreateNotification : Confirmation
    {
        public GroupJoinCreateNotification()
        {
            Items = new List<IUser>();
            SelectedItems = new List<IUser>();
        }

        public GroupJoinCreateNotification(IEnumerable<IUser> items) : this()
        {
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        public IList<IUser> SelectedItems { get; set; }

        public IList<IUser> Items { get; private set; }
    }
}