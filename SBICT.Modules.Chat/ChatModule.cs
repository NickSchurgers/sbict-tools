using Prism.Modularity;
using Prism.Ioc;
using Prism.Regions;
using SBICT.Infrastructure;
using SBICT.Infrastructure.Chat;
using SBICT.Modules.Chat.Views;


namespace SBICT.Modules.Chat
{
    [ModuleDependency("SystemLogModule")]
    public class ChatModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IChatManager, ChatManager>();
            containerRegistry.Register<IChatWindow, ChatBase>();
            containerRegistry.Register<ChatList>();
            containerRegistry.Register<ChatWindow>();
            containerRegistry.Register<GroupInviteCreate>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionNames.SideTopRegion, typeof(ChatList));
            regionManager.RegisterViewWithRegion(RegionNames.MainRegion, typeof(ChatWindow));
        }
    }
}