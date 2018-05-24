namespace SBICT.Modules.SystemLog
{
    using Prism.Ioc;
    using Prism.Modularity;
    using Prism.Regions;
    using SBICT.Infrastructure;
    using SBICT.Modules.SystemLog.Views;

    /// <inheritdoc />
    // ReSharper disable once UnusedMember.Global
    public class SystemLogModule : IModule
    {
        /// <inheritdoc/>
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<SystemLogWindow>();
        }

        /// <inheritdoc/>
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionNames.BottomRegion, typeof(SystemLogWindow));
        }
    }
}