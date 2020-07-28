using System;
using ChannelAdam.ServiceModel;
using GadzhiCommon.Infrastructure.Implementations.Services;
using GadzhiConverting.Infrastructure.Interfaces.Services;
using GadzhiDTOServer.Contracts.FilesConvert;
using GadzhiDTOServer.Contracts.Signatures;

namespace GadzhiConverting.Infrastructure.Implementations.Services
{
    /// <summary>
    /// Фабрика для создания сервисов WCF
    /// </summary>
    public class WcfServerServicesFactory : IWcfServerServicesFactory
    {
        public WcfServerServicesFactory(Func<IServiceConsumer<IFileConvertingServerService>> getConvertingService,
                                        Func<IServiceConsumer<ISignatureServerService>> getSignatureService)
        {
            if (getConvertingService == null) throw new ArgumentNullException(nameof(getConvertingService));
            if (getSignatureService == null) throw new ArgumentNullException(nameof(getSignatureService));

            ConvertingServerServiceFactory = new ConvertingServerServiceFactory(getConvertingService, RetryServiceDefault);
            SignatureServerServiceFactory = new SignatureServerServiceFactory(getSignatureService, RetryServiceDefault);
        }

        /// <summary>
        /// Количество повторов при разрыве соединения
        /// </summary>
        public const int RETRY_COUNT = 10;

        /// <summary>
        /// Фабрика для создания сервиса конвертации
        /// </summary>
        public ConvertingServerServiceFactory ConvertingServerServiceFactory { get; }

        /// <summary>
        /// Фабрика для создания сервиса подписей
        /// </summary>
        public SignatureServerServiceFactory SignatureServerServiceFactory { get; }

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
            ConvertingServerServiceFactory?.Dispose();
            SignatureServerServiceFactory?.Dispose();
            _disposedValue = true;
        }

        ~WcfServerServicesFactory()
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