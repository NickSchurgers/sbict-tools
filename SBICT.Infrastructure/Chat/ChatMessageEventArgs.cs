namespace SBICT.Infrastructure.Chat
{
    public class ChatMessageEventArgs
    {
        public IChatMessage ChatMessage { get; }

        public ChatMessageEventArgs(IChatMessage chatMessage)
        {
            ChatMessage = chatMessage;
        }
    }
}