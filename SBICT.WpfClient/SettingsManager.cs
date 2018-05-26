// <copyright file="SettingsManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SBICT.WpfClient
{
    using System;
    using System.Security.Principal;
    using SBICT.Data;
    using SBICT.Infrastructure;
    using SBICT.WpfClient.Properties;

    /// <inheritdoc />
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SettingsManager : ISettingsManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsManager"/> class.
        /// </summary>
        public SettingsManager()
        {
            if (this.IsFirstRun)
            {
                Settings.Default.Guid = Guid.NewGuid();
                Settings.Default.FirstRun = false;
                Settings.Default.DisplayName = WindowsIdentity.GetCurrent().Name;
                Settings.Default.Save();
            }

            this.User = new User(Settings.Default.Guid, WindowsIdentity.GetCurrent().Name)
            {
                DisplayName = Settings.Default.DisplayName,
            };

#if DEBUG
            var rand = new Random().Next();
            var name = $"TestUser{rand}";
            this.User = new User(Guid.NewGuid(), name)
            {
                DisplayName = name,
            };
#endif
        }

        /// <inheritdoc />
        public bool IsFirstRun => Settings.Default.FirstRun;

        /// <inheritdoc />
        public IUser User { get; }

        /// <inheritdoc />
        public (string, int) Server => (Settings.Default.ServerAddress, Settings.Default.ServerPort);
    }
}