using System.Collections.Generic;

namespace SBICT.Modules.Chat
{
    public struct ChatGroup
    {
        public string Name { get; set; }
        public List<Chat> Chats { get; set; }
        public List<string> Participants { get; set; }
    }
}