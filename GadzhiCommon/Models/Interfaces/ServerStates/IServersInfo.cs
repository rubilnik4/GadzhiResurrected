using System.Collections;
using System.Collections.Generic;

namespace GadzhiCommon.Models.Interfaces.ServerStates
{
    /// <summary>
    /// Информация о серверах
    /// </summary>
    public interface IServersInfo
    {
        /// <summary>
        /// Наименования серверов
        /// </summary>
        IReadOnlyCollection<string> ServerNames { get; }

        /// <summary>
        /// Готовые пакеты
        /// </summary>
        int CompletePackages { get; }

        /// <summary>
        /// Готовые пакеты
        /// </summary>
        int CompleteFiles { get; }

        /// <summary>
        /// Готовые пакеты
        /// </summary>
        int QueuePackages { get; }

        /// <summary>
        /// Готовые пакеты
        /// </summary>
        int QueueFiles { get; }
    }
}