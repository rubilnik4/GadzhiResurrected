using GadzhiModules.BaseClasses.ViewModels;
using GadzhiModules.Infrastructure;
using GadzhiModules.Modules.FilesConvertModule.Model;
using GongSolutions.Wpf.DragDrop;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using Unity;

namespace GadzhiModules.Modules.FilesConvertModule.ViewModels
{
    public class FilesConvertViewModel : ViewModelBase, IDropTarget
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

            ClearFilesDelegateCommand = new DelegateCommand(
               ClearFiles,
               () => !IsLoading).
               ObservesProperty(() => IsLoading);

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
        /// Очистить список файлов
        /// </summary>       
        public DelegateCommand ClearFilesDelegateCommand { get; private set; }

        /// <summary>
        /// Добавить файлы для конвертации
        /// </summary>       
        public DelegateCommand AddFromFilesDelegateCommand { get; private set; }

        /// <summary>
        /// Добавить папки для конвертации
        /// </summary>       
        public DelegateCommand AddFromFoldersDelegateCommand { get; private set; }

        /// <summary>
        /// Очистить список файлов
        /// </summary> 
        private void ClearFiles()
        {
            ExecuteMethod(_applicationGadzhi.ClearFiles);
        }

        /// <summary>
        /// Добавить файлы для конвертации
        /// </summary> 
        private async Task AddFromFiles()
        {
            await ExecuteAndHandleErrorAsync(_applicationGadzhi.AddFromFiles);
        }

        /// <summary>
        /// Добавить папки для конвертации
        /// </summary> 
        private async Task AddFromFolders()
        {
            await ExecuteAndHandleErrorAsync(_applicationGadzhi.AddFromFolders);
        }

        /// <summary>
        /// Добавить файлы и папки для конвертации
        /// </summary> 
        private async Task AddFromFilesAndFolders(IEnumerable<string> fileOrDirectoriesPaths)
        {
            await ExecuteAndHandleErrorAsync(_applicationGadzhi.AddFromFilesOrDirectories, fileOrDirectoriesPaths);
        }

        /// <summary>
        /// Обновление данных после изменения модели
        /// </summary> 
        private void OnFilesInfoUpdated(object sender, EventArgs args)
        {
            FilesDataCollection.Clear();
            FilesDataCollection.AddRange(_applicationGadzhi.FilesInfoProject.Files);
        }

        /// <summary>
        /// Реализация Drag&Drop для ссылки на файлы
        /// </summary>       
        public void DragOver(IDropInfo dropInfo)
        {
            dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;

            var dataObject = dropInfo.Data as IDataObject;
            if (dataObject != null && dataObject.GetDataPresent(DataFormats.FileDrop))
            {
                dropInfo.Effects = DragDropEffects.Copy;
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            var dataObject = dropInfo.Data as DataObject;
            if (dataObject != null && dataObject.ContainsFileDropList())
            {
                var filePaths = dataObject.GetFileDropList().Cast<string>().ToList();                
                Task.FromResult(AddFromFilesAndFolders(filePaths));
            }

        }
    }

}
