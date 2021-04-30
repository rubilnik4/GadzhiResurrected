using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Extensions.Functional;
using GadzhiDTOBase.TransferModels.Histories;
using GadzhiModules.Infrastructure.Implementations.Converters.Histories;
using GadzhiModules.Infrastructure.Implementations.Services;
using Nito.Mvvm;
using Prism.Commands;
using Prism.Mvvm;

namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs.HistoryViewModels
{
    /// <summary>
    /// Фильтры поиска в истории
    /// </summary>
    public class HistoryFilterViewModel : BindableBase, IDisposable
    {
        public HistoryFilterViewModel(HistoryClientServiceFactory historyClientServiceFactory,
                                      Action<HistoryDataRequest> historySearch)
        {
            _historyClientServiceFactory = historyClientServiceFactory;
            HistorySearchCommand = new DelegateCommand(() => historySearch(GetHistoryDataRequest()));
        }

        /// <summary>
        /// Действия при загрузке
        /// </summary>
        public void OnInitialize()
        {
            ClientNames ??= NotifyTask.Create(GetClientNames);
        }

        /// <summary>
        /// Выбрать всех пользователей
        /// </summary>
        public const string FILTER_CLIENTS_ALL = "Все";

        /// <summary>
        /// Фабрика для создания подключения к WCF сервису получения истории конвертирования
        /// </summary>
        private readonly HistoryClientServiceFactory _historyClientServiceFactory;

        /// <summary>
        /// Команда поиска
        /// </summary>
        public DelegateCommand HistorySearchCommand { get; }

        /// <summary>
        /// Типы режимов историй
        /// </summary>
        public IReadOnlyCollection<string> HistoryTypes =>
            HistoryTypeConverter.HistoriesString;

        /// <summary>
        /// Текущий режим типа истории
        /// </summary>
        public string SelectedHistoryType { get; set; } =
            HistoryTypeConverter.HistoriesString.FirstOrDefault();

        /// <summary>
        /// Типы расширений
        /// </summary>
        public IReadOnlyCollection<string> FileExtensionTypes =>
            ConvertingFileTypeConverter.FileExtensionsString;

        /// <summary>
        /// Текущее расширение
        /// </summary>
        public string SelectedFileExtensionType { get; set; } =
            ConvertingFileTypeConverter.FileExtensionsString.FirstOrDefault();

        /// <summary>
        /// Дата с
        /// </summary>
        public DateTime DateTimeFrom { get; set; } = DateTimeNow;

        /// <summary>
        /// Дата по
        /// </summary>
        public DateTime DateTimeTo { get; set; } = DateTimeNow;

        /// <summary>
        /// Список серверов
        /// </summary>
        public NotifyTask<IReadOnlyCollection<string>> ClientNames { get; private set; }

        /// <summary>
        /// Выбранное имя пользователя
        /// </summary>
        private string _selectedClientName;

        /// <summary>
        /// Выбранное имя пользователя
        /// </summary>
        public string SelectedClientName
        {
            get => _selectedClientName;
            set => SetProperty(ref _selectedClientName, value);
        }

        /// <summary>
        /// Текущее время
        /// </summary>
        public static DateTime DateTimeNow =>
            DateTime.Now;

        /// <summary>
        /// Получить список пользователей
        /// </summary>
        private async Task<IReadOnlyCollection<string>> GetClientNames() =>
            await _historyClientServiceFactory.UsingServiceRetry(service => service.Operations.GetClientNames()).
            WhereContinueAsync(result => result.OkStatus,
                               okFunc: result => result.Value.ToList(),
                               badFunc: result => new List<string>()).
            MapAsync(names => new List<string> { FILTER_CLIENTS_ALL }.Concat(names).ToList()).
            VoidAsync(names => SelectedClientName = names.First());

        /// <summary>
        /// Получить запрос поиска истории
        /// </summary>
        private HistoryDataRequest GetHistoryDataRequest() =>
            HistoryDataConverter.ToHistoryDataRequest(DateTimeFrom, DateTimeTo, SelectedClientName);

        #region IDisposable
        public void Dispose()
        {
            _historyClientServiceFactory?.Dispose();
        }
        #endregion
    }
}