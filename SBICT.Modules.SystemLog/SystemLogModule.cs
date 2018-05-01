using Prism.Modularity;
using Prism.Regions;
using System;
using Prism.Ioc;
using Prism.Unity;
using SBICT.Infrastructure.Enums;
using SBICT.Modules.SystemLog.Views;

namespace SBICT.Modules.SystemLog
{
    public class SystemLogModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<SystemLogWindow>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionNames.BottomRegion, typeof(SystemLogWindow));
        }
    }
}