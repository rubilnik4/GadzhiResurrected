using GadzhiModules.Infrastructure;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace GadzhiModules.BaseClasses.ViewModels
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
        /// Обертка для вызова индикатора загрузки и отлова ошибок
        /// </summary> 
        protected async Task ExecuteMethodAsync(Func<Task> asyncMethod)
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
    }
}
