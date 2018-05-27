// <copyright file="IChatWindow.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Chat
{
    using System;
    using System.Collections.ObjectModel;
    using SBICT.Infrastructure.Connection;

    /// <summary>
    /// Base class for chat windows.
    /// </summary>
    public interface IChatWindow
    {
        /// <summary>
        /// Gets or sets the title of the window.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the window is active.
        /// </summary>
        bool IsActive { get; set; }

        /// <summary>
        /// Gets the scope of the window.
        /// </summary>
        ConnectionScope Scope { get; }

        /// <summary>
        /// Gets the messages of the chat.
        /// </summary>
        ObservableCollection<IChatMessage> Messages { get; }

        /// <summary>
        /// Gets the recipient of the window.
        /// </summary>
        /// <returns>Guid used as recipient indentifier of the inherited classes.</returns>
        Guid GetRecipient();
    }
}