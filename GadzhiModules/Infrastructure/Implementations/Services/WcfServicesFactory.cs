using ChannelAdam.ServiceModel;
using GadzhiDTOClient.Contracts.FilesConvert;
using GadzhiDTOClient.Contracts.Signatures;
using GadzhiModules.Infrastructure.Interfaces.Services;
using Unity;

namespace GadzhiModules.Infrastructure.Implementations.Services
{
    /// <summary>
    /// Фабрика для создания сервисов WCF
    /// </summary>
    public class WcfServicesFactory: IWcfServicesFactory
    {
        /// <summary>
        /// Контейнер зависимостей
        /// </summary>
        private readonly IUnityContainer _container;

        public WcfServicesFactory(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Инициализировать сервис конвертации
        /// </summary>
        public IServiceConsumer<IFileConvertingClientService> GetFileConvertingServiceService() =>
            _container.Resolve<IServiceConsumer<IFileConvertingClientService>>();

        /// <summary>
        /// Инициализировать сервис для получения подписей
        /// </summary>
        public IServiceConsumer<ISignatureClientService> GetSignatureService() =>
            _container.Resolve<IServiceConsumer<ISignatureClientService>>();
    }
}