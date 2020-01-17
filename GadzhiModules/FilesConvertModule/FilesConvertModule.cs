using GadzhiModules.FilesConvertModule.ViewModels;
using GadzhiModules.FilesConvertModule.Views;
using GadzhiModules.Infrastructure;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace GadzhiModules.FilesConvertModule
{
    public class FilesConvertModule : IModule
    {
        /// <summary>
        /// Инверсия управления для слоя application
        /// </summary>
        IUnityContainer _container; //используется для подключения injection property [Dependency]

        public FilesConvertModule(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Привязка модулей к View
        /// </summary>
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionNames.FilesConvertModule, typeof(FilesConvertView));
        }

        /// <summary>
        /// Привязка View к ViewModels
        /// </summary>
        private void ConfigureViewModelLocator()
        {
            ViewModelLocationProvider.Register<FilesConvertView, FilesConvertViewModel>();
        }

        /// <summary>
        /// Инверсия управления для сервисов
        /// </summary>
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            _container.RegisterType<IApplicationGadzhi, ApplicationGadzhi>();
        }
    }
}
