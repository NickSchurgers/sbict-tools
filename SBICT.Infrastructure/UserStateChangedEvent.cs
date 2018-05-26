// <copyright file="SystemLogEvent.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

using SBICT.Data;
using SBICT.Infrastructure.Connection;

namespace SBICT.Infrastructure
{
    using Prism.Events;
    using SBICT.Infrastructure.Logger;

    /// <inheritdoc />
    public class UserStateChangedEvent : PubSubEvent<ConnectionEventArgs>
    {
    }
}