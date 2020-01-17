using GadzhieResurrected.Infrastructure;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhieResurrected.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        /// <summary>
        /// Слой инфраструктуры
        /// </summary> 
        IApplicationGadzhi ApplicationGadzhi { get; }

        public MainWindowViewModel(IApplicationGadzhi applicationGadzhi)
        {
            ApplicationGadzhi = applicationGadzhi;

            AddFromFilesDelegateCommand = new DelegateCommand(
                async () => await AddFromFiles(),
                () => !IsLoading).
                ObservesProperty(() => IsLoading);
        }

        /// <summary>
        /// Добавить файлы для конвертации
        /// </summary>       
        public DelegateCommand AddFromFilesDelegateCommand { get; private set; }

        /// <summary>
        /// Индикатор загрузки
        /// </summary>         
        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }


        /// <summary>
        /// Добавить файлы для конвертации
        /// </summary> 
        private async Task AddFromFiles()
        {
            IsLoading = true;
            try
            {
                await ApplicationGadzhi.AddFromFiles();
            }
            finally
            {
                IsLoading = false;
            }
        }
    }

}
