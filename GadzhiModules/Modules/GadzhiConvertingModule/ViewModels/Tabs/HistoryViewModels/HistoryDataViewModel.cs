using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Extensions.Functional.Result;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.LibraryData.Histories;
using GadzhiDTOBase.TransferModels.Histories;
using GadzhiModules.Infrastructure.Implementations.Converters.Histories;
using GadzhiModules.Infrastructure.Implementations.Services;
using GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs.HistoryViewModels.HistoryViewModelItems;
using Nito.Mvvm;
using Prism.Commands;
using Prism.Mvvm;

namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs.HistoryViewModels
{
    /// <summary>
    /// Данные поиска истории
    /// </summary>
    public class HistoryDataViewModel : BindableBase
    {
        public HistoryDataViewModel(HistoryClientServiceFactory historyClientServiceFactory,
                                    ConvertingClientServiceFactory convertingClientServiceFactory)
        {
            _historyClientServiceFactory = historyClientServiceFactory;
            _convertingClientServiceFactory = convertingClientServiceFactory;
            DownloadFilesDataCommand = new DelegateCommand(async () => await GetFilesData(),
                                                           CanDownloadFilesData);
        }

        /// <summary>
        /// Фабрика для создания подключения к WCF сервису получения истории конвертирования
        /// </summary>
        private readonly HistoryClientServiceFactory _historyClientServiceFactory;

        /// <summary>
        /// Фабрика для создания подключения к WCF сервису конвертирования для клиента
        /// </summary>
        private readonly ConvertingClientServiceFactory _convertingClientServiceFactory;

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
        /// Команда получения файлов
        /// </summary>
        public DelegateCommand DownloadFilesDataCommand { get; }

        /// <summary>
        /// Выбранная позиция данных истории
        /// </summary>
        private HistoryDataViewModelItem _selectedHistoryViewModelItem;

        /// <summary>
        /// Выбранная позиция данных истории
        /// </summary>
        public HistoryDataViewModelItem SelectedHistoryViewModelItem
        {
            get => _selectedHistoryViewModelItem;
            set
            {
                SetProperty(ref _selectedHistoryViewModelItem, value);
                DownloadFilesDataCommand.RaiseCanExecuteChanged();
            }
        }

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

        /// <summary>
        /// Получить завершенные файлы
        /// </summary>
        private async Task GetFilesData() =>
            await new ResultValue<HistoryDataViewModelItem>(SelectedHistoryViewModelItem,
                                                            new ErrorCommon(ErrorConvertingType.ValueNotInitialized,
                                                                            "Строка не выделена")).
            ResultValueOkAsync(package => _convertingClientServiceFactory.
                                          UsingServiceRetry(service => service.Operations.GetCompleteFiles(package.HistoryData.PackageId))).
            ResultValueOkAsync(package => package);

        /// <summary>
        /// Доступность загрузки данных
        /// </summary>
        private bool CanDownloadFilesData() =>
            SelectedHistoryViewModelItem != null &&
            CheckStatusProcessing.CompletedStatusProcessingProject.Contains(SelectedHistoryViewModelItem.HistoryData.StatusProcessingProject);
    }
}