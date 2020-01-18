using GadzhiModules.BaseClasses.ViewModels;
using GadzhiModules.Infrastructure;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace GadzhiModules.FilesConvertModule.ViewModels
{
    public class FilesConvertViewModel : ViewModelBase
    {
        /// <summary>
        /// Слой инфраструктуры
        /// </summary> 
        [Dependency]
        public IApplicationGadzhi ApplicationGadzhi { get; set; }

        public FilesConvertViewModel()
        {
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
        /// Добавить файлы для конвертации
        /// </summary> 
        private async Task AddFromFiles()
        {
           await ExecuteMethodAsync(ApplicationGadzhi.AddFromFiles);  
        }


    }

}
