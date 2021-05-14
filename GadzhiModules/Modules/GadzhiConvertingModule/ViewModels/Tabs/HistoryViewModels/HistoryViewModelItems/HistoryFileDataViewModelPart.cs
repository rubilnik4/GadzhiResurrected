using System;
using System.Collections.Generic;
using Prism.Commands;
using Prism.Mvvm;

namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs.HistoryViewModels.HistoryViewModelItems
{
    /// <summary>
    /// Данные файлов поиска истории
    /// </summary>
    public class HistoryFileDataViewModelPart: BindableBase
    {
        public HistoryFileDataViewModelPart(Action showHistory)
        {
            ShowHistoryDataCommand = new DelegateCommand(showHistory);
        }
        
        /// <summary>
        /// Вернуться к истории пакетов
        /// </summary>
        public DelegateCommand ShowHistoryDataCommand { get; }

        /// <summary>
        /// Данные истории конвертации файлов
        /// </summary>
        private IReadOnlyCollection<HistoryFileDataViewModelItem> _historyFileViewModelItems =
            new List<HistoryFileDataViewModelItem>();

        /// <summary>
        /// Данные истории конвертации файлов
        /// </summary>
        public IReadOnlyCollection<HistoryFileDataViewModelItem> HistoryFileViewModelItems
        {
            get => _historyFileViewModelItems;
            set => SetProperty(ref _historyFileViewModelItems, value);
        }
    }
}