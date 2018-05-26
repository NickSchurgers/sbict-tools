using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Prism.Events;
using SBICT.Data;

namespace SBICT.Infrastructure.Connection
{
    public class Connection : IConnection
    {
        private readonly IEventAggregator eventAggregator;

        #region Fields

        private ConnectionStatus _status = ConnectionStatus.Disconnected;
        private bool _isStarted;

        #endregion

        #region Properties

        public ConnectionStatus Status
        {
            get => _status;
            set
            {
                if (value == _status) return;
                _status = value;
                OnConnectionStatusChanged(new ConnectionEventArgs { Status = _status });
            }
        }

        public HubConnection Hub { get; set; }

        public string HubName { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="hubName"></param>
        /// <param name="eventAggregator"></param>
        public Connection(HubConnection connection, string hubName, IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            Hub = connection;
            HubName = hubName;
        }

        /// <summary>
        /// Start the connection withe the hub
        /// </summary>
        /// <returns></returns>
        /// <exception cref="HubException"></exception>
        public async Task StartAsync()
        {
            if (!_isStarted)
            {
                try
                {
                    Status = ConnectionStatus.Connecting;
                    Hub.On<User, ConnectionScope>(
                        "Connected",
                        (connUser, scope) => this.OnUserStatusChanged(new ConnectionEventArgs
                        {
                            User = connUser,
                            HubName = this.HubName,
                            Status = ConnectionStatus.Connected,
                        }));

                   Hub.On<User, ConnectionScope>(
                        "Disconnected",
                        (connUser, scope) => this.OnUserStatusChanged(new ConnectionEventArgs
                       {
                           User = connUser,
                           HubName = this.HubName,
                           Status = ConnectionStatus.Disconnected,
                       }));
                    await Hub.StartAsync();
                    Status = ConnectionStatus.Connected;
                    _isStarted = true;
                }
                catch (Exception e)
                {
                    await StopAsync();
                    throw new HubException(e.Message);
                }
            }
        }


        /// <summary>
        /// Dispose of the connection with the hub
        /// </summary>
        /// <returns></returns>
        public async Task StopAsync()
        {
            if (_isStarted)
            {
                await Hub.StopAsync();
                Status = ConnectionStatus.Disconnected;
                _isStarted = false;
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Triggered when the status of this connection changes
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnConnectionStatusChanged(ConnectionEventArgs e)
        {
            ConnectionStatusChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Triggered when the status of a user changes.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnUserStatusChanged(ConnectionEventArgs e)
        {
            UserStatusChanged?.Invoke(this, e);
        }

        #endregion

        #region Events

        public event EventHandler<ConnectionEventArgs> ConnectionStatusChanged;
        public event EventHandler<ConnectionEventArgs> UserStatusChanged;

        #endregion
    }
}