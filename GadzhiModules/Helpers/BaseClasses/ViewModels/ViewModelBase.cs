using GadzhiModules.Infrastructure;
using GadzhiModules.Infrastructure.Interfaces;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace Helpers.GadzhiModules.BaseClasses.ViewModels
{
    public abstract class ViewModelBase : BindableBase
    {
        /// <summary>
        /// Стандартные диалоговые окна
        /// </summary>     
        [Dependency]
        public IDialogServiceStandard DialogServiceStandard { get; set; } 

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
        /// Обертка для вызова индикатора загрузки и отлова ошибок метода
        /// </summary> 
        protected void ExecuteAndHandleError(Action method)
        {
            IsLoading = true;
            try
            {
                method();
            }
            catch (Exception ex)
            {
                DialogServiceStandard.ShowMessage(ex.Message);
            }
            finally
            {
                IsLoading = false;
            }

        }

        public void ExecuteAndHandleError<T1>(Action<T1> function, T1 arg1)
        {
            ExecuteAndHandleError(() => function(arg1));
        }

        //https://gist.github.com/ghstahl/7022ee06c1f9a1753a11efb51882740c
        /// <summary>
        /// Обертка для вызова индикатора загрузки и отлова ошибок асинхронного метода
        /// </summary> 
        protected async Task ExecuteAndHandleErrorAsync(Func<Task> asyncMethod)
        {
            IsLoading = true;
            try
            {               
                await asyncMethod();
            }
            catch (Exception ex)
            {
                DialogServiceStandard.ShowMessage(ex.Message);
            }
            finally
            {
                IsLoading = false;
            }           
        }

        /// <summary>
        /// Обертка для вызова индикатора загрузки и отлова ошибок асинхронной функции
        /// </summary> 
        public async Task ExecuteAndHandleErrorAsync<T1>(Func<T1, Task> functionAsync, T1 arg1)        
        {
            await ExecuteAndHandleErrorAsync(() => functionAsync(arg1));
        }
    }
}
