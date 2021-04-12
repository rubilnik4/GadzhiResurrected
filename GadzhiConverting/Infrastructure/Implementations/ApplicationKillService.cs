using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Models.Implementations.Functional;
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiMicrostation.Factory;
using GadzhiWord.Factory;
using static GadzhiCommon.Infrastructure.Implementations.ExecuteAndCatchErrors;

namespace GadzhiConverting.Infrastructure.Implementations
{
    /// <summary>
    /// Очистка приложений при их зависании
    /// </summary>
    public class ApplicationKillService: IApplicationKillService
    {
        /// <summary>
        /// Дата и время выполнения операций
        /// </summary>
        private readonly IAccessService _accessService;

        public ApplicationKillService(IAccessService accessService)
        {
            _accessService = accessService ?? throw new ArgumentNullException(nameof(accessService));

            _timeOutApplicationSubscriptions = new CompositeDisposable();
        }

        /// <summary>
        /// Время проверки на зависания
        /// </summary>
        private const int SCAN_TIME_SECONDS = 10;

        /// <summary>
        /// Подписка на сканирование зависаний
        /// </summary>
        private readonly CompositeDisposable _timeOutApplicationSubscriptions;

        /// <summary>
        /// Запущен ли процесс сканирования
        /// </summary>
        private bool _isScanning;

        /// <summary>
        /// Начать процесс сканирования на зависания
        /// </summary>
        public void StartScan() => SubscribeToScan();

        /// <summary>
        /// Завершить процесс сканирования на зависания
        /// </summary>
        public void StopScan() => UnSubscribeToScan();

        /// <summary>
        /// Подписаться на сканирование зависаний
        /// </summary>
        private void SubscribeToScan() =>
            _timeOutApplicationSubscriptions.
            Add(Observable.
                Interval(TimeSpan.FromSeconds(SCAN_TIME_SECONDS)).
                Where(_ => !_isScanning && _accessService.IsTimeOut).
                Select(_ => ExecuteAndHandleError(KillAllApplications,
                                                  beforeMethod: () => _isScanning = true,
                                                  finallyMethod: () => _isScanning = false)).
                Subscribe());

        /// <summary>
        /// Отписаться от сканирования на зависания
        /// </summary>
        private void UnSubscribeToScan() => _timeOutApplicationSubscriptions?.Clear();

        /// <summary>
        /// Очистить приложения
        /// </summary>
        private static void KillAllApplications()
        {
            MicrostationInstance.KillAllPreviousProcess();
            WordInstance.KillAllPreviousProcess();
            ExcelInstance.KillAllPreviousProcess();
        }

        public void Dispose()
        {
            _timeOutApplicationSubscriptions?.Dispose();
        }
    }
}