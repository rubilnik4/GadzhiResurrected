using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Models.Interfaces.ServerStates;

namespace GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Implementations.ServerStates
{
    /// <summary>
    /// Информация о серверах
    /// </summary>
    public class ServersInfoClient : IServersInfo
    {
        public ServersInfoClient(IEnumerable<string> serverNames, int completePackages,
                                 int completeFiles, int queuePackages, int queueFiles)
        {
            ServerNames = serverNames.ToList();
            CompletePackages = completePackages;
            CompleteFiles = completeFiles;
            QueuePackages = queuePackages;
            QueueFiles = queueFiles;
        }

        /// <summary>
        /// Наименования серверов
        /// </summary>
        public IReadOnlyCollection<string> ServerNames { get; }

        /// <summary>
        /// Готовые пакеты
        /// </summary>
        public int CompletePackages { get; }

        /// <summary>
        /// Готовые пакеты
        /// </summary>
        public int CompleteFiles { get; }

        /// <summary>
        /// Готовые пакеты
        /// </summary>
        public int QueuePackages { get; }

        /// <summary>
        /// Готовые пакеты
        /// </summary>
        public int QueueFiles { get; }
    }
}