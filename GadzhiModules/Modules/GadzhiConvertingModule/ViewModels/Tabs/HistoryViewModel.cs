using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiModules.Infrastructure.Implementations.Converters.Histories;
using GadzhiModules.Infrastructure.Implementations.Services;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Base;
using GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs.HistoryViewModels;

namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs
{
    /// <summary>
    /// История
    /// </summary>
    public class HistoryViewModel : ViewModelBase
    {
        public HistoryViewModel(IDialogService dialogService,
                                HistoryClientServiceFactory historyClientServiceFactory,
                                ConvertingClientServiceFactory convertingClientServiceFactory)
        {
            DialogService = dialogService;
            HistoryDataViewModel = new HistoryDataViewModel(historyClientServiceFactory, convertingClientServiceFactory);
            HistoryFilterViewModel = new HistoryFilterViewModel(historyClientServiceFactory, HistoryDataViewModel.UpdateHistoryData);
        }
        /// <summary>
        /// Стандартные диалоговые окна
        /// </summary>
        protected override IDialogService DialogService { get; }

        /// <summary>
        /// Название
        /// </summary>
        public override string Title => "История";

        /// <summary>
        /// Выбор вкладки
        /// </summary>
        private bool _isSelected;

        /// <summary>
        /// Выбор вкладки
        /// </summary>
        public override bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                HistoryFilterViewModel.OnInitialize();
            }
        }

        /// <summary>
        /// Фильтры поиска в истории
        /// </summary>
        public HistoryFilterViewModel HistoryFilterViewModel { get; }

        /// <summary>
        /// Данные поиска истории
        /// </summary>
        public HistoryDataViewModel HistoryDataViewModel { get; }

        #region IDisposable Support
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            HistoryFilterViewModel.Dispose();
        }
        #endregion
    }
}