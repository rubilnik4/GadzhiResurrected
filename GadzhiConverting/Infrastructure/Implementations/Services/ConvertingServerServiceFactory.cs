using System;
using ChannelAdam.ServiceModel;
using GadzhiCommon.Infrastructure.Implementations.Services;
using GadzhiDTOServer.Contracts.FilesConvert;

namespace GadzhiConverting.Infrastructure.Implementations.Services
{
    /// <summary>
    /// Фабрика для создания подключения к WCF сервису конвертации для сервера
    /// </summary>
    public class ConvertingServerServiceFactory : WcfServiceFactory<IServiceConsumer<IFileConvertingServerService>>
    {
        public ConvertingServerServiceFactory(Func<IServiceConsumer<IFileConvertingServerService>> getConvertingService, 
                                              RetryService retryService)
            : base(getConvertingService, retryService)
        { }

        /// <summary>
        /// Необходимость инициализации сервиса при вызове метода
        /// </summary>
        protected override bool IsInitMethodService(string methodName) => false;

        /// <summary>
        /// Необходимость освобождения сервиса при вызове метода
        /// </summary>
        protected override bool IsDisposeMethodService(string methodName) => false;
    }
}