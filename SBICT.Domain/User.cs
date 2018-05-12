using System;

namespace SBICT.Data
{
    [Serializable]
    public class User
    {
        public Guid Id { get; }
        public string UserName { get; }
        public string DisplayName { get; set; }

        public User(Guid id, string userName)
        {
            Id = id;
            UserName = userName;
        }
    }
}