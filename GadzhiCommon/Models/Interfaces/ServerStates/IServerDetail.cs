using System;
using GadzhiCommon.Models.Enums.ServerStates;

namespace GadzhiCommon.Models.Interfaces.ServerStates
{
    /// <summary>
    /// Подробная информация о сервере
    /// </summary>
    public interface IServerDetail<out TQueue>
        where TQueue: IServerDetailQueue
    {
        /// <summary>
        /// Имя сервера
        /// </summary>
        string ServerName { get; }

        /// <summary>
        /// Статус работы сервера
        /// </summary>
        ServerActivityType ServerActivityType { get; }

        /// <summary>
        /// Время последней активности
        /// </summary>
        DateTime? LastAccess { get; }

        /// <summary>
        /// Информация о очереди на сервере
        /// </summary>
        TQueue ServerDetailQueue { get; }
    }
}