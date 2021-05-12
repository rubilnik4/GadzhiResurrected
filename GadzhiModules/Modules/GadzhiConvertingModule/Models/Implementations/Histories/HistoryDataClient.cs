using System;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Interfaces.Histories;

namespace GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.Histories
{
    /// <summary>
    /// Данные истории. Клиентская модель
    /// </summary>
    public class HistoryDataClient : IHistoryData
    {
        public HistoryDataClient(Guid packageId, DateTime creationDateTime, string clientName,
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
        public Guid PackageId { get; }

        /// <summary>
        /// Дата пакета
        /// </summary>
        public DateTime CreationDateTime { get; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string ClientName { get; }

        /// <summary>
        /// Статус обработки проекта
        /// </summary>
        public StatusProcessingProject StatusProcessingProject { get; }

        /// <summary>
        /// Количество файлов
        /// </summary>
        public int FilesCount { get; }
    }
}