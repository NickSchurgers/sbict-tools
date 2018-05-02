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
        public MainWindowViewModel(IModuleManager moduleManager, IEventAggregator eventAggregator)
        {
            //Set up commands
            WindowClosing = new DelegateCommand(DeInitializeSystemHub);
            WindowLoaded = new DelegateCommand(OnWindowLoaded);

            _eventAggregator = eventAggregator;
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
            _systemConnection.UserStatusChanged += SystemConnectionOnUserStatusChanged;

            await _systemConnection.StartAsync();
        }

        /// <summary>
        /// Dispose of connection with the system hub
        /// </summary>
        private async void DeInitializeSystemHub()
        {
            await _systemConnection.StopAsync();
        }

        /// <summary>
        /// Publish an event to the systemlog
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logLevel"></param>
        private void LogEvent(string message, LogLevel logLevel = LogLevel.Info)
        {
            _eventAggregator.GetEvent<SystemLogEvent>()
                .Publish(new Log {Message = message, LogLevel = logLevel});
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
        /// Triggered when a user (dis)connects from the suystem hub
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void SystemConnectionOnUserStatusChanged(object sender, ConnectionEventArgs e)
        {
            var message = $"{e.User} has ";
            switch (e.Status)
            {
                case ConnectionStatus.Connected:
                    LogEvent($"{message} joined");
                    break;
                case ConnectionStatus.Disconnected:
                    LogEvent($"{message} left");
                    break;
                case ConnectionStatus.Connecting:
                case ConnectionStatus.Reconnecting:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

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
            Console.WriteLine(e.ModuleInfo.ModuleName);
            LogEvent($"{e.ModuleInfo.ModuleName} has been loaded");
        }

        /// <summary>
        /// Trigered when window has been loaded
        /// </summary>
        private void OnWindowLoaded()
        {
            LogEvent("Application Loaded");
            InitializeSystemHub();
        }

        #endregion
    }
}