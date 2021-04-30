using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Extensions.Functional.Result;
using GadzhiCommon.Models.Interfaces.LibraryData.Histories;
using GadzhiDTOBase.TransferModels.Histories;
using GadzhiModules.Infrastructure.Implementations.Converters.Histories;
using GadzhiModules.Infrastructure.Implementations.Services;
using GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs.HistoryViewModels.HistoryViewModelItems;
using Nito.Mvvm;
using Prism.Mvvm;

namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs.HistoryViewModels
{
    /// <summary>
    /// Данные поиска истории
    /// </summary>
    public class HistoryDataViewModel: BindableBase
    {
        public HistoryDataViewModel(HistoryClientServiceFactory historyClientServiceFactory)
        {
            _historyClientServiceFactory = historyClientServiceFactory;
        }

        /// <summary>
        /// Фабрика для создания подключения к WCF сервису получения истории конвертирования
        /// </summary>
        private readonly HistoryClientServiceFactory _historyClientServiceFactory;

        /// <summary>
        /// Данные истории конвертаций
        /// </summary>
        private NotifyTask<IReadOnlyCollection<HistoryDataViewModelItem>> _historyViewModelItems =
            NotifyTask.Create(Task.FromResult((IReadOnlyCollection<HistoryDataViewModelItem>)new List<HistoryDataViewModelItem>()));

        /// <summary>
        /// Данные истории конвертаций
        /// </summary>
        public NotifyTask<IReadOnlyCollection<HistoryDataViewModelItem>> HistoryViewModelItems 
        {
            get => _historyViewModelItems;
            private set => SetProperty(ref _historyViewModelItems, value);
        }
            
        /// <summary>
        /// Выбранная позиция данных истории
        /// </summary>
        public IHistoryData SelectedHistoryViewModelItem { get; set; }

        /// <summary>
        /// Обновить данные истории конвертаций
        /// </summary>
        public void UpdateHistoryData(HistoryDataRequest historyDataRequest) =>
            HistoryViewModelItems = NotifyTask.Create(GetHistoryData(historyDataRequest));

        /// <summary>
        /// Получить данные истории конвертаций
        /// </summary>
        private async Task<IReadOnlyCollection<HistoryDataViewModelItem>> GetHistoryData(HistoryDataRequest historyDataRequest) =>
            await _historyClientServiceFactory.UsingServiceRetry(service => service.Operations.GetHistoryData(historyDataRequest)).
            ResultValueOkAsync(HistoryDataConverter.ToClients).
            ResultValueOkAsync(histories => histories.Select(history => new HistoryDataViewModelItem(history))).
            WhereContinueAsync(result => result.OkStatus,
                               okFunc: result => result.Value.ToList(),
                               badFunc: result => new List<HistoryDataViewModelItem>());
           
    }
}