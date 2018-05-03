using System;
using Prism.Commands;
using Prism.Events;
using Prism.Modularity;
using Prism.Mvvm;
using SBICT.Data.Enums;
using SBICT.Data.Models;
using SBICT.Infrastructure;
using SBICT.Infrastructure.Connection;

namespace SBICT.WpfClient.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region Commands

        public DelegateCommand WindowClosing { get; set; }
        public DelegateCommand WindowLoaded { get; set; }

        #endregion

        #region Fields

        private string _title = "SBICT Application";
        private readonly IEventAggregator _eventAggregator;
        private readonly IConnectionManager<Connection> _connectionManager;
        private Connection _systemConnection;
        private string _statusText;

        #endregion

        #region Properties

        /// <summary>
        /// Title of the application
        /// </summary>
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        /// <summary>
        /// Text displayed in the status bar
        /// </summary>
        public string StatusText
        {
            get => _statusText;
            set => SetProperty(ref _statusText, value);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="moduleManager"></param>
        /// <param name="eventAggregator"></param>
        /// <param name="connectionManager"></param>
        public MainWindowViewModel(IModuleManager moduleManager, IEventAggregator eventAggregator,
            IConnectionManager<Connection> connectionManager)
        {
            //Set event aggreggator te event logger
            SystemLogger.EventAggregator = eventAggregator;
            
            //Set up commands
            WindowClosing = new DelegateCommand(DeInitializeSystemHub);
            WindowLoaded = new DelegateCommand(OnWindowLoaded);

            _eventAggregator = eventAggregator;
            _connectionManager = connectionManager;
            moduleManager.LoadModuleCompleted += ModuleManagerOnLoadModuleCompleted;
        }

        /// <summary>
        /// Initialize a connection with the system hub
        /// </summary>
        private async void InitializeSystemHub()
        {
            //TODO: Refactor url to config
            _systemConnection = ConnectionFactory.Create("http://localhost:5000/hubs/system");
            _systemConnection.ConnectionStatusChanged += SystemConnectionOnConnectionStatusChanged;
            _connectionManager.Set("System", _systemConnection);
            await _systemConnection.StartAsync();
        }

        /// <summary>
        /// Dispose of connection with the system hub
        /// </summary>
        private async void DeInitializeSystemHub()
        {
            await _systemConnection.StopAsync();
            _connectionManager.Unset("System");
        }

        /// <summary>
        /// Update status text
        /// </summary>
        /// <param name="message"></param>
        private void UpdateStatusText(string message)
        {
            StatusText = message;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Triggerd when connection with the system hub changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SystemConnectionOnConnectionStatusChanged(object sender, ConnectionEventArgs e)
        {
            UpdateStatusText($"State: {e.Status.ToString()}");
        }

        /// <summary>
        /// Triggered when a module has been loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModuleManagerOnLoadModuleCompleted(object sender, LoadModuleCompletedEventArgs e)
        {
            SystemLogger.LogEvent($"{e.ModuleInfo.ModuleName} has been loaded");
        }

        /// <summary>
        /// Trigered when window has been loaded
        /// </summary>
        private void OnWindowLoaded()
        {
            InitializeSystemHub();
            SystemLogger.LogEvent("Application Loaded");
        }

        #endregion
    }
}