using Prism.Mvvm;
using System.Collections.ObjectModel;
using Prism.Events;
using SBICT.Infrastructure;
using SBICT.Infrastructure.Logger;

namespace SBICT.Modules.SystemLog.ViewModels
{
    public class SystemLogWindowViewModel : BindableBase
    {
        #region Fields

        public ObservableCollection<Log> LogEntries { get; } =
            new ObservableCollection<Log> {new Log {Message = "Application Starting", LogLevel = LogLevel.Info}};

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="eventAggregator"></param>
        public SystemLogWindowViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<SystemLogEvent>().Subscribe(WriteLine, ThreadOption.UIThread);
        }

        /// <summary>
        /// Append a log to the logentries collection
        /// </summary>
        /// <param name="log"></param>
        private void WriteLine(Log log)
        {
            LogEntries.Add(log);
        }

        #endregion
    }
}