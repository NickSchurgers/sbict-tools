using System;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;

namespace SBICT.Modules.Chat.ViewModels
{
    public class GroupCreateViewModel : BindableBase, IInteractionRequestAware
    {
        public INotification Notification { get; set; }
        public Action FinishInteraction { get; set; }
    }
}