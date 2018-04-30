using SBICT.Modules.Chat.Views;
using Prism.Modularity;
using Prism.Regions;
using System;
using Prism.Ioc;
using Prism.Unity;
using Unity;

namespace SBICT.Modules.Chat
{
    public class ChatModule : IModule
    {
//        private IRegionManager _regionManager;
//        private readonly IUnityContainer _container;
//
//        public ChatModule(IUnityContainer container, IRegionManager regionManager)
//        {
//            _container = container;
//            _regionManager = regionManager;
//        }
//
//        public void Initialize()
//        {
//            _container.RegisterTypeForNavigation<ViewA>();
//        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            throw new NotImplementedException();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            throw new NotImplementedException();
        }
    }
}