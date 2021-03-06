// <copyright file="SystemLogEvent.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

namespace SBICT.Infrastructure
{
    using Prism.Events;
    using SBICT.Infrastructure.Logger;

    /// <inheritdoc />
    public class SystemLogEvent : PubSubEvent<Log>
    {
    }
}