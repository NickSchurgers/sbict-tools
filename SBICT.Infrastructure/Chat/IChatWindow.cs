// <copyright file="IChatWindow.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Chat
{
    using System;
    using System.Collections.ObjectModel;
    using SBICT.Infrastructure.Connection;

    public interface IChatWindow
    {
        string Title { get; set; }

        bool IsActive { get; set; }

        ConnectionScope Scope { get; }

        ObservableCollection<IChatMessage> Messages { get; }

        Guid GetRecipient();
    }
}