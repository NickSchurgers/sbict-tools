﻿using System;

namespace SBICT.Data
{
    public interface IUser
    {
        Guid Id { get; }
        string UserName { get; }
        string DisplayName { get; set; }

    }
}