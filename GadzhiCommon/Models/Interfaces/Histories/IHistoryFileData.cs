using System.Collections.Generic;
using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiCommon.Models.Interfaces.Histories
{
    /// <summary>
    /// Данные истории конвертации файла
    /// </summary>
    public interface IHistoryFileData
    {
        /// <summary>
        /// Путь файла
        /// </summary>
        string FilePath { get; }

        /// <summary>
        /// Статус
        /// </summary>
        StatusProcessing StatusProcessing { get; }

        /// <summary>
        /// Типы обработанных файлов
        /// </summary>
        IReadOnlyCollection<FileExtensionType> FileExtensionTypes { get; }

        /// <summary>
        /// Количество ошибок
        /// </summary>
        int ErrorCount { get; }
    }
}