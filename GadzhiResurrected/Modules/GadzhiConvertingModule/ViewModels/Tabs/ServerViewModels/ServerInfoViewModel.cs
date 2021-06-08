using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions.Functional.Result;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiCommon.Models.Interfaces.ServerStates;
using GadzhiResurrected.Infrastructure.Implementations.Converters.ServerStates;
using GadzhiResurrected.Infrastructure.Implementations.Services;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Implementations.ProjectSettings;
using Prism.Mvvm;

namespace GadzhiResurrected.Modules.GadzhiConvertingModule.ViewModels.Tabs.ServerViewModels
{
    /// <summary>
    /// Информация о серверах
    /// </summary>
    public class ServerInfoViewModel: BindableBase, IDisposable
    {
        public ServerInfoViewModel(ServerStateClientServiceFactory serverStateClientServiceFactory, Func<bool> isSelected)
        {
            _serverStateClientServiceFactory = serverStateClientServiceFactory;
            _isSelected = isSelected;
            ServersInfo = new ResultValue<IServersInfo>(new ErrorCommon(ErrorConvertingType.ValueNotInitialized, "Данные не загружены"));
            _subscriptions = new CompositeDisposable(SubscribeToServerTotalUpdate());
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
        /// Обработанные файлы
        /// </summary>
        private IResultValue<IServersInfo> _serversInfo;

        /// <summary>
        /// Обработанные файлы
        /// </summary>
        public IResultValue<IServersInfo> ServersInfo
        {
            get => _serversInfo;
            private set
            {
                SetProperty(ref _serversInfo, value);
                RaisePropertyChanged(nameof(ServersCount));
                RaisePropertyChanged(nameof(CompletePackages));
                RaisePropertyChanged(nameof(CompleteFiles));
                RaisePropertyChanged(nameof(QueuePackages));
                RaisePropertyChanged(nameof(QueueFiles));
            }
        }

        /// <summary>
        /// Наименования серверов
        /// </summary>
        public string ServersCount =>
             ServersInfo.OkStatus ? ServersInfo.Value.ServerNames.Count.ToString() : "Нет соединения";

        /// <summary>
        /// Готовые пакеты
        /// </summary>
        public string CompletePackages =>
             ServersInfo.OkStatus ? ServersInfo.Value.CompletePackages.ToString() : "-";

        /// <summary>
        /// Готовые пакеты
        /// </summary>
        public string CompleteFiles =>
             ServersInfo.OkStatus ? ServersInfo.Value.CompleteFiles.ToString() : "-";

        /// <summary>
        /// Готовые пакеты
        /// </summary>
        public string QueuePackages =>
             ServersInfo.OkStatus ? ServersInfo.Value.QueuePackages.ToString() : "-";

        /// <summary>
        /// Готовые пакеты
        /// </summary>
        public string QueueFiles =>
             ServersInfo.OkStatus ? ServersInfo.Value.QueueFiles.ToString() : "-";

        /// <summary>
        /// Подписаться на обновление
        /// </summary>
        private IDisposable SubscribeToServerTotalUpdate() =>
            Observable.
            Interval(TimeSpan.FromSeconds(ProjectSettings.IntervalSecondsToIntermediateResponse)).
            Where(_ => _isSelected()).
            Select(_ => Observable.FromAsync(GetServersInfoClient)).
            Concat().
            Subscribe();

        /// <summary>
        /// Получить модель информации о серверах
        /// </summary>
        private async Task<IResultValue<IServersInfo>> GetServersInfoClient() =>
             await _serverStateClientServiceFactory.UsingServiceRetry(service => service.Operations.GetServersInfo()).
             ResultValueOkAsync(ServersInfoConverter.ToClient).
             ResultVoidOkAsync(serversInfo => ServersInfo = new ResultValue<IServersInfo>(serversInfo));

        #region IDisposable
        public void Dispose()
        {
            _serverStateClientServiceFactory?.Dispose();
            _subscriptions?.Dispose();
        }
        #endregion
    }
}