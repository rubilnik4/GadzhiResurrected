using System;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Interfaces.LibraryData.Histories;
using GadzhiModules.Infrastructure.Implementations.Converters;

namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs.HistoryViewModels.HistoryViewModelItems
{
    /// <summary>
    /// Данные истории конвертаций
    /// </summary>
    public class HistoryDataViewModelItem
    {
        public HistoryDataViewModelItem(IHistoryData historyData)
        {
            _historyData = historyData;
        }

        /// <summary>
        /// Данные истории конвертаций
        /// </summary>
        private readonly IHistoryData _historyData;

        /// <summary>
        /// Дата пакета
        /// </summary>
        public string CreationDateTime =>
            _historyData.CreationDateTime.ToString("G");

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string ClientName =>
            _historyData.ClientName;

        /// <summary>
        /// Статус обработки проекта
        /// </summary>
        public string StatusProcessingProject =>
            StatusProcessingProjectConverter.StatusProcessingProjectToString(_historyData.StatusProcessingProject);

        /// <summary>
        /// Количество файлов
        /// </summary>
        public int FilesCount =>
            _historyData.FilesCount;
    }
}