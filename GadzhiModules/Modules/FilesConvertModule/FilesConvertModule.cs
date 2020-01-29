using GadzhiModules.Modules.FilesConvertModule.Model;
using GadzhiModules.Modules.FilesConvertModule.ViewModels;
using GadzhiModules.Modules.FilesConvertModule.Views;
using GadzhiModules.Infrastructure;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using GadzhiModules.Modules.FilesConvertModule.Model.Implementations;
using GadzhiDTO.Contracts.FilesConvert;
using WcfClientProxyGenerator;
using System.ServiceModel.Configuration;
using System.Configuration;
using GadzhiDTO.Healpers;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Infrastructure.Implementations;

namespace GadzhiModules.Modules.FilesConvertModule
{
    public class FilesConvertModule : IModule
    {

        public FilesConvertModule()
        {

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
        public void ConfigureViewModelLocator()
        {
            ViewModelLocationProvider.Register<FilesConvertView, FilesConvertViewModel>();
        }

        /// <summary>
        /// Регистрация зависимостей
        /// </summary>        
        public void RegisterTypes(IContainerRegistry containerRegistry)
        { 
            IUnityContainer unityContainer = containerRegistry.GetContainer();

            var clientEndpoints = new ClientEndpoints();
            string fileConvertingEndpoint = clientEndpoints.GetEndpointByInterfaceFullPath(typeof(IFileConvertingService));
            unityContainer.RegisterFactory<IFileConvertingService>((unity) => WcfClientProxy.Create<IFileConvertingService>(
                                                                                    c => c.SetEndpoint(fileConvertingEndpoint)));
            unityContainer.RegisterSingleton<IFilesData, FilesData>();
            unityContainer.RegisterType<IFileDataProcessingStatusMark, FileDataProcessingStatusMark>();
        }
    }
}
