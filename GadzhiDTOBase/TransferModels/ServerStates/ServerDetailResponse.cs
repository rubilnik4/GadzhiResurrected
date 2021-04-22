using System;
using System.Runtime.Serialization;
using GadzhiCommon.Models.Enums.ServerStates;
using GadzhiCommon.Models.Interfaces.ServerStates;

namespace GadzhiDTOBase.TransferModels.ServerStates
{
    /// <summary>
    /// Статус работы сервера. Трансферная модель
    /// </summary>
    [DataContract]
    public class ServerDetailResponse : IServerDetail<ServerDetailQueueResponse>
    {
        public ServerDetailResponse(string serverName, DateTime? lastAccess, 
                                    ServerDetailQueueResponse serverDetailQueue, ServerActivityType serverActivityType)
        {
            ServerName = serverName;
            LastAccess = lastAccess;
            ServerDetailQueue = serverDetailQueue;
            ServerActivityType = serverActivityType;
        }

        /// <summary>
        /// Имя сервера
        /// </summary>
        [DataMember]
        public string ServerName { get; private set; }

        /// <summary>
        /// Статус работы сервера
        /// </summary>
        [DataMember]
        public ServerActivityType ServerActivityType { get; private set; }

        /// <summary>
        /// Время последней активности
        /// </summary>
        [DataMember]
        public DateTime? LastAccess { get; private set; }

        /// <summary>
        /// Информация о очереди на сервере. Трансферная модель
        /// </summary>
        [DataMember]
        public ServerDetailQueueResponse ServerDetailQueue { get; private set; }
    }
}