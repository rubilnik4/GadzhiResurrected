using System;
using ChannelAdam.ServiceModel;
using GadzhiCommon.Infrastructure.Implementations.Services;
using GadzhiDTOClient.Contracts.FilesConvert;

namespace GadzhiResurrected.Infrastructure.Implementations.Services
{
    /// <summary>
    /// Фабрика для создания подключения к WCF сервису конвертирования для клиента
    /// </summary>
    public class ConvertingClientServiceFactory : WcfServiceFactory<IServiceConsumer<IFileConvertingClientService>>
    {
        public ConvertingClientServiceFactory(Func<IServiceConsumer<IFileConvertingClientService>> getConvertingService, RetryService retryService)
            : base(getConvertingService, retryService)
        { }

        /// <summary>
        /// Необходимость инициализации сервиса при вызове метода
        /// </summary>
        protected override bool IsInitMethodService(string methodName) =>
            nameof(IFileConvertingClientService.SendFiles) == methodName;

        /// <summary>
        /// Необходимость освобождения сервиса при вызове метода
        /// </summary>
        protected override bool IsDisposeMethodService(string methodName) =>
            nameof(IFileConvertingClientService.SetFilesDataLoadedByClient) == methodName ||
            nameof(IFileConvertingClientService.AbortConvertingById) == methodName;
    }
}