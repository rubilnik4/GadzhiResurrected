using System;
using System.Threading.Tasks;
using ChannelAdam.ServiceModel;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiDTOClient.Contracts.FilesConvert;
using GadzhiDTOClient.Contracts.Signatures;
using GadzhiModules.Infrastructure.Interfaces.Services;
using Unity;
using static GadzhiCommon.Infrastructure.Implementations.ExecuteAndCatchErrors;

namespace GadzhiModules.Infrastructure.Implementations.Services
{
    /// <summary>
    /// Фабрика для создания сервисов WCF
    /// </summary>
    public class WcfServicesFactory : IWcfServicesFactory
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

        /// <summary>
        /// Выполнить функцию для сервиса подписей и проверить на ошибки
        /// </summary>
        public async Task<IResultValue<TResult>> UsingSignatureServiceAsync<TResult>(Func<IServiceConsumer<ISignatureClientService>, Task<TResult>> signatureFunc)
        {
            using var signatureService = GetSignatureService();
            return await ExecuteAndHandleErrorAsync(() => signatureFunc(signatureService));
        }
    }
}