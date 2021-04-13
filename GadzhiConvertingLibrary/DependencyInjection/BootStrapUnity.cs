using ChannelAdam.ServiceModel;
using GadzhiCommon.Helpers.Wcf;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiConvertingLibrary.Infrastructure.Implementations;
using GadzhiConvertingLibrary.Infrastructure.Implementations.Services;
using GadzhiConvertingLibrary.Infrastructure.Interfaces;
using GadzhiConvertingLibrary.Infrastructure.Interfaces.Converters;
using GadzhiConvertingLibrary.Infrastructure.Interfaces.Services;
using GadzhiDTOServer.Contracts.FilesConvert;
using GadzhiDTOServer.Contracts.Signatures;
using Unity;
using Unity.Lifetime;

namespace GadzhiConvertingLibrary.DependencyInjection
{
    /// <summary>
    /// Класс для регистрации зависимостей
    /// </summary>
    public static class BootStrapUnity
    {
        /// <summary>
        /// Время ожидания
        /// </summary>
        public const int TIME_OUT_MINUTES_OPERATION = 3;

        /// <summary>
        /// Регистрация WCF сервисов
        /// </summary>
        public static void RegisterServices(IUnityContainer container)
        {
            var clientEndpoints = new ClientEndpoints();
            string fileConvertingEndpoint = clientEndpoints.GetEndpointByInterfaceFullPath(typeof(IFileConvertingServerService));
            container.RegisterFactory<IAccessService>(unity => new AccessService(TIME_OUT_MINUTES_OPERATION));

            container.RegisterFactory<IServiceConsumer<IFileConvertingServerService>>(unity =>
                ServiceConsumerFactory.Create<IFileConvertingServerService>(fileConvertingEndpoint));

            string signatureEndpoint = clientEndpoints.GetEndpointByInterfaceFullPath(typeof(ISignatureServerService));
            container.RegisterFactory<IServiceConsumer<ISignatureServerService>>(unity =>
                ServiceConsumerFactory.Create<ISignatureServerService>(signatureEndpoint));

            container.RegisterFactory<IWcfServerServicesFactory>(unity =>
                new WcfServerServicesFactory(() => unity.Resolve<IServiceConsumer<IFileConvertingServerService>>(),
                                             () => unity.Resolve<IServiceConsumer<ISignatureServerService>>()), 
                                                                      new SingletonLifetimeManager());
        }
    }
}
