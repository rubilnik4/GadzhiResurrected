using System;
using GadzhiCommon.Models.Enums.ServerStates;
using GadzhiCommon.Models.Implementations.ServerStates;
using GadzhiCommon.Models.Interfaces.ServerStates;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.ServerStates;

namespace GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Implementations.ServerStates
{
    /// <summary>
    /// Статус работы сервера. Клиентский класс
    /// </summary>
    public class ServerDetailClient : ServerDetailBase<IServerDetailQueue>, IServerDetailClient
    {
        public ServerDetailClient(string serverName, DateTime? lastAccess, IServerDetailQueue serverDetailQueue, 
                                  ServerActivityType serverActivityType)
                : base(serverName, lastAccess, serverDetailQueue)
        {
            ServerActivityType = serverActivityType;
        }

        /// <summary>
        /// Статус работы сервера
        /// </summary>
        public override ServerActivityType ServerActivityType { get; }
    }
}