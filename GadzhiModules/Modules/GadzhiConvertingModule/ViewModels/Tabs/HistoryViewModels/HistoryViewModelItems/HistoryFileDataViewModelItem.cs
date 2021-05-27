using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Interfaces.Histories;
using GadzhiModules.Infrastructure.Implementations.Converters;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.Histories;

namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs.HistoryViewModels.HistoryViewModelItems
{
    /// <summary>
    /// Данные истории конвертации файла
    /// </summary>
    public class HistoryFileDataViewModelItem
    {
        public HistoryFileDataViewModelItem(IHistoryFileDataClient historyFileData)
        {
            _historyFileData = historyFileData;
        }

        /// <summary>
        /// Данные истории конвертации файла
        /// </summary>
        private readonly IHistoryFileDataClient _historyFileData;

        /// <summary>
        /// Отконвертированные файлы
        /// </summary>
        public IReadOnlyCollection<HistoryFileDataSourceViewModelItem> HistoryFileDataSourceViewModelItems =>
            _historyFileData.HistoryFileDataSources.
            Select(source => new HistoryFileDataSourceViewModelItem(source)).
            ToList();

        /// <summary>
        /// Путь файла
        /// </summary>
        public string FilePath =>
            _historyFileData.FilePath;

        /// <summary>
        /// Статус
        /// </summary>
        public string StatusProcessing =>
            StatusProcessingConverter.StatusProcessingToString(_historyFileData.StatusProcessing);

        /// <summary>
        /// Типы цветов для печати
        /// </summary>
        public string ColorPrintType =>
            ColorPrintConverter.ColorPrintToString(_historyFileData.ColorPrintType);

        /// <summary>
        /// Количество ошибок
        /// </summary>
        public int ErrorCount =>
            _historyFileData.ErrorCount;
    }
}