// <copyright file="ChatModule.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SBICT.Modules.Chat
{
    using Prism.Ioc;
    using Prism.Modularity;
    using Prism.Regions;
    using SBICT.Infrastructure;
    using SBICT.Infrastructure.Chat;
    using SBICT.Modules.Chat.Views;

    /// <inheritdoc />
    [ModuleDependency("SystemLogModule")]
    // ReSharper disable once UnusedMember.Global
    public class ChatModule : IModule
    {
        /// <inheritdoc/>
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IChatManager, ChatManager>();
            containerRegistry.Register<IChatWindow, ChatBase>();
            containerRegistry.Register<ChatList>();
            containerRegistry.Register<ChatWindow>();
            containerRegistry.Register<GroupInviteCreate>();
        }

        /// <inheritdoc/>
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionNames.SideTopRegion, typeof(ChatList));
            regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(ChatWindow));
        }
    }
}