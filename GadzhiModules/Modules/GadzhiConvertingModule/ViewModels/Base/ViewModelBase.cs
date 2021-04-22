using System;
using System.Globalization;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Extensions.Functional.Result;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Infrastructure.Interfaces.Logger;
using GadzhiCommon.Models.Implementations.Functional;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiModules.Infrastructure.Interfaces;
using Prism.Mvvm;

namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Base
{
    public abstract class ViewModelBase : BindableBase, IFormattable, IDisposable
    {
        /// <summary>
        /// Журнал системных сообщений
        /// </summary>
        private static readonly ILoggerService _loggerService = LoggerFactory.GetFileLogger();

        /// <summary>
        /// Стандартные диалоговые окна
        /// </summary>
        protected abstract IDialogService DialogService { get; }

        /// <summary>
        /// Название
        /// </summary>
        public abstract string Title { get; }

        /// <summary>
        /// Видимость
        /// </summary>
        public virtual bool Visibility => true;

        /// <summary>
        /// Выбрана ли страница
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Подписка на изменение видимости
        /// </summary>
        private readonly Subject<bool> _visibilityChange = new Subject<bool>();

        /// <summary>
        /// Подписка на изменение видимости
        /// </summary>
        public ISubject<bool> VisibilityChange => _visibilityChange;

        /// <summary>
        /// Индикатор загрузки
        /// </summary>         
        private bool _isLoading;

        /// <summary>
        /// Индикатор загрузки
        /// </summary>    
        protected bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        /// <summary>
        /// Обертка для вызова индикатора загрузки и отлова ошибок метода.
        /// </summary> 
        protected void ExecuteAndHandleError(Action method, Action applicationAbortionMethod = null) =>
            ExecuteAndCatchErrors.ExecuteAndHandleError(method,
                                                        () => IsLoading = true,
                                                        applicationAbortionMethod,
                                                        () => IsLoading = false).
            ResultVoidBad(errors => DialogService.ShowErrors(errors)).
            ResultVoidBad(errors => _loggerService.ErrorsLog(errors));

        /// <summary>
        /// Обертка для вызова индикатора загрузки и отл5ова ошибок асинхронного метода
        /// </summary> 
        protected async Task ExecuteAndHandleErrorAsync(Func<Task> asyncMethod, Action applicationAbortionMethod = null) =>
            await ExecuteAndCatchErrors.ExecuteAndHandleErrorAsync(asyncMethod,
                                                                   () => IsLoading = true,
                                                                   applicationAbortionMethod,
                                                                   () => IsLoading = false).
            MapAsync(result => (IResultValue<Unit>)result).
            ResultVoidBadAsync(errors => _loggerService.ErrorsLog(errors));

        #region IFormattable Support
        public override string ToString() => ToString(String.Empty, CultureInfo.CurrentCulture);

        public string ToString(string format, IFormatProvider formatProvider) => this.GetType().Name;
        #endregion

        #region IDisposable Support
        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;

            if (disposing)
            {
                _visibilityChange?.Dispose();
            }

            _disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
