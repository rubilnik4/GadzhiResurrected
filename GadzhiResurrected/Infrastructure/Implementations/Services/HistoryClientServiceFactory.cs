using System;
using ChannelAdam.ServiceModel;
using GadzhiCommon.Infrastructure.Implementations.Services;
using GadzhiDTOClient.Contracts.Histories;

namespace GadzhiResurrected.Infrastructure.Implementations.Services
{
    /// <summary>
    /// Фабрика для создания подключения к WCF сервису получения истории конвертирования
    /// </summary>
    public class HistoryClientServiceFactory : WcfServiceFactory<IServiceConsumer<IHistoryClientService>>
    {
        public HistoryClientServiceFactory(Func<IServiceConsumer<IHistoryClientService>> getHistoryClientService)
          : base(getHistoryClientService)
        { }

        /// <summary>
        /// Необходимость инициализации сервиса при вызове метода
        /// </summary>
        protected override bool IsInitMethodService(string methodName) => true;

        /// <summary>
        /// Необходимость освобождения сервиса при вызове метода
        /// </summary>
        protected override bool IsDisposeMethodService(string methodName) => true;
    }
}