using System;
using System.Runtime.Remoting.Channels;
using System.Security.Principal;
using System.Windows;
using Microsoft.AspNetCore.Hosting;
using SBICT.Data;
using SBICT.Infrastructure;
using SBICT.WpfClient.Properties;

namespace SBICT.WpfClient
{
    public class SettingsManager : ISettingsManager
    {
        public bool IsFirstRun => Settings.Default.FirstRun;

        public User User { get; }

        public (string, int) Server => (Settings.Default.ServerAddress, Settings.Default.ServerPort);

        public SettingsManager()
        {
            if (IsFirstRun)
            {
                Settings.Default.Guid = Guid.NewGuid();
                Settings.Default.FirstRun = false;
                Settings.Default.DisplayName = WindowsIdentity.GetCurrent().Name;
                Settings.Default.Save();
            }

            User = new User(Settings.Default.Guid, Settings.Default.DisplayName)
            {
                DisplayName = Settings.Default.DisplayName
            };
        }
    }
}