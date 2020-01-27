using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiDTO.Healpers
{
    /// <summary>
    /// Класс для получения точек подключения
    /// </summary>
    public class ClientEndpoints
    {
        private ClientSection _clientSection = ConfigurationManager.GetSection("system.serviceModel/client") as ClientSection;

        /// <summary>
        /// Получить точку подключения через полное имя интерфейса
        /// </summary>
        public string GetEndpointByInterfaceFullPath(Type interfaceType)
        {
            return _clientSection?.
                                        Endpoints.
                                        Cast<ChannelEndpointElement>()?.
                                        FirstOrDefault(endpont => endpont.Contract == interfaceType.ToString());
        }

    }
}
