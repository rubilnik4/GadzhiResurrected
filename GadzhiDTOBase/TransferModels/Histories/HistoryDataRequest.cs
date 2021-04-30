using System;
using System.Runtime.Serialization;

namespace GadzhiDTOBase.TransferModels.Histories
{
    /// <summary>
    /// Запрос на получение истории
    /// </summary>
    [DataContract]
    public class HistoryDataRequest
    {
        public HistoryDataRequest(DateTime dateTimeFrom, DateTime dateTimeTo)
            : this(dateTimeFrom, dateTimeTo, String.Empty)
        { }

        public HistoryDataRequest(DateTime dateTimeFrom, DateTime dateTimeTo, string clientName)
        {
            DateTimeFrom = dateTimeFrom;
            DateTimeTo = dateTimeTo;
            ClientName = clientName;
        }

        /// <summary>
        /// Дата начала
        /// </summary>
        [DataMember]
        public DateTime DateTimeFrom { get; private set; }

        /// <summary>
        /// Дата по
        /// </summary>
        [DataMember]
        public DateTime DateTimeTo { get; private set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        [DataMember]
        public string ClientName { get; private set; }
    }
}