using System.Collections.Generic;
using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiCommon.Models.Interfaces.Histories
{
    /// <summary>
    /// Данные истории конвертации файла
    /// </summary>
    public interface IHistoryFileData<out TSource>
        where TSource : IHistoryFileDataSource
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
        /// Типы цветов для печати
        /// </summary>
        ColorPrintType ColorPrintType { get; }

        /// <summary>
        /// Типы обработанных файлов
        /// </summary>
        IReadOnlyCollection<TSource> HistoryFileDataSources { get; }

        /// <summary>
        /// Количество ошибок
        /// </summary>
        int ErrorCount { get; }
    }
}