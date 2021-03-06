﻿using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Interfaces.Histories;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Interfaces.Histories;

namespace GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Implementations.Histories
{
    /// <summary>
    /// Данные истории конвертации файла на клиенте
    /// </summary>
    public class HistoryFileDataClient: IHistoryFileDataClient
    {
        public HistoryFileDataClient(string filePath, StatusProcessing statusProcessing, ColorPrintType colorPrintType,
                                     IEnumerable<IHistoryFileDataSource> historyFileDataSources, int errorCount)
        {
            FilePath = filePath;
            StatusProcessing = statusProcessing;
            ColorPrintType = colorPrintType;
            HistoryFileDataSources = historyFileDataSources.ToList();
            ErrorCount = errorCount;
        }

        /// <summary>
        /// Путь файла
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// Статус
        /// </summary>
        public StatusProcessing StatusProcessing { get; }

        /// <summary>
        /// Типы цветов для печати
        /// </summary>
        public ColorPrintType ColorPrintType { get; }

        /// <summary>
        /// Типы обработанных файлов
        /// </summary>
        public IReadOnlyCollection<IHistoryFileDataSource> HistoryFileDataSources { get; }

        /// <summary>
        /// Количество ошибок
        /// </summary>
        public int ErrorCount { get; }
    }
}