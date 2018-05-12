using System;
using SBICT.Data;

namespace SBICT.Modules.Chat
{
    public class ChatUser: IUser
    {
        public Guid Id { get; }
        public string UserName { get; }
        public string DisplayName { get; set; }
        
        public ChatUser(Guid id, string userName)
        {
            Id = id;
            UserName = userName;
            DisplayName = userName;
        }
    }
}