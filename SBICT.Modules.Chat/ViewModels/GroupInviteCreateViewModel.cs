// <copyright file="GroupInviteCreateViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SBICT.Modules.Chat.ViewModels
{
    using System;
    using System.Collections;
    using System.Linq;
    using Prism.Commands;
    using Prism.Interactivity.InteractionRequest;
    using Prism.Mvvm;
    using SBICT.Data;

    /// <inheritdoc cref="BindableBase" />
    /// <inheritdoc cref="IInteractionRequest" />
    /// <summary>
    /// ViewModel for GroupInviteCreate.xaml.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class GroupInviteCreateViewModel : BindableBase, IInteractionRequestAware
    {
        private GroupInviteCreateNotification notification;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupInviteCreateViewModel"/> class.
        /// </summary>
        public GroupInviteCreateViewModel()
        {
            this.OkCommand = new DelegateCommand<object>(this.OnOkClick);
            this.CancelCommand = new DelegateCommand(this.OnCancelClick);
        }

        /// <summary>
        /// Gets the command raised when pressing the Ok button.
        /// </summary>
        public DelegateCommand<object> OkCommand { get; }

        /// <summary>
        /// Gets the command raised when pressing the Cancel button.
        /// </summary>
        public DelegateCommand CancelCommand { get; }

        /// <inheritdoc/>
        public Action FinishInteraction { get; set; }

        /// <inheritdoc/>
        public INotification Notification
        {
            get => this.notification;
            set
            {
                if (value is GroupInviteCreateNotification inviteCreateNotification)
                {
                    this.notification = inviteCreateNotification;
                    this.RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Raised when cancel is clicked.
        /// </summary>
        private void OnCancelClick()
        {
            if (this.notification != null)
            {
                this.notification.SelectedItems = null;
                this.notification.Confirmed = false;
            }

            this.FinishInteraction();
        }

        /// <summary>
        /// Raised when Ok is clicked.
        /// </summary>
        /// <param name="obj">Selected Items.</param>
        private void OnOkClick(object obj)
        {
            if (this.notification != null)
            {
                this.notification.SelectedItems = ((IList)obj).Cast<IUser>().ToList();
                this.notification.Confirmed = true;
            }

            this.FinishInteraction();
        }
    }
}