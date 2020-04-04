using ChannelAdam.ServiceModel;
using GadzhiCommon.Helpers.Wcf;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiDTOClient.Contracts.FilesConvert;
using GadzhiModules.Infrastructure.Implementations;
using GadzhiModules.Infrastructure.Implementations.Converters;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Infrastructure.Interfaces.Converters;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations;
using GadzhiModules.Modules.FilesConvertModule.ViewModels;
using GadzhiModules.Modules.FilesConvertModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Unity;
using Unity;
using Unity.Lifetime;

namespace GadzhiModules.Modules.FilesConvertModule
{
    public class FilesConvertModule : IModule
    {

        public FilesConvertModule() { }     

        /// <summary>
        /// Привязка модулей к View
        /// </summary>
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();          
            regionManager.RegisterViewWithRegion(RegionNames.FilesConvertModule, typeof(FilesConvertView));
           
        }

        /// <summary>
        /// Регистрация зависимостей
        /// </summary>        
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            IUnityContainer unityContainer = containerRegistry.GetContainer();         
            var clientEndpoints = new ClientEndpoints();
            string fileConvertingEndpoint = clientEndpoints.GetEndpointByInterfaceFullPath(typeof(IFileConvertingClientService));

            unityContainer.RegisterFactory<IServiceConsumer<IFileConvertingClientService>>((unity) =>
                      ServiceConsumerFactory.Create<IFileConvertingClientService>(fileConvertingEndpoint), new ContainerControlledLifetimeManager());

            unityContainer.RegisterSingleton<IFilesData, FilesData>();
            unityContainer.RegisterSingleton<IStatusProcessingInformation, StatusProcessingInformation>();
            unityContainer.RegisterType<IFileDataProcessingStatusMark, FileDataProcessingStatusMark>();
            unityContainer.RegisterType<IExecuteAndCatchErrors, ExecuteAndCatchErrors>();
            unityContainer.RegisterType<IConverterClientFilesDataToDTO, ConverterClientFilesDataToDTO>();
            unityContainer.RegisterType<IConverterClientFilesDataFromDTO, ConverterClientFilesDataFromDTO>();
        }
    }
}
