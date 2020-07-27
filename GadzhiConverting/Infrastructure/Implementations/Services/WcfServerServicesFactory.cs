using System;
using ChannelAdam.ServiceModel;
using GadzhiConverting.Infrastructure.Interfaces.Services;
using GadzhiDTOServer.Contracts.FilesConvert;
using GadzhiDTOServer.Contracts.Signatures;

namespace GadzhiConverting.Infrastructure.Implementations.Services
{
    /// <summary>
    /// Фабрика для создания сервисов WCF
    /// </summary>
    public class WcfServerServicesFactory : IWcfServerSevicesFactory
    {
        public WcfServerServicesFactory(Func<IServiceConsumer<IFileConvertingServerService>> getConvertingService,
                                        Func<IServiceConsumer<ISignatureServerService>> getSignatureService)
        {
            if (getConvertingService == null) throw new ArgumentNullException(nameof(getConvertingService));
            if (getSignatureService == null) throw new ArgumentNullException(nameof(getSignatureService));

            ConvertingServerServiceFactory = new ConvertingServerServiceFactory(getConvertingService);
            SignatureServerServiceFactory = new SignatureServerServiceFactory(getSignatureService);
        }

        /// <summary>
        /// Фабрика для создания сервиса конвертации
        /// </summary>
        public ConvertingServerServiceFactory ConvertingServerServiceFactory { get; }

        /// <summary>
        /// Фабрика для создания сервиса подписей
        /// </summary>
        public SignatureServerServiceFactory SignatureServerServiceFactory { get; }

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