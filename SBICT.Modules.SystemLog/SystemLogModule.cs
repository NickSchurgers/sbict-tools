using Prism.Modularity;
using Prism.Regions;
using Prism.Ioc;
using SBICT.Infrastructure;
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