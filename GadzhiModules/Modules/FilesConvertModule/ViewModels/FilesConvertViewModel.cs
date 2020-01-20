using GadzhiModules.BaseClasses.ViewModels;
using GadzhiModules.Infrastructure;
using GadzhiModules.Modules.FilesConvertModule.Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Threading;
using Unity;

namespace GadzhiModules.Modules.FilesConvertModule.ViewModels
{
    public class FilesConvertViewModel : ViewModelBase
    {       
        /// <summary>
        /// Слой инфраструктуры
        /// </summary>        
        private IApplicationGadzhi _applicationGadzhi;

        public FilesConvertViewModel(IApplicationGadzhi applicationGadzhi)
        {
            _applicationGadzhi = applicationGadzhi;

            FilesDataCollection = new ObservableCollection<FileData>();
            _applicationGadzhi.FilesInfoProject.FilesDataUpdatedEvent += OnFilesInfoUpdated;

            AddFromFilesDelegateCommand = new DelegateCommand(
                async () => await AddFromFiles(),
                () => !IsLoading).
                ObservesProperty(() => IsLoading);

            AddFromFoldersDelegateCommand = new DelegateCommand(
              async () => await AddFromFolders(),
              () => !IsLoading).
              ObservesProperty(() => IsLoading);
        }

        /// <summary>
        /// Данные о конвертируемых файлах
        /// </summary>
        public ObservableCollection<FileData> FilesDataCollection { get; }

        /// <summary>
        /// Добавить файлы для конвертации
        /// </summary>       
        public DelegateCommand AddFromFilesDelegateCommand { get; private set; }

        /// <summary>
        /// Добавить папки для конвертации
        /// </summary>       
        public DelegateCommand AddFromFoldersDelegateCommand { get; private set; }

        /// <summary>
        /// Добавить файлы для конвертации
        /// </summary> 
        private async Task AddFromFiles()
        {
            await ExecuteMethodAsync(_applicationGadzhi.AddFromFiles);
        }

        /// <summary>
        /// Добавить папки для конвертации
        /// </summary> 
        private async Task AddFromFolders()
        {
            await ExecuteMethodAsync(_applicationGadzhi.AddFromFolders);
        }

        /// <summary>
        /// Обновление данных после изменения модели
        /// </summary> 
        private void OnFilesInfoUpdated(object sender, EventArgs args) 
        {          
                FilesDataCollection.Clear();
                FilesDataCollection.AddRange(_applicationGadzhi.FilesInfoProject.Files);           
        }
    }

}
