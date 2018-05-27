using CommonServiceLocator;
using Prism.Ioc;
using Prism.Unity;
using SBICT.WpfClient.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Prism.Events;
using Prism.Modularity;
using SBICT.Data;
using SBICT.Infrastructure;
using SBICT.Infrastructure.Connection;

namespace SBICT.WpfClient
{
    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : PrismApplication
    {
        /// <inheritdoc />
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ISettingsManager, SettingsManager>();
            containerRegistry.RegisterSingleton<IConnectionFactory, ConnectionFactory>();
            containerRegistry.Register<IConnection, Connection>();
        }

        /// <inheritdoc />
        protected override Window CreateShell()
        {
            return ServiceLocator.Current.GetInstance<MainWindow>();
        }

        /// <inheritdoc />
        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new DirectoryModuleCatalog {ModulePath = @".\Modules"};
        }
    }
}