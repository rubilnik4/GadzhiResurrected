using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Extensions.Functional.Result;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiModules.Infrastructure.Implementations.Converters;
using GadzhiModules.Infrastructure.Implementations.Converters.ServerStates;
using GadzhiModules.Infrastructure.Implementations.Services;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.ProjectSettings;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.ServerStates;
using Nito.Mvvm;
using Prism.Mvvm;

namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs.ServerViewModels
{
    /// <summary>
    /// Подробная информация о сервере
    /// </summary>
    public class ServerDetailViewModel : BindableBase, IDisposable
    {
        public ServerDetailViewModel(ServerStateClientServiceFactory serverStateClientServiceFactory, Func<bool> isSelected)
        {
            _serverStateClientServiceFactory = serverStateClientServiceFactory;
            _isSelected = isSelected;
            _subscriptions = new CompositeDisposable(SubscribeToServerDetailUpdate());
        }

        /// <summary>
        /// Действия при загрузке
        /// </summary>
        public void OnInitialize()
        {
            ServerNames ??= NotifyTask.Create(GetServerNames);
        }

        /// <summary>
        /// Фабрика для создания подключения к WCF сервису состояния сервера
        /// </summary>
        private readonly ServerStateClientServiceFactory _serverStateClientServiceFactory;

        /// <summary>
        /// Отрыта ли форма
        /// </summary>
        private readonly Func<bool> _isSelected;

        /// <summary>
        /// Подписки
        /// </summary>
        private readonly CompositeDisposable _subscriptions;

        /// <summary>
        /// Список серверов
        /// </summary>
        public NotifyTask<IReadOnlyCollection<string>> ServerNames { get; private set; }

        /// <summary>
        /// Выбранный сервер
        /// </summary>
        private string _selectedServerName;

        /// <summary>
        /// Выбранный сервер
        /// </summary>
        public string SelectedServerName
        {
            get => _selectedServerName;
            set
            {
                SetProperty(ref _selectedServerName, value);
                IsDetailLoading = true;
                GetServerDetail(_selectedServerName).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Загрузка данных сервера
        /// </summary>
        private bool _isDetailLoading;

        /// <summary>
        /// Загрузка данных сервера
        /// </summary>
        public bool IsDetailLoading
        {
            get => _isDetailLoading;
            private set
            {
                SetProperty(ref _isDetailLoading, value);
                RaisePropertyChanged(nameof(IsDetailLoading));
                RaisePropertyChanged(nameof(IsDetailLoaded));
            }
        }

        /// <summary>
        /// Загрузка данных сервера
        /// </summary>
        public bool IsDetailLoaded => !IsDetailLoading;

        /// <summary>
        /// Обработанные файлы
        /// </summary>
        private IResultValue<IServerDetailClient> _serverDetail;

        /// <summary>
        /// Обработанные файлы
        /// </summary>
        public IResultValue<IServerDetailClient> ServerDetail
        {
            get => _serverDetail;
            private set
            {
                SetProperty(ref _serverDetail, value);
                RaisePropertyChanged(nameof(Status));
                RaisePropertyChanged(nameof(LastAccess));
                RaisePropertyChanged(nameof(CurrentUser));
                RaisePropertyChanged(nameof(CurrentPackage));
                RaisePropertyChanged(nameof(CurrentFile));
                RaisePropertyChanged(nameof(FilesInQueue));
                RaisePropertyChanged(nameof(PackagesComplete));
                RaisePropertyChanged(nameof(FilesComplete));
            }
        }

        /// <summary>
        /// Статус сервера
        /// </summary>
        public string Status =>
             ServerDetail?.OkStatus == true
                 ? ServerActivityConverter.ServerActivityString[ServerDetail.Value.ServerActivityType]
                 : "-";

        /// <summary>
        /// Последнее время активности
        /// </summary>
        public string LastAccess =>
             ServerDetail?.OkStatus == true
                 ? ServerDetail.Value.LastAccess?.ToString("G") ?? "-"
                 : "-";

        /// <summary>
        /// Текущий пользователь
        /// </summary>
        public string CurrentUser =>
             ServerDetail?.OkStatus == true &&
             !String.IsNullOrWhiteSpace(ServerDetail.Value.ServerDetailQueue.CurrentUser)
                 ? ServerDetail.Value.ServerDetailQueue.CurrentUser
                 : "-";

        /// <summary>
        /// Текущий пакет
        /// </summary>
        public string CurrentPackage =>
             ServerDetail?.OkStatus == true &&
             !String.IsNullOrWhiteSpace(ServerDetail.Value.ServerDetailQueue.CurrentPackage)
                 ? ServerDetail.Value.ServerDetailQueue.CurrentPackage
                 : "-";

        /// <summary>
        /// Текущий файл
        /// </summary>
        public string CurrentFile =>
             ServerDetail?.OkStatus == true &&
             !String.IsNullOrWhiteSpace(ServerDetail.Value.ServerDetailQueue.CurrentFile)
                 ? ServerDetail.Value.ServerDetailQueue.CurrentFile
                 : "-";

        /// <summary>
        /// Файлов в очереди
        /// </summary>
        public string FilesInQueue =>
             ServerDetail?.OkStatus == true
                 ? ServerDetail.Value.ServerDetailQueue.FilesInQueue.ToString()
                 : "-";

        /// <summary>
        /// Обработано пакетов
        /// </summary>
        public string PackagesComplete =>
             ServerDetail?.OkStatus == true
                 ? ServerDetail.Value.ServerDetailQueue.PackagesComplete.ToString()
                 : "-";

        /// <summary>
        /// Обработано файлов
        /// </summary>
        public string FilesComplete =>
             ServerDetail?.OkStatus == true
                 ? ServerDetail.Value.ServerDetailQueue.FilesComplete.ToString()
                 : "-";

        /// <summary>
        /// Получить список серверов
        /// </summary>
        public async Task<IReadOnlyCollection<string>> GetServerNames() =>
            await _serverStateClientServiceFactory.UsingServiceRetry(service => service.Operations.GetServerNames()).
            WhereContinueAsync(result => result.OkStatus,
                               okFunc: result => result.Value.ToList(),
                               badFunc: result => new List<string>());

        /// <summary>
        /// Подписаться на обновление
        /// </summary>
        private IDisposable SubscribeToServerDetailUpdate() =>
            Observable.
            Interval(TimeSpan.FromSeconds(ProjectSettings.IntervalSecondsToIntermediateResponse)).
            Where(_ => _isSelected() && !String.IsNullOrWhiteSpace(SelectedServerName)).
            Select(_ => Observable.FromAsync(() => GetServerDetail(SelectedServerName))).
            Concat().
            Subscribe();

        /// <summary>
        /// Получить модель информацию о сервере
        /// </summary>
        private async Task<IResultValue<IServerDetailClient>> GetServerDetail(string serverName) =>
             await _serverStateClientServiceFactory.UsingServiceRetry(service => service.Operations.GetServerDetail(serverName)).
             ResultValueOkAsync(ServerDetailConverter.ToClient).
             ResultVoidOkAsync(serversDetail => ServerDetail = new ResultValue<IServerDetailClient>(serversDetail)).
             ResultVoidAsync(_ => IsDetailLoading = false);

        #region IDisposable
        public void Dispose()
        {
            _serverStateClientServiceFactory?.Dispose();
            _subscriptions?.Dispose();
        }
        #endregion
    }
}