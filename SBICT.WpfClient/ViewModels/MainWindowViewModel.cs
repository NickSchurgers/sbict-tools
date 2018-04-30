using System;
using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Mvvm;
using SBICT.Data.Models;

namespace SBICT.WpfClient.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public DelegateCommand WindowClosing { get; set; }
        public ObservableCollection<Log> LogEntries { get; } = new ObservableCollection<Log>();
        
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }



        public MainWindowViewModel()
        {
            WindowClosing = new DelegateCommand(DeInitializeSystemHub);
        }

        private void DeInitializeSystemHub()
        {
            
        }
    }
}
