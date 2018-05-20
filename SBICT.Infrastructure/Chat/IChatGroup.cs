﻿using System;
using System.Collections.ObjectModel;
using SBICT.Data;

namespace SBICT.Infrastructure.Chat
{
    public interface IChatGroup: IGroup
    {
        ObservableCollection<IUser> Participants { get; }
    }
}