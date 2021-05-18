﻿using System;
using ChannelAdam.ServiceModel;
using GadzhiCommon.Infrastructure.Implementations.Services;
using GadzhiDTOClient.Contracts.Histories;
using GadzhiDTOClient.Contracts.Likes;

namespace GadzhiModules.Infrastructure.Implementations.Services
{
    /// <summary>
    /// Фабрика для создания подключения к WCF сервису получения лайков
    /// </summary>
    public class LikeClientServiceFactory : WcfServiceFactory<IServiceConsumer<ILikeClientService>>
    {
        public LikeClientServiceFactory(Func<IServiceConsumer<ILikeClientService>> getLikeClientService)
          : base(getLikeClientService)
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