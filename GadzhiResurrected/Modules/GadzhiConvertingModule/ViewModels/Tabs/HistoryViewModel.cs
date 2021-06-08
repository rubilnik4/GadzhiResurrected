using GadzhiResurrected.Infrastructure.Implementations.Services;
using GadzhiResurrected.Infrastructure.Interfaces;
using GadzhiResurrected.Infrastructure.Interfaces.Converters;
using GadzhiResurrected.Modules.GadzhiConvertingModule.ViewModels.Base;
using GadzhiResurrected.Modules.GadzhiConvertingModule.ViewModels.Tabs.HistoryViewModels;

namespace GadzhiResurrected.Modules.GadzhiConvertingModule.ViewModels.Tabs
{
    /// <summary>
    /// История
    /// </summary>
    public class HistoryViewModel : ViewModelBase
    {
        public HistoryViewModel(IDialogService dialogService, HistoryClientServiceFactory historyClientServiceFactory,
                                ConvertingClientServiceFactory convertingClientServiceFactory,
                                IConverterClientPackageDataFromDto converterClientPackageDataFromDto)
        {
            DialogService = dialogService;
            HistoryDataViewModel = new HistoryDataViewModel(historyClientServiceFactory, convertingClientServiceFactory, 
                                                            converterClientPackageDataFromDto, dialogService);
            HistoryFilterViewModel = new HistoryFilterViewModel(historyClientServiceFactory, 
                                                                HistoryDataViewModel.HistoryDataViewModelPart.UpdateHistoryData);
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