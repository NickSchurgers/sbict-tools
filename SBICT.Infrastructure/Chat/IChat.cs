using SBICT.Data;

namespace SBICT.Infrastructure.Chat
{
    public interface IChat
    {
        IUser Recipient { get; }
    }
}