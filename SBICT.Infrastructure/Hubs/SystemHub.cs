// <copyright file="SystemHub.cs" company="SBICT">
// Copyright (c) SBICT. All rights reserved.
// </copyright>

// ReSharper disable ClassNeverInstantiated.Global
namespace SBICT.Infrastructure.Hubs
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Logging;
    using SBICT.Data;

    /// <inheritdoc />
    /// <summary>
    /// Hub used for system connections.
    /// </summary>
    [Authorize]
    public class SystemHub : HubBase
    {
        private static readonly IStore<IUser, string> UserConnectionStore =
            new InMemoryStore<IUser, string>(new UserComparer());

        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemHub"/> class.
        /// </summary>
        /// <param name="loggerFactory">Instance of ILoggerFactory</param>
        public SystemHub(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger("SystemHub");
        }

        /// <inheritdoc />
        protected override IStore<IUser, string> GetUserConnectionStore()
        {
            return UserConnectionStore;
        }
    }
}