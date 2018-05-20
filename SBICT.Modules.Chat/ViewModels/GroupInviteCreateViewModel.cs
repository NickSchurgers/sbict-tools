using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using SBICT.Data;

namespace SBICT.Modules.Chat.ViewModels
{
    public class GroupInviteCreateViewModel : BindableBase, IInteractionRequestAware
    {
        private GroupInviteCreateNotification _notification;
        public DelegateCommand<object> OkCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        public Action FinishInteraction { get; set; }

        public INotification Notification
        {
            get => _notification;
            set
            {
                if (value is GroupInviteCreateNotification notification)
                {
                    _notification = notification;
                    RaisePropertyChanged();
                }
            }
        }

        public GroupInviteCreateViewModel()
        {
            OkCommand = new DelegateCommand<object>(OnOkClick);
            CancelCommand = new DelegateCommand(OnCancelClick);
        }

        private void OnCancelClick()
        {
            if (_notification != null)
            {
                _notification.SelectedItems = null;
                _notification.Confirmed = false;
            }

            FinishInteraction();
        }

        private void OnOkClick(object obj)
        {
            if (_notification != null)
            {
                _notification.SelectedItems = ((IList) obj).Cast<IUser>().ToList();
                _notification.Confirmed = true;
            }

            FinishInteraction();
        }
    }
}