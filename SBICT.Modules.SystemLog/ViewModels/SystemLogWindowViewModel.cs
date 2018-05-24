namespace SBICT.Modules.SystemLog.ViewModels
{
    using System.Collections.ObjectModel;
    using Prism.Events;
    using Prism.Mvvm;
    using SBICT.Infrastructure;
    using SBICT.Infrastructure.Logger;

    /// <inheritdoc />
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SystemLogWindowViewModel : BindableBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemLogWindowViewModel"/> class.
        /// </summary>
        /// <param name="eventAggregator">Prism EventAggreggator.</param>
        public SystemLogWindowViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<SystemLogEvent>().Subscribe(this.WriteLine, ThreadOption.UIThread);
            this.LogEntries =
                new ObservableCollection<Log> {new Log {Message = "Application Starting", LogLevel = LogLevel.Info}};
        }

        /// <summary>
        /// Gets a collection of all logged items.
        /// </summary>
        public ObservableCollection<Log> LogEntries { get; }

        /// <summary>
        /// Append a log to the logentries collection.
        /// </summary>
        /// <param name="log">Log instance.</param>
        private void WriteLine(Log log)
        {
            this.LogEntries.Add(log);
        }
    }
}