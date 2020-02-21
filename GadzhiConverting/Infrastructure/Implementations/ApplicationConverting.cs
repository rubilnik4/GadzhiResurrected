using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiConverting.Infrastructure.Interfaces;
using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace GadzhiConverting.Infrastructure.Implementations
{
    /// <summary>
    /// Инфраструктура для конвертирования файлов
    /// </summary>
    public sealed class ApplicationConverting : IApplicationConverting, IDisposable
    {
        /// <summary>
        ///Контейнер зависимостей
        /// </summary>
        private readonly IConvertingService _convertingService;

        /// <summary>
        /// Параметры приложения
        /// </summary>
        private readonly IProjectSettings _projectSettings;

        /// <summary>
        /// Класс для отображения изменений и логгирования
        /// </summary>
        private readonly IMessageAndLoggingService _messageAndLoggingService;

        /// <summary>
        /// Класс обертка для отлова ошибок
        /// </summary> 
        private readonly IExecuteAndCatchErrors _executeAndCatchErrors;

        /// <summary>
        /// Запуск процесса конвертирования
        /// </summary>
        private readonly CompositeDisposable _convertingUpdaterSubsriptions;

        public ApplicationConverting(IConvertingService convertingService,
                                     IProjectSettings projectSettings,
                                     IMessageAndLoggingService messageAndLoggingService,
                                     IExecuteAndCatchErrors executeAndCatchErrors)
        {
            _convertingService = convertingService;
            _projectSettings = projectSettings;
            _messageAndLoggingService = messageAndLoggingService;
            _executeAndCatchErrors = executeAndCatchErrors;

            _convertingUpdaterSubsriptions = new CompositeDisposable();
        }



        /// <summary>
        /// Запущен ли процесс конвертации
        /// </summary>
        private bool IsConverting { get; set; }

        /// <summary>
        /// Запустить процесс конвертирования
        /// </summary>      
        public void StartConverting()
        {
            _messageAndLoggingService.ShowMessage("Запуск процесса конвертирования...");

            _convertingUpdaterSubsriptions.Add(
                Observable.Interval(TimeSpan.FromSeconds(_projectSettings.IntervalSecondsToServer)).
                           Where(_ => !IsConverting).
                           Subscribe(async _ =>
                                     await _executeAndCatchErrors.
                                     ExecuteAndHandleErrorAsync(_convertingService.ConvertingFirstInQueuePackage,
                                                                ApplicationBeforeMethod: () => IsConverting = true,
                                                                ApplicationFinallyMethod: () => IsConverting = false)));
        }

        #region IDisposable Support
        private bool disposedValue = false;
       
        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _convertingUpdaterSubsriptions?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion       
    }
}
