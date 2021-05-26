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
            HistoryFileData = historyFileData;
        }

        /// <summary>
        /// Данные истории конвертации файла
        /// </summary>
        public IHistoryFileDataClient HistoryFileData { get; }

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
        /// Количество ошибок
        /// </summary>
        public int ErrorCount =>
            HistoryFileData.ErrorCount;
    }
}