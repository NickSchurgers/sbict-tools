// <copyright file="UserStateChangedEvent.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure.Connection
{
    using Prism.Events;

    /// <inheritdoc />
    public class UserStateChangedEvent : PubSubEvent<ConnectionEventArgs>
    {
    }
}