﻿using System;

namespace SBICT.Data
{
    public struct User : IUser
    {
        public Guid Id { get; }
        public string UserName { get; }
        public string DisplayName { get; set; }

        public User(Guid id, string userName)
        {
            Id = id;
            UserName = userName;
            DisplayName = userName;
        }
    }
}