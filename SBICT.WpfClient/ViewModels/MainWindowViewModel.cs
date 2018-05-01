using System;
using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Events;
using Prism.Modularity;
using Prism.Mvvm;
using SBICT.Data.Enums;
using SBICT.Data.Models;
using SBICT.Infrastructure;

namespace SBICT.WpfClient.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public DelegateCommand WindowClosing { get; set; }
        public DelegateCommand WindowLoaded { get; set; }


        private string _title = "SBICT Application";
        private readonly IEventAggregator _eventAggregator;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }


        public MainWindowViewModel(IModuleManager moduleManager, IEventAggregator eventAggregator)
        {
            WindowClosing = new DelegateCommand(DeInitializeSystemHub);
            WindowLoaded = new DelegateCommand(OnWindowLoaded);
            _eventAggregator = eventAggregator;
            moduleManager.LoadModuleCompleted += ModuleManagerOnLoadModuleCompleted;
        }

        private void ModuleManagerOnLoadModuleCompleted(object sender, LoadModuleCompletedEventArgs e)
        {
            _eventAggregator.GetEvent<SystemLogEvent>()
                .Publish(new Log {Message = $"{e.ModuleInfo.ModuleName} has been loaded", LogLevel = LogLevel.Info});
        }

        private void DeInitializeSystemHub()
        {
        }

        private void OnWindowLoaded()
        {
            _eventAggregator.GetEvent<SystemLogEvent>()
                .Publish(new Log {Message = "Application Started", LogLevel = LogLevel.Info});
        }
    }
}