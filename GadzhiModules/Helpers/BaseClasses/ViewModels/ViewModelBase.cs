using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiModules.Infrastructure;
using GadzhiModules.Infrastructure.Interfaces;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace Helpers.GadzhiModules.BaseClasses.ViewModels
{
    public abstract class ViewModelBase : BindableBase
    {
        /// <summary>
        /// Класс обертка для отлова ошибок
        /// </summary>       
        protected IExecuteAndCatchErrors ExecuteAndCatchErrors { get; set; }

        public ViewModelBase(IExecuteAndCatchErrors executeAndCatchErrors)
        {
            ExecuteAndCatchErrors = executeAndCatchErrors;
        }

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
        protected void ExecuteAndHandleError(Action method, Action ApplicationAbortionMethod = null)
        {

            ExecuteAndCatchErrors.ExecuteAndHandleError(method,
                                                        () => IsLoading = true,
                                                        ApplicationAbortionMethod,
                                                        () => IsLoading = false);

        }

        /// <summary>
        /// Обертка для вызова индикатора загрузки и отл5ова ошибок асинхронного метода
        /// </summary> 
        protected async Task ExecuteAndHandleErrorAsync(Func<Task> asyncMethod, Action ApplicationAbortionMethod = null)
        {           
            await ExecuteAndCatchErrors.ExecuteAndHandleErrorAsync(asyncMethod,
                                                                   () => IsLoading = true,
                                                                   ApplicationAbortionMethod,
                                                                   () => IsLoading = false);
           
        }
    }
}
