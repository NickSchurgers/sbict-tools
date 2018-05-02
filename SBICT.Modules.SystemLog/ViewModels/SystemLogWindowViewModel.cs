using Prism.Commands;
using Prism.Mvvm;
using SBICT.Data.Models;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Prism.Events;
using SBICT.Data.Enums;
using SBICT.Infrastructure;

namespace SBICT.Modules.SystemLog.ViewModels
{
    public class SystemLogWindowViewModel : BindableBase
    {
        public ObservableCollection<Log> LogEntries { get; } = new ObservableCollection<Log> { new Log { Message = "Application Starting", LogLevel = LogLevel.Info} };

        public SystemLogWindowViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<SystemLogEvent>().Subscribe(WriteLine, ThreadOption.UIThread);
        }

        private void WriteLine(Log log)
        {
            LogEntries.Add(log);
        }
    }
}