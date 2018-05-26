// <copyright file="ChatBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SBICT.Modules.Chat
{
    using System;
    using System.Collections.ObjectModel;
    using Prism.Mvvm;
    using SBICT.Infrastructure.Chat;
    using SBICT.Infrastructure.Connection;

    /// <inheritdoc cref="IChatWindow" />
    public abstract class ChatBase : BindableBase, IChatWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatBase"/> class.
        /// </summary>
        /// <param name="scope">Scope of recipient of the window.</param>
        protected ChatBase(ConnectionScope scope)
        {
            this.Scope = scope;
            this.Messages = new ObservableCollection<IChatMessage>();
        }

        /// <inheritdoc/>
        public string Title { get; set; }

        /// <inheritdoc/>
        public bool IsActive { get; set; }

        /// <inheritdoc/>
        public ConnectionScope Scope { get; }

        /// <inheritdoc/>
        public ObservableCollection<IChatMessage> Messages { get; }

        /// <inheritdoc/>
        public abstract Guid GetRecipient();
    }
}