using System;
using System.Threading.Tasks;
using GadzhiCommon.Infrastructure.Implementations;
using Prism.Mvvm;

namespace GadzhiModules.Helpers.BaseClasses.ViewModels
{
    public abstract class ViewModelBase : BindableBase
    {
        /// <summary>
        /// Название
        /// </summary>
        public abstract string Title { get; }

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
    }
}
