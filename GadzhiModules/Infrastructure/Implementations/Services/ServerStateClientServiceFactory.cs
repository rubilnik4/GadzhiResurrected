using System;
using ChannelAdam.ServiceModel;
using GadzhiCommon.Infrastructure.Implementations.Services;
using GadzhiDTOClient.Contracts.ServerStates;
using GadzhiDTOClient.Contracts.Signatures;

namespace GadzhiModules.Infrastructure.Implementations.Services
{
    /// <summary>
    /// Фабрика для создания подключения к WCF сервису состояния сервера
    /// </summary>
    public class ServerStateClientServiceFactory : WcfServiceFactory<IServiceConsumer<IServerStateClientService>>
    {
        public ServerStateClientServiceFactory(Func<IServiceConsumer<IServerStateClientService>> getServerStateClientService)
            : base(getServerStateClientService)
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