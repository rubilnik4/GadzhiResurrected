using System;
using ChannelAdam.ServiceModel;
using GadzhiCommon.Infrastructure.Implementations.Services;
using GadzhiDTOClient.Contracts.FilesConvert;

namespace GadzhiModules.Infrastructure.Implementations.Services
{
    /// <summary>
    /// Фабрика для создания подключения у WCF сервису конвертирования для клиента
    /// </summary>
    public class ConvertingClientServiceFactory : WcfServiceFactory<IServiceConsumer<IFileConvertingClientService>>
    {
        public ConvertingClientServiceFactory(Func<IServiceConsumer<IFileConvertingClientService>> getConvertingService)
            :base(getConvertingService)
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