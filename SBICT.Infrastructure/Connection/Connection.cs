using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace SBICT.Infrastructure.Connection
{
    public class Connection : IConnection
    {
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
                OnConnectionStatusChanged(new ConnectionEventArgs {Status = _status});
            }
        }

        public HubConnection Hub { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connection"></param>
        public Connection(HubConnection connection)
        {
            Hub = connection;
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

                    Hub.On<string, ConnectionScope>("Connected",
                        (user, scope) => OnUserStatusChanged(new ConnectionEventArgs
                        {
                            Status = ConnectionStatus.Connected,
                            User = user
                        }));
                    Hub.On<string, ConnectionScope>("Disconnected",
                        (user, scope) => OnUserStatusChanged(new ConnectionEventArgs
                        {
                            Status = ConnectionStatus.Disconnected,
                            User = user
                        }));

                    await Hub.StartAsync();
                    Status = ConnectionStatus.Connected;
                    _isStarted = true;
                }
                catch (Exception e)
                {
                    Status = ConnectionStatus.Disconnected;
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
        /// Triggered when another client (dis)connects or updates it's status
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnUserStatusChanged(ConnectionEventArgs args)
        {
            UserStatusChanged?.Invoke(this, args);
        }

        #endregion

        #region Events

        public event EventHandler<ConnectionEventArgs> ConnectionStatusChanged;
        public event EventHandler<ConnectionEventArgs> UserStatusChanged;

        #endregion
    }
}