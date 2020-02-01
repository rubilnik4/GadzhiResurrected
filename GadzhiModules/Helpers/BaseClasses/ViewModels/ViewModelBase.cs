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
        [Dependency]
        public IExecuteAndCatchErrors ExecuteAndCatchErrors { get; set; }

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
            IsLoading = true;
            ExecuteAndCatchErrors.ExecuteAndHandleError(method, ApplicationAbortionMethod);
            IsLoading = false;
        }

        public void ExecuteAndHandleError<T1>(Action<T1> function, T1 arg1, Action ApplicationAbortionMethod = null)
        {
            ExecuteAndHandleError(() => function(arg1), ApplicationAbortionMethod);
        }

        //https://gist.github.com/ghstahl/7022ee06c1f9a1753a11efb51882740c
        /// <summary>
        /// Обертка для вызова индикатора загрузки и отл5ова ошибок асинхронного метода
        /// </summary> 
        protected async Task ExecuteAndHandleErrorAsync(Func<Task> asyncMethod, Action ApplicationAbortionMethod = null)
        {
            IsLoading = true;
            await ExecuteAndCatchErrors.ExecuteAndHandleErrorAsync(asyncMethod, ApplicationAbortionMethod);
            IsLoading = false;

        }

        /// <summary>
        /// Обертка для вызова индикатора загрузки и отлова ошибок асинхронной функции
        /// </summary> 
        public async Task ExecuteAndHandleErrorAsync<T1>(Func<T1, Task> functionAsync, T1 arg1, Action ApplicationAbortionMethod = null)
        {
            await ExecuteAndHandleErrorAsync(() => functionAsync(arg1), ApplicationAbortionMethod);
        }
    }
}
