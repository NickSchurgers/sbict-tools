using System;
using System.Collections.ObjectModel;
using SBICT.Data;
using SBICT.Infrastructure.Chat;
using SBICT.Infrastructure.Connection;

namespace SBICT.Modules.Chat
{
    public class ChatGroup : ChatBase, IChatGroup
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }

        public ObservableCollection<IUser> Participants { get; }

        public ChatGroup(string groupName) : base(ConnectionScope.Group)
        {
            Participants = new ObservableCollection<IUser>();
            Id = Guid.NewGuid();
            Name = groupName;
            Title = groupName;
        }

        public override Guid GetRecipient()
        {
            return Id;
        }

        public static explicit operator ChatGroup(Group group)
        {
            return new ChatGroup(group.Name) {Id = group.Id};
        }
    }
}