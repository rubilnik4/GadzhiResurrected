using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Interfaces.Histories;
using GadzhiModules.Infrastructure.Implementations.Converters;

namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs.HistoryViewModels.HistoryViewModelItems
{
    /// <summary>
    /// Данные истории конвертации файла
    /// </summary>
    public class HistoryFileDataViewModelItem
    {
        public HistoryFileDataViewModelItem(IHistoryFileData historyFileData)
        {
            HistoryFileData = historyFileData;
        }

        /// <summary>
        /// Данные истории конвертации файла
        /// </summary>
        public IHistoryFileData HistoryFileData { get; }

        /// <summary>
        /// Путь файла
        /// </summary>
        public string FilePath => 
            HistoryFileData.FilePath;

        /// <summary>
        /// Статус
        /// </summary>
        public string StatusProcessing =>
            StatusProcessingConverter.StatusProcessingToString(HistoryFileData.StatusProcessing);

        /// <summary>
        /// Типы обработанных файлов
        /// </summary>
        public IReadOnlyCollection<string> FileExtensionTypes =>
            HistoryFileData.FileExtensionTypes.
            Select(fileType => fileType.ToString()).
            ToList();

        /// <summary>
        /// Количество ошибок
        /// </summary>
        public int ErrorCount =>
            HistoryFileData.ErrorCount;

        /// <summary>
        /// Форматы
        /// </summary>
        public string PaperSizes =>
            HistoryFileData.PaperSizes.
            Aggregate(String.Empty, (first, second) => first + "; " + second);
    }
}