using System;
using System.Runtime.Serialization;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Interfaces.LibraryData.Histories;

namespace GadzhiDTOBase.TransferModels.Histories
{
    /// <summary>
    /// Данные истории. Трансферная модель
    /// </summary>
    [DataContract]
    public class HistoryDataResponse: IHistoryData
    {
        public HistoryDataResponse(Guid packageId, DateTime creationDateTime, string clientName,
                                   StatusProcessingProject statusProcessingProject, int filesCount)
        {
            PackageId = packageId;
            CreationDateTime = creationDateTime;
            ClientName = clientName;
            StatusProcessingProject = statusProcessingProject;
            FilesCount = filesCount;
        }

        /// <summary>
        /// Идентификатор пакета
        /// </summary>
        [DataMember]
        public Guid PackageId { get; private set; }

        /// <summary>
        /// Дата пакета
        /// </summary>
        [DataMember]
        public DateTime CreationDateTime { get; private set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        [DataMember]
        public string ClientName { get; private set; }

        /// <summary>
        /// Статус обработки проекта
        /// </summary>
        [DataMember]
        public StatusProcessingProject StatusProcessingProject { get; private set; }

        /// <summary>
        /// Количество файлов
        /// </summary>
        [DataMember]
        public int FilesCount { get; private set; }
    }
}