using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions.Functional.Result;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiResurrected.Infrastructure.Implementations.Converters.ServerStates;
using GadzhiResurrected.Infrastructure.Implementations.Services;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Implementations.ProjectSettings;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Implementations.ServerStates;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Interfaces.ServerStates;
using Prism.Mvvm;

namespace GadzhiResurrected.Modules.GadzhiConvertingModule.ViewModels.Tabs.ServerViewModels
{
    /// <summary>
    /// Статистика обработанных файлов
    /// </summary>
    public class ServerTotalViewModel : BindableBase, IDisposable
    {
        public ServerTotalViewModel(ServerStateClientServiceFactory serverStateClientServiceFactory, Func<bool> isSelected)
        {
            _serverStateClientServiceFactory = serverStateClientServiceFactory;
            _isSelected = isSelected;
            ServerCompleteFilesClient = new ResultValue<ServerCompleteFilesClient>(new ErrorCommon(ErrorConvertingType.ValueNotInitialized, "Данные не загружены"));
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
        private IResultValue<IServerCompleteFilesClient> _serverCompleteFilesClient;

        /// <summary>
        /// Обработанные файлы
        /// </summary>
        public IResultValue<IServerCompleteFilesClient> ServerCompleteFilesClient
        {
            get => _serverCompleteFilesClient;
            private set
            {
                SetProperty(ref _serverCompleteFilesClient, value);
                RaisePropertyChanged(nameof(DgnCount));
                RaisePropertyChanged(nameof(DocCount));
                RaisePropertyChanged(nameof(PdfCount));
                RaisePropertyChanged(nameof(DwgCount));
                RaisePropertyChanged(nameof(XlsCount));
                RaisePropertyChanged(nameof(TotalCount));
            }
        }

        /// <summary>
        /// Количество DGN
        /// </summary>
        public string DgnCount =>
            ServerCompleteFilesClient.OkStatus ? ServerCompleteFilesClient.Value.DgnCount.ToString() : "-";

        /// <summary>
        /// Количество DOC
        /// </summary>
        public string DocCount =>
            ServerCompleteFilesClient.OkStatus ? ServerCompleteFilesClient.Value.DocCount.ToString() : "-";

        /// <summary>
        /// Количество PDF
        /// </summary>
        public string PdfCount =>
            ServerCompleteFilesClient.OkStatus ? ServerCompleteFilesClient.Value.PdfCount.ToString() : "-";

        /// <summary>
        /// Количество DWG
        /// </summary>
        public string DwgCount =>
            ServerCompleteFilesClient.OkStatus ? ServerCompleteFilesClient.Value.DwgCount.ToString() : "-";

        /// <summary>
        /// Количество XLS
        /// </summary>
        public string XlsCount =>
            ServerCompleteFilesClient.OkStatus ? ServerCompleteFilesClient.Value.XlsCount.ToString() : "-";

        /// <summary>
        /// Количество всего
        /// </summary>
        public string TotalCount =>
            ServerCompleteFilesClient.OkStatus ? ServerCompleteFilesClient.Value.TotalCount.ToString() : "-";

        /// <summary>
        /// Подписаться на обновление
        /// </summary>
        private IDisposable SubscribeToServerTotalUpdate() =>
            Observable.
            Interval(TimeSpan.FromSeconds(ProjectSettings.IntervalSecondsToIntermediateResponse)).
            Where(_ => _isSelected()).
            Select(_ => Observable.FromAsync(GetServerCompleteFilesClient)).
            Concat().
            Subscribe();

        /// <summary>
        /// Получить модель обработанных файлов
        /// </summary>
        private async Task<IResultValue<IServerCompleteFilesClient>> GetServerCompleteFilesClient() =>
             await _serverStateClientServiceFactory.UsingServiceRetry(service => service.Operations.GetServerCompleteFiles()).
             ResultValueOkAsync(ServerCompleteFilesConverter.ToClient).
             ResultVoidOkAsync(serverCompleteFilesClient => ServerCompleteFilesClient = new ResultValue<IServerCompleteFilesClient>(serverCompleteFilesClient));

        #region IDisposable
        public void Dispose()
        {
            _serverStateClientServiceFactory?.Dispose();
            _subscriptions?.Dispose();
        }
        #endregion
    }
}