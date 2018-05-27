// <copyright file="MainWindowViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SBICT.WpfClient.ViewModels
{
    using Prism.Commands;
    using Prism.Events;
    using Prism.Modularity;
    using Prism.Mvvm;
    using SBICT.Infrastructure;
    using SBICT.Infrastructure.Connection;
    using SBICT.Infrastructure.Hubs;
    using SBICT.Infrastructure.Logger;

    /// <inheritdoc />
    /// <summary>
    /// ViewModel for MainWindow.
    /// </summary>

    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowViewModel : BindableBase
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IConnectionFactory connectionFactory;
        private readonly ISettingsManager settingsManager;
        private string title = "SBICT Application";
        private string statusText;
        private IConnection systemConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
        /// </summary>
        /// <param name="moduleManager">Prism ModuleManager.</param>
        /// <param name="eventAggregator">Prism EventAggregator.</param>
        /// <param name="connectionFactory">Instance of IConnectionFactory.</param>
        /// <param name="settingsManager">SettingsManager.</param>
        public MainWindowViewModel(
            IModuleManager moduleManager,
            IEventAggregator eventAggregator,
            IConnectionFactory connectionFactory,
            ISettingsManager settingsManager)
        {
            // Set event aggreggator te event logger
            SystemLogger.EventAggregator = eventAggregator;

            // Set up commands
            this.WindowClosing = new DelegateCommand(this.DeInitializeSystemHub);
            this.WindowLoaded = new DelegateCommand(this.OnWindowLoaded);

            this.eventAggregator = eventAggregator;
            this.connectionFactory = connectionFactory;
            this.settingsManager = settingsManager;
            moduleManager.LoadModuleCompleted += this.ModuleManagerOnLoadModuleCompleted;
        }

        /// <summary>
        /// Gets or Sets the command raising the WindowClosing event.
        /// </summary>
        public DelegateCommand WindowClosing { get; set; }

        /// <summary>
        /// Gets or Sets the command raising the WindowLoaded event.
        /// </summary>
        public DelegateCommand WindowLoaded { get; set; }

        /// <summary>
        /// Gets or sets title of the application.
        /// </summary>
        public string Title
        {
            get => this.title;
            set => this.SetProperty(ref this.title, value);
        }

        /// <summary>
        /// Gets or sets text displayed in the status bar.
        /// </summary>
        public string StatusText
        {
            get => this.statusText;
            set => this.SetProperty(ref this.statusText, value);
        }

        /// <summary>
        /// Initialize a connection with the system hub.
        /// </summary>
        private async void InitializeSystemHub()
        {
            var user = this.settingsManager.User;
            var (address, port) = this.settingsManager.Server;
            var connection = $"{address}:{port}/hubs/system?displayName={user.DisplayName}&guid={user.Id.ToString()}";

            this.systemConnection = this.connectionFactory.Create(connection, HubNames.SystemHub);
            this.systemConnection.ConnectionStatusChanged += this.SystemConnectionOnConnectionStatusChanged;
            await this.systemConnection.StartAsync();
            SystemLogger.LogEvent($"Logged in as {user.DisplayName} with id {user.Id.ToString()}", LogLevel.Debug);
        }

        /// <summary>
        /// Dispose of connection with the system hub.
        /// </summary>
        private async void DeInitializeSystemHub()
        {
            await this.systemConnection.StopAsync();
        }

        /// <summary>
        /// Triggerd when connection with the system hub changes.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">ConnectionEventArgs.</param>
        private void SystemConnectionOnConnectionStatusChanged(object sender, ConnectionEventArgs e)
        {
            this.StatusText = $"State: {e.Status.ToString()}";
        }

        /// <summary>
        /// Triggered when a module has been loaded.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">LoadModuleCompletedEventArgs.</param>
        private void ModuleManagerOnLoadModuleCompleted(object sender, LoadModuleCompletedEventArgs e)
        {
            SystemLogger.LogEvent($"{e.ModuleInfo.ModuleName} has been loaded");
        }

        /// <summary>
        /// Trigered when window has been loaded.
        /// </summary>
        private void OnWindowLoaded()
        {
            this.InitializeSystemHub();
            SystemLogger.LogEvent("Application Loaded");
        }
    }
}