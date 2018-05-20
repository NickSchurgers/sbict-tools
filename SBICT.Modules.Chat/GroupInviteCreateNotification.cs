using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Interactivity.InteractionRequest;
using SBICT.Data;

namespace SBICT.Modules.Chat
{
    public class GroupInviteCreateNotification : Confirmation
    {
        public bool IsNew { get; private set; }
        public string GroupName { get; set; }

        public GroupInviteCreateNotification(string groupName = null)
        {
            Items = new List<IUser>();
            SelectedItems = new List<IUser>();
            GroupName = groupName ?? "New Group";
            IsNew = groupName == null;
        }

        public GroupInviteCreateNotification(IEnumerable<IUser> items, string groupName = null) : this(groupName)
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