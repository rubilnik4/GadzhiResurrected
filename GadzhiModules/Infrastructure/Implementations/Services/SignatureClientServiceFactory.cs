using System;
using ChannelAdam.ServiceModel;
using GadzhiCommon.Infrastructure.Implementations.Services;
using GadzhiDTOClient.Contracts.Signatures;

namespace GadzhiModules.Infrastructure.Implementations.Services
{
    /// <summary>
    /// Фабрика для создания подключения к WCF сервису подписей для клиента
    /// </summary>
    public class SignatureClientServiceFactory : WcfServiceFactory<IServiceConsumer<ISignatureClientService>>
    {
        public SignatureClientServiceFactory(Func<IServiceConsumer<ISignatureClientService>> getSignatureClientService)
            : base(getSignatureClientService)
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