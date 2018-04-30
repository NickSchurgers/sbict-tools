using SBICT.Module.SystemLog.Views;
using Prism.Modularity;
using Prism.Regions;
using System;
using Microsoft.Practices.Unity;
using Prism.Unity;

namespace SBICT.Module.SystemLog
{
    public class SystemLogModule : IModule
    {
        private IRegionManager _regionManager;
        private IUnityContainer _container;

        public SystemLogModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterTypeForNavigation<ViewA>();
        }
    }
}