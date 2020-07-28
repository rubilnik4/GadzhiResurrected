using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ChannelAdam.ServiceModel;
using GadzhiCommon.Infrastructure.Implementations.Services;
using GadzhiDTOClient.Contracts.FilesConvert;
using GadzhiDTOClient.Contracts.Signatures;
using GadzhiModules.Infrastructure.Interfaces.Services;

namespace GadzhiModules.Infrastructure.Implementations.Services
{
    /// <summary>
    /// Фабрика для создания сервисов WCF
    /// </summary>
    public class WcfClientServicesFactory : IWcfClientServicesFactory
    {
        public WcfClientServicesFactory(Func<IServiceConsumer<IFileConvertingClientService>> getConvertingService,
                                        Func<IServiceConsumer<ISignatureClientService>> getSignatureService)
        {
            if (getConvertingService == null) throw new ArgumentNullException(nameof(getConvertingService));
            if (getSignatureService == null) throw new ArgumentNullException(nameof(getSignatureService));

            ConvertingClientServiceFactory = new ConvertingClientServiceFactory(getConvertingService, RetryServiceDefault);
            SignatureClientServiceFactory = new SignatureClientServiceFactory(getSignatureService);
        }

        /// <summary>
        /// Количество повторов при разрыве соединения
        /// </summary>
        public const int RETRY_COUNT = 3;

        /// <summary>
        /// Фабрика для создания сервиса конвертации
        /// </summary>
        public ConvertingClientServiceFactory ConvertingClientServiceFactory { get; }

        /// <summary>
        /// Фабрика для создания сервиса подписей
        /// </summary>
        public SignatureClientServiceFactory SignatureClientServiceFactory { get; }

        /// <summary>
        /// Параметры повторных подключений для серверной части
        /// </summary>
        public static RetryService RetryServiceDefault => new RetryService(RETRY_COUNT);

        #region IDisposable Support
        private bool _disposedValue;

        private void Dispose(bool disposing)
        {
            if (_disposedValue) return;

            if (disposing)
            {

            }
            ConvertingClientServiceFactory?.Dispose();
            SignatureClientServiceFactory?.Dispose();
            _disposedValue = true;
        }

        ~WcfClientServicesFactory()
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