using ChannelAdam.ServiceModel;
using GadzhiCommon.Helpers.Wcf;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiDTOClient.Contracts.FilesConvert;
using GadzhiDTOClient.Contracts.Histories;
using GadzhiDTOClient.Contracts.Likes;
using GadzhiDTOClient.Contracts.ServerStates;
using GadzhiDTOClient.Contracts.Signatures;
using GadzhiModules.Modules;
using GadzhiResurrected.Infrastructure.Implementations;
using GadzhiResurrected.Infrastructure.Implementations.Converters;
using GadzhiResurrected.Infrastructure.Implementations.Services;
using GadzhiResurrected.Infrastructure.Interfaces;
using GadzhiResurrected.Infrastructure.Interfaces.ApplicationGadzhi;
using GadzhiResurrected.Infrastructure.Interfaces.Converters;
using GadzhiResurrected.Infrastructure.Interfaces.Services;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Implementations.ProjectSettings;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Interfaces.FileConverting;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Interfaces.ProjectSettings;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using Unity;
using ApplicationGadzhi = GadzhiResurrected.Infrastructure.Implementations.ApplicationGadzhi.ApplicationGadzhi;

namespace GadzhiResurrected.Modules.GadzhiConvertingModule
{
    public class GadzhiConvertingModule : IModule
    {
        /// <summary>
        /// Привязка модулей к View
        /// </summary>
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionNames.GadzhiConvertingModule, typeof(GadzhiConvertingView));
        }

        /// <summary>
        /// Регистрация зависимостей
        /// </summary>
        [Logger]
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var unityContainer = containerRegistry.GetContainer();

            RegisterServices(unityContainer);

            unityContainer.RegisterSingleton<IPackageData, PackageData>();
            unityContainer.RegisterSingleton<IStatusProcessingInformation, StatusProcessingInformation>();
            unityContainer.RegisterType<IFileDataProcessingStatusMark, FileDataProcessingStatusMark>();
            unityContainer.RegisterType<IConverterClientPackageDataToDto, ConverterClientPackageDataToDto>();
            unityContainer.RegisterType<IConverterClientPackageDataFromDto, ConverterClientPackageDataFromDto>();

            unityContainer.RegisterFactory<IProjectSettings>(unity =>
                new ProjectSettings(ApplicationGadzhi.GetConvertingSettingFromConfiguration()), FactoryLifetime.Singleton);

            var application = unityContainer.Resolve<IApplicationGadzhi>();
            unityContainer.RegisterFactory<IProjectResources>(unity => new ProjectResources(application.GetSignaturesNames()), FactoryLifetime.Singleton);
        }

        /// <summary>
        /// Регистрация WCF сервисов
        /// </summary>
        private static void RegisterServices(IUnityContainer unityContainer)
        {
            var clientEndpoints = new ClientEndpoints();

            string fileConvertingEndpoint = clientEndpoints.GetEndpointByInterfaceFullPath(typeof(IFileConvertingClientService));
            unityContainer.RegisterFactory<IServiceConsumer<IFileConvertingClientService>>(unity =>
                ServiceConsumerFactory.Create<IFileConvertingClientService>(fileConvertingEndpoint));

            string signatureEndpoint = clientEndpoints.GetEndpointByInterfaceFullPath(typeof(ISignatureClientService));
            unityContainer.RegisterFactory<IServiceConsumer<ISignatureClientService>>(unity =>
                ServiceConsumerFactory.Create<ISignatureClientService>(signatureEndpoint));

            string serverStateEndpoint = clientEndpoints.GetEndpointByInterfaceFullPath(typeof(IServerStateClientService));
            unityContainer.RegisterFactory<IServiceConsumer<IServerStateClientService>>(unity =>
                ServiceConsumerFactory.Create<IServerStateClientService>(serverStateEndpoint));

            string historyEndpoint = clientEndpoints.GetEndpointByInterfaceFullPath(typeof(IHistoryClientService));
            unityContainer.RegisterFactory<IServiceConsumer<IHistoryClientService>>(unity =>
                ServiceConsumerFactory.Create<IHistoryClientService>(historyEndpoint));

            string likeEndpoint = clientEndpoints.GetEndpointByInterfaceFullPath(typeof(ILikeClientService));
            unityContainer.RegisterFactory<IServiceConsumer<ILikeClientService>>(unity =>
                ServiceConsumerFactory.Create<ILikeClientService>(likeEndpoint));

            unityContainer.RegisterFactory<IWcfClientServicesFactory>(unity =>
                new WcfClientServicesFactory(() => unity.Resolve<IServiceConsumer<IFileConvertingClientService>>(),
                                             () => unity.Resolve<IServiceConsumer<ISignatureClientService>>(),
                                             () => unity.Resolve<IServiceConsumer<IServerStateClientService>>(),
                                             () => unity.Resolve<IServiceConsumer<IHistoryClientService>>()));

        }
    }
}
