// <copyright file="IChatGroup.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Chat
{
    using System.Collections.ObjectModel;
    using SBICT.Data;

    public interface IChatGroup: IGroup
    {
        ObservableCollection<IUser> Participants { get; }
    }
}