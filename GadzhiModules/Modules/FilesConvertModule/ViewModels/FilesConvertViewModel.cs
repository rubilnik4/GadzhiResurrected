using GadzhiCommon.Enums.FilesConvert;
using GadzhiModules.Helpers.Converters;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations.ReactiveSubjects;
using GadzhiModules.Modules.FilesConvertModule.ViewModels.FilesConvertViewModelItems;
using GongSolutions.Wpf.DragDrop;
using Helpers.GadzhiModules.BaseClasses.ViewModels;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections;
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
        private IApplicationGadzhi ApplicationGadzhi { get; }

        /// <summary>
        /// Текущий статус конвертирования
        /// </summary>        
        private IStatusProcessingInformation StatusProcessingInformation { get; }

        public FilesConvertViewModel(IApplicationGadzhi applicationGadzhi,
                                     IStatusProcessingInformation statusProcessingInformation,
                                     IExecuteAndCatchErrors executeAndCatchErrors)
            :base(executeAndCatchErrors)
        {
            ApplicationGadzhi = applicationGadzhi;
            StatusProcessingInformation = statusProcessingInformation;

            FilesDataCollection = new ObservableCollection<FileDataViewModelItem>();

            ApplicationGadzhi.FileDataChange.Subscribe(OnFilesInfoUpdated);

            InitializeDelegateCommands();
        }

        /// <summary>
        /// Инициализировать команды
        /// </summary>
        private void InitializeDelegateCommands()
        {
            ClearFilesDelegateCommand = new DelegateCommand(
               ClearFiles,
               () => !IsLoading && !IsConverting).
               ObservesProperty(() => IsLoading).
               ObservesProperty(() => IsConverting);

            AddFromFilesDelegateCommand = new DelegateCommand(
               async () => await AddFromFiles(),
               () => !IsLoading && !IsConverting).
               ObservesProperty(() => IsLoading).
               ObservesProperty(() => IsConverting);

            AddFromFoldersDelegateCommand = new DelegateCommand(
               async () => await AddFromFolders(),
               () => !IsLoading && !IsConverting).
               ObservesProperty(() => IsLoading).
               ObservesProperty(() => IsConverting);

            RemoveFilesDelegateCommand = new DelegateCommand(
               RemoveFiles,
               () => !IsLoading && !IsConverting).
               ObservesProperty(() => IsLoading).
               ObservesProperty(() => IsConverting);

            ConvertingFilesDelegateCommand = new DelegateCommand(
              async () => await ConvertingFiles(),
              () => !IsLoading && !IsConverting).
              ObservesProperty(() => IsLoading).
              ObservesProperty(() => IsConverting);

            CloseApplicationDelegateCommand = new DelegateCommand(CloseApplication);
        }
        /// <summary>
        /// Данные о конвертируемых файлах
        /// </summary>
        public ObservableCollection<FileDataViewModelItem> FilesDataCollection { get; }

        private IList<object> _selectedFilesData = new List<object>();
        /// <summary>
        /// Выделенные строки
        /// </summary>
        public IList<object> SelectedFilesData
        {
            get { return _selectedFilesData; }
            set { SetProperty(ref _selectedFilesData, value); }
        }

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
        /// Конвертировать файлы
        /// </summary>       
        public DelegateCommand ConvertingFilesDelegateCommand { get; private set; }

        /// <summary>
        /// Закрыть приложение
        /// </summary>       
        public DelegateCommand CloseApplicationDelegateCommand { get; private set; }

        /// <summary>
        /// Индикатор конвертирования файлов
        /// </summary 
        public bool IsConverting => StatusProcessingInformation.IsConverting;

        /// <summary>
        /// Типы цветов для печати
        /// </summary>
        public IEnumerable<string> ColorPrintToString => ColorPrintConverter.
                                                         ColorPrintToString.
                                                         Select(color => color.Value);

        /// <summary>
        /// Статус обработки проекта
        /// </summary>
        public StatusProcessingProject StatusProcessingProject => StatusProcessingInformation.StatusProcessingProject;

        /// <summary>
        /// Статус обработки проекта c процентом выполнения
        /// </summary>
        public string StatusProcessingProjectName => StatusProcessingInformation.GetStatusProcessingProjectName();
        
        /// <summary>
        /// Отображать ли процент выполнения для ProgressBar
        /// </summary>
        public bool IsIndeterminateProgressBar => !StatusProcessingInformation.HasStatusProcessPercentage;

        /// <summary>
        /// Процент выполнения для ProgressBar
        /// </summary>
        public int PercentageOfComplete => StatusProcessingInformation.PercentageOfComplete;

        /// <summary>
        /// Очистить список файлов
        /// </summary> 
        private void ClearFiles()
        {
            ExecuteAndHandleError(ApplicationGadzhi.ClearFiles);
        }

        /// <summary>
        /// Добавить файлы для конвертации
        /// </summary> 
        private async Task AddFromFiles()
        {
            await ExecuteAndHandleErrorAsync(ApplicationGadzhi.AddFromFiles);
        }

        /// <summary>
        /// Добавить папки для конвертации
        /// </summary> 
        private async Task AddFromFolders()
        {
            await ExecuteAndHandleErrorAsync(ApplicationGadzhi.AddFromFolders);
        }

        /// <summary>
        /// Добавить файлы и папки для конвертации
        /// </summary> 
        private async Task AddFromFilesAndFolders(IEnumerable<string> fileOrDirectoriesPaths)
        {
            await ExecuteAndHandleErrorAsync(ApplicationGadzhi.AddFromFilesOrDirectories, fileOrDirectoriesPaths);
        }

        /// <summary>
        /// Удалить файлы из списка
        /// </summary> 
        private void RemoveFiles()
        {
            var removeFiles = SelectedFilesData?.
                              OfType<FileDataViewModelItem>().
                              Select(FileVm => FileVm.FileData);

            ExecuteAndHandleError(ApplicationGadzhi.RemoveFiles, removeFiles);
        }

        /// <summary>
        /// Конвертировать файлы
        /// </summary> 
        private async Task ConvertingFiles()
        {
            await ExecuteAndHandleErrorAsync(ApplicationGadzhi.ConvertingFiles, ApplicationGadzhi.AbortPropertiesConverting);
        }

        /// <summary>
        /// Удалить файлы из списка
        /// </summary> 
        private void CloseApplication()
        {
            ExecuteAndHandleError(ApplicationGadzhi.CloseApplication);
        }

        #region FilesInfoUpdate
        /// <summary>
        /// Обновление данных после изменения модели
        /// </summary> 
        private void OnFilesInfoUpdated(FilesChange fileChange)
        {
            if (fileChange.ActionType != ActionType.StatusChange)
            {

                if (fileChange.ActionType == ActionType.Add)
                {
                    ActionOnTypeAdd(fileChange);
                }
                else if (fileChange.ActionType == ActionType.Remove || fileChange.ActionType == ActionType.Clear)
                {
                    ActionOnTypeRemove(fileChange);
                }
            }           
            ActionOnTypeStatusChange(fileChange);
        }

        /// <summary>
        /// Изменения коллекции при добавлении файлов
        /// </summary>
        private void ActionOnTypeAdd(FilesChange filesChange)
        {
            var FileDataViewModel = filesChange?.FileData?.Select(fileData => new FileDataViewModelItem(fileData));
            FilesDataCollection.AddRange(FileDataViewModel);
        }

        /// <summary>
        /// Изменения коллекции при удалении файлов
        /// </summary>
        private void ActionOnTypeRemove(FilesChange filesChange)
        {
            if (filesChange?.FileData?.Count() == 1)
            {
                var fileRemove = FilesDataCollection.First(f => f.FilePath == filesChange.FileData.First().FilePath);
                FilesDataCollection.Remove(fileRemove);
            }
            else
            {
                FilesDataCollection.Clear();
                var FileDataViewModel = filesChange?.FilesDataProject?.Select(fileData => new FileDataViewModelItem(fileData));
                FilesDataCollection.AddRange(FileDataViewModel);
            }
        }

        /// <summary>
        /// Изменения коллекции при корректировке статуса конвертирования
        /// </summary>
        private void ActionOnTypeStatusChange(FilesChange filesChange)
        {
            var fileChangePath = filesChange?.FileData?.Select(file => file.FilePath);
            if (fileChangePath != null)
            {
                var filesDataNeedUpdate = FilesDataCollection.Where(fileData => fileChangePath?.Contains(fileData.FilePath) == true);
                foreach (var fileUpdate in filesDataNeedUpdate)
                {
                    fileUpdate.UpdateStatusProcessing();
                }
            }

            RaiseAfterStatusChange(filesChange);
        }

        /// <summary>
        /// Обновить поля после изменения статуса конвертирования
        /// </summary>
        private void RaiseAfterStatusChange(FilesChange filesChange)
        {
            //необходимо сначала вычислить проценты, затем статус
            if (StatusProcessingInformation.HasStatusProcessPercentage)
            {
                RaisePropertyChanged(nameof(PercentageOfComplete));
            }
            if (filesChange.IsStatusProcessingProjectChanged)
            {
                RaisePropertyChanged(nameof(IsIndeterminateProgressBar));
                RaisePropertyChanged(nameof(StatusProcessingProject));              
            }

            //Изменяем процент выполнения в зависимости от типа операции
            if (filesChange.IsStatusProcessingProjectChanged || StatusProcessingInformation.HasStatusProcessPercentage)
            {
                RaisePropertyChanged(nameof(StatusProcessingProjectName));
            }
            if (StatusProcessingInformation.IsConvertingChanged)
            {
                RaisePropertyChanged(nameof(IsConverting));
            }
        }
        #endregion


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
