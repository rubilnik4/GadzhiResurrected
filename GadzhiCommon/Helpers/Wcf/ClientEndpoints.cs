﻿using System;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Configuration;

namespace GadzhiCommon.Helpers.Wcf
{
    /// <summary>
    /// Класс для получения точек подключения
    /// </summary>
    public class ClientEndpoints
    {
        private readonly ClientSection _clientSection =
            ConfigurationManager.GetSection("system.serviceModel/client") as ClientSection;

        /// <summary>
        /// Получить точку подключения через полное имя интерфейса
        /// </summary>
        public string GetEndpointByInterfaceFullPath(Type interfaceType) =>
            _clientSection?.Endpoints.
                            Cast<ChannelEndpointElement>().
                            FirstOrDefault(endPoint => endPoint.Contract == interfaceType.ToString())?.
                            Name
            ?? String.Empty;
    }
}
