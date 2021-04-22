using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using GadzhiCommon.Models.Interfaces.ServerStates;

namespace GadzhiDTOBase.TransferModels.ServerStates
{
    /// <summary>
    /// Информация о серверах
    /// </summary>
    [DataContract]
    public class ServersInfoResponse: IServersInfo
    {
        public ServersInfoResponse(IEnumerable<string> serverNames, int completePackages, 
                                   int completeFiles, int queuePackages, int queueFiles)
        {
            ServerNamesList = serverNames.ToList();
            CompletePackages = completePackages;
            CompleteFiles = completeFiles;
            QueuePackages = queuePackages;
            QueueFiles = queueFiles;
        }

        /// <summary>
        /// Наименования серверов
        /// </summary>
        [IgnoreDataMember]
        public IReadOnlyCollection<string> ServerNames => 
            ServerNamesList;

        /// <summary>
        /// Наименования серверов
        /// </summary>
        [DataMember]
        public List<string> ServerNamesList { get; private set; }

        /// <summary>
        /// Готовые пакеты
        /// </summary>
        [DataMember]
        public int CompletePackages { get; private set; }

        /// <summary>
        /// Готовые пакеты
        /// </summary>
        [DataMember]
        public int CompleteFiles { get; private set; }

        /// <summary>
        /// Готовые пакеты
        /// </summary>
        [DataMember]
        public int QueuePackages { get; private set; }

        /// <summary>
        /// Готовые пакеты
        /// </summary>
        [DataMember]
        public int QueueFiles { get; private set; }
    }
}