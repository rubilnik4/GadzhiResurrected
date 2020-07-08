using System;
using System.Threading.Tasks;
using ChannelAdam.ServiceModel;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Infrastructure.Implementations.Logger;
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
    public class WcfServicesFactory : IWcfServicesFactory, IDisposable
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
        /// Сервис конвертации
        /// </summary>
        private IServiceConsumer<IFileConvertingClientService> _signatureService;

        /// <summary>
        /// Инициализировать сервис конвертации
        /// </summary>
        private IServiceConsumer<IFileConvertingClientService> GetFileConvertingServiceService() =>
            _signatureService ??= _container.Resolve<IServiceConsumer<IFileConvertingClientService>>();

        /// <summary>
        /// Инициализировать сервис для получения подписей
        /// </summary>
        public IServiceConsumer<ISignatureClientService> GetSignatureService() =>
            _container.Resolve<IServiceConsumer<ISignatureClientService>>();

        /// <summary>
        /// Очистить сервис конвертации
        /// </summary>
        public void SignatureServiceDispose() => _signatureService?.Dispose();

        /// <summary>
        /// Выполнить функцию для сервиса конвертации и проверить на ошибки
        /// </summary>
        public async Task<IResultValue<TResult>> UsingConvertingServiceAsync<TResult>(Func<IServiceConsumer<IFileConvertingClientService>, Task<TResult>> fileConvertingFunc) =>
            await GetFileConvertingServiceService().
            Map(fileConvertingService => ExecuteAndHandleErrorAsync(() => fileConvertingFunc(fileConvertingService)));

        /// <summary>
        /// Выполнить функцию для сервиса подписей и проверить на ошибки
        /// </summary>
        public async Task<IResultValue<TResult>> UsingSignatureServiceAsync<TResult>(Func<IServiceConsumer<ISignatureClientService>, Task<TResult>> signatureFunc)
        {
            using var signatureService = GetSignatureService();
            return await ExecuteAndHandleErrorAsync(() => signatureFunc(signatureService));
        }

        #region IDisposable Support
        private bool _disposedValue;

        [Logger]
        private void Dispose(bool disposing)
        {
            if (_disposedValue) return;

            if (disposing)
            {

            }
            _signatureService?.Dispose();
            _signatureService = null;
            _disposedValue = true;
        }

        ~WcfServicesFactory()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}