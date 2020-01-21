using GadzhiModules.Infrastructure;
using GadzhiModules.Modules.FilesConvertModule.Model;
using GadzhiModules.Modules.FilesConvertModule.Model.ReactiveSubjects;
using GongSolutions.Wpf.DragDrop;
using Helpers.GadzhiModules.BaseClasses.ViewModels;
using MvvmHelpers;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

            FilesDataCollection = new ObservableRangeCollection<FileData>();
            _applicationGadzhi.FilesInfoProject.FileDataChange.Subscribe(OnFilesInfoUpdated);

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

            RemoveFilesDelegateCommand = new DelegateCommand(
              RemoveFiles,
              () => !IsLoading).
              ObservesProperty(() => IsLoading);
        }

        /// <summary>
        /// Данные о конвертируемых файлах
        /// </summary>
        public ObservableRangeCollection<FileData> FilesDataCollection { get; }

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
        /// Удалить файлы из списка
        /// </summary>       
        public DelegateCommand RemoveFilesDelegateCommand { get; private set; }

        /// <summary>
        /// Очистить список файлов
        /// </summary> 
        private void ClearFiles()
        {
            ExecuteAndHandleError(_applicationGadzhi.ClearFiles);
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
        /// Удалить файлы из списка
        /// </summary> 
        private void RemoveFiles()
        {
            ExecuteAndHandleError(_applicationGadzhi.RemoveFiles, new List<FileData>());
        }

        /// <summary>
        /// Обновление данных после изменения модели
        /// </summary> 
        private void OnFilesInfoUpdated(FileChange fileChange)
        {
            if (fileChange.ActionType == ActionType.Add)
            {
                FilesDataCollection.AddRange(fileChange.FileData, NotifyCollectionChangedAction.Add);
            }
            else if (fileChange.ActionType == ActionType.Remove)
            {
                FilesDataCollection.RemoveRange(fileChange.FileData, NotifyCollectionChangedAction.Remove);
            }
            else if (fileChange.ActionType == ActionType.Clear)
            {
                FilesDataCollection.Clear();
            }


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
