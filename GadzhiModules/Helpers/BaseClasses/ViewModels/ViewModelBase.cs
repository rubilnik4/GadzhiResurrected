using System;
using System.Globalization;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using GadzhiCommon.Infrastructure.Implementations;
using Prism.Mvvm;

namespace GadzhiModules.Helpers.BaseClasses.ViewModels
{
    public abstract class ViewModelBase : BindableBase, IFormattable, IDisposable
    {
        /// <summary>
        /// Название
        /// </summary>
        public abstract string Title { get; }

        /// <summary>
        /// Видимость
        /// </summary>
        public virtual bool Visibility => true;

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
        /// При наличие ошибок WCF останавливает процесс конвертации
        /// </summary> 
        protected void ExecuteAndHandleError(Action method, Action applicationAbortionMethod = null) =>       
            ExecuteAndCatchErrors.ExecuteAndHandleError(method,
                                                        () => IsLoading = true,
                                                        applicationAbortionMethod,
                                                        () => IsLoading = false);
        
        /// <summary>
        /// Обертка для вызова индикатора загрузки и отл5ова ошибок асинхронного метода
        /// </summary> 
        protected async Task ExecuteAndHandleErrorAsync(Func<Task> asyncMethod, Action applicationAbortionMethod = null) =>        
            await ExecuteAndCatchErrors.ExecuteAndHandleErrorAsync(asyncMethod,
                                                                   () => IsLoading = true,
                                                                   applicationAbortionMethod,
                                                                   () => IsLoading = false);

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
