using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using SBICT.Data;

namespace SBICT.Modules.Chat.ViewModels
{
    public class GroupJoinCreateViewModel : BindableBase, IInteractionRequestAware
    {
        public bool IsCreate { get; set; }
        public INotification Notification { get; set; }
        public Action FinishInteraction { get; set; }
        public ObservableCollection<IUser> Users { get; set; }
        public string GroupName { get; set; }
    }
}