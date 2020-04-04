using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Interfaces.Errors;
using Prism.Mvvm;
using System;
using System.Threading.Tasks;

namespace Helpers.GadzhiModules.BaseClasses.ViewModels
{
    public abstract class ViewModelBase : BindableBase
    { 
        public ViewModelBase() { }

        /// <summary>
        /// Индикатор загрузки
        /// </summary>         
        private bool _isLoading;
        protected bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        /// <summary>
        /// Обертка для вызова индикатора загрузки и отлова ошибок метода.
        /// При наличие ошибок WCF останаливает процеес конвертации
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
