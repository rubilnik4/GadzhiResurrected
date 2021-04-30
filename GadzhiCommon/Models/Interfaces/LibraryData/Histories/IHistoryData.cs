using System;
using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiCommon.Models.Interfaces.LibraryData.Histories
{
    /// <summary>
    /// Данные истории конвертаций
    /// </summary>
    public interface IHistoryData
    {
        /// <summary>
        /// Идентификатор пакета
        /// </summary>
        Guid PackageId { get; }

        /// <summary>
        /// Дата пакета
        /// </summary>
        DateTime CreationDateTime { get; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        string ClientName { get; }

        /// <summary>
        /// Статус обработки проекта
        /// </summary>
        StatusProcessingProject StatusProcessingProject { get; }

        /// <summary>
        /// Количество файлов
        /// </summary>
        int FilesCount { get; }
    }
}