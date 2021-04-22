using System;
using GadzhiCommon.Models.Enums.ServerStates;
using GadzhiCommon.Models.Interfaces.ServerStates;

namespace GadzhiCommon.Models.Implementations.ServerStates
{
    /// <summary>
    /// Статус работы сервера. Базовый класс
    /// </summary>
    public abstract class ServerDetailBase<TQueue> : IServerDetail<TQueue>
        where TQueue : IServerDetailQueue
    {
        protected ServerDetailBase(string serverName, DateTime? lastAccess, TQueue serverDetailQueue)
        {
            ServerName = serverName;
            LastAccess = lastAccess;
            ServerDetailQueue = serverDetailQueue;
        }

        /// <summary>
        /// Статус работы сервера
        /// </summary>
        public abstract ServerActivityType ServerActivityType { get; }

        /// <summary>
        /// Имя сервера
        /// </summary>
        public string ServerName { get; }

        /// <summary>
        /// Время последней активности
        /// </summary>
        public DateTime? LastAccess { get; }

        /// <summary>
        /// Информация о очереди на сервере
        /// </summary>
        public TQueue ServerDetailQueue { get; }
    }
}