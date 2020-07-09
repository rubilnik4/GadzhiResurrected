using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiModules.Infrastructure.Implementations.Converters;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Infrastructure.Interfaces.ApplicationGadzhi;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting.ReactiveSubjects;
using GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Base;
using GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs.FilesConvertingViewModelItems;
using GongSolutions.Wpf.DragDrop;
using Nito.AsyncEx.Synchronous;
using Prism.Commands;

namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs
{
    /// <summary>
    /// Представление конвертации файлов
    /// </summary>
    public class FilesConvertingViewModel : ViewModelBase, IDropTarget
    {

        /// <summary>
        /// Слой инфраструктуры
        /// </summary>        
        private readonly IApplicationGadzhi _applicationGadzhi;

        /// <summary>
        /// Текущий статус конвертирования
        /// </summary>        
        private readonly IStatusProcessingInformation _statusProcessingInformation;

        public FilesConvertingViewModel(IApplicationGadzhi applicationGadzhi, IStatusProcessingInformation statusProcessingInformation,
                                        IDialogService dialogService)
        {
            _applicationGadzhi = applicationGadzhi ?? throw new ArgumentNullException(nameof(applicationGadzhi));
            _statusProcessingInformation = statusProcessingInformation ?? throw new ArgumentNullException(nameof(statusProcessingInformation));
            DialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));

            FilesDataCollection = new ObservableCollection<FileDataViewModelItem>();
            applicationGadzhi.FileDataChange.Subscribe(OnFilesInfoUpdated);

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

            CloseApplicationDelegateCommand = new DelegateCommand(async () => await CloseApplication());
        }

        /// <summary>
        /// Стандартные диалоговые окна
        /// </summary>
        protected override IDialogService DialogService { get; }

        /// <summary>
        /// Название
        /// </summary>
        public override string Title => "Конвертация";

        /// <summary>
        /// Данные о конвертируемых файлах
        /// </summary>
        public ObservableCollection<FileDataViewModelItem> FilesDataCollection { get; }

        /// <summary>
        /// Выделенные строки
        /// </summary>
        private IList<object> _selectedFilesData = new List<object>();

        /// <summary>
        /// Выделенные строки
        /// </summary>
        public IList<object> SelectedFilesData
        {
            get => _selectedFilesData;
            set => SetProperty(ref _selectedFilesData, value);
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
        /// </summary>
        public bool IsConverting => _statusProcessingInformation.IsConverting;

        /// <summary>
        /// Типы цветов для печати
        /// </summary>
        public IEnumerable<string> ColorPrintToString => ColorPrintConverter.
                                                         ColorPrintString.
                                                         Select(color => color.Value);

        /// <summary>
        /// Статус обработки проекта
        /// </summary>
        public StatusProcessingProject StatusProcessingProject => _statusProcessingInformation.StatusProcessingProject;

        /// <summary>
        /// Статус обработки проекта c процентом выполнения
        /// </summary>
        public string StatusProcessingProjectName => _statusProcessingInformation.GetStatusProcessingProjectName();

        /// <summary>
        /// Отображать ли процент выполнения для ProgressBar
        /// </summary>
        public bool IsIndeterminateProgressBar => !_statusProcessingInformation.HasStatusProcessPercentage;

        /// <summary>
        /// Процент выполнения для ProgressBar
        /// </summary>
        public int PercentageOfComplete => _statusProcessingInformation.PercentageOfComplete;

        /// <summary>
        /// Очистить список файлов
        /// </summary>
        [Logger]
        private void ClearFiles() => ExecuteAndHandleError(_applicationGadzhi.ClearFiles);

        /// <summary>
        /// Добавить файлы для конвертации
        /// </summary>
        [Logger]
        private async Task AddFromFiles() => await ExecuteAndHandleErrorAsync(_applicationGadzhi.AddFromFiles);

        /// <summary>
        /// Добавить папки для конвертации
        /// </summary>
        [Logger]
        private async Task AddFromFolders() => await ExecuteAndHandleErrorAsync(_applicationGadzhi.AddFromFolders);

        /// <summary>
        /// Добавить файлы и папки для конвертации
        /// </summary>
        [Logger]
        private async Task AddFromFilesAndFolders(IEnumerable<string> fileOrDirectoriesPaths) =>
            await ExecuteAndHandleErrorAsync(() => _applicationGadzhi.AddFromFilesOrDirectories(fileOrDirectoriesPaths));

        /// <summary>
        /// Удалить файлы из списка
        /// </summary>
        [Logger]
        private void RemoveFiles() =>
            SelectedFilesData?.
            OfType<FileDataViewModelItem>().
            Select(fileVm => fileVm.FileData).
            Void(filesData => ExecuteAndHandleError(() => _applicationGadzhi.RemoveFiles(filesData)));

        /// <summary>
        /// Конвертировать файлы
        /// </summary>
        [Logger]
        private async Task ConvertingFiles() =>
            await ExecuteAndHandleErrorAsync(_applicationGadzhi.ConvertingFiles,
                                             () => _applicationGadzhi.AbortPropertiesCancellation().WaitAndUnwrapException());

        /// <summary>
        /// Закрыть приложение
        /// </summary>
        [Logger]
        private async Task CloseApplication() => await ExecuteAndHandleErrorAsync(_applicationGadzhi.CloseApplication);

        /// <summary>
        /// Обновление данных после изменения модели
        /// </summary> 
        private void OnFilesInfoUpdated(FilesChange fileChange)
        {
            switch (fileChange.ActionType)
            {
                case ActionType.Add:
                    ActionOnTypeAdd(fileChange);
                    break;
                case ActionType.Remove:
                case ActionType.Clear:
                    ActionOnTypeRemove(fileChange);
                    break;
                case ActionType.StatusChange:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fileChange));
            }

            ActionOnTypeStatusChange(fileChange);
        }

        /// <summary>
        /// Изменения коллекции при добавлении файлов
        /// </summary>
        private void ActionOnTypeAdd(FilesChange filesChange)
        {
            var fileDataViewModel = filesChange?.FilesData?.Select(fileData => new FileDataViewModelItem(fileData));
            FilesDataCollection.AddRange(fileDataViewModel);
        }

        /// <summary>
        /// Изменения коллекции при удалении файлов
        /// </summary>
        private void ActionOnTypeRemove(FilesChange filesChange)
        {
            if (filesChange?.FilesData.Count == 1)
            {
                var fileRemove = FilesDataCollection.First(f => f.FilePath == filesChange.FilesData.First().FilePath);
                FilesDataCollection.Remove(fileRemove);
            }
            else
            {
                FilesDataCollection.Clear();
                var fileDataViewModel = filesChange?.FilesDataProject?.Select(fileData => new FileDataViewModelItem(fileData));
                FilesDataCollection.AddRange(fileDataViewModel);
            }
        }

        /// <summary>
        /// Изменения коллекции при корректировке статуса конвертирования
        /// </summary>
        private void ActionOnTypeStatusChange(FilesChange filesChange)
        {
            var fileChangePath = filesChange.FilesData.Select(file => file.FilePath);
            var filesDataNeedUpdate = FilesDataCollection.Where(fileData => fileChangePath.Contains(fileData.FilePath));
            foreach (var fileUpdate in filesDataNeedUpdate)
            {
                fileUpdate.UpdateStatusProcessing();
            }

            RaiseAfterStatusChange(filesChange);
        }

        /// <summary>
        /// Обновить поля после изменения статуса конвертирования
        /// </summary>
        private void RaiseAfterStatusChange(FilesChange filesChange)
        {
            //необходимо сначала вычислить проценты, затем статус
            if (_statusProcessingInformation.HasStatusProcessPercentage)
            {
                RaisePropertyChanged(nameof(PercentageOfComplete));
            }
            if (filesChange.IsStatusProcessingProjectChanged)
            {
                RaisePropertyChanged(nameof(IsIndeterminateProgressBar));
                RaisePropertyChanged(nameof(StatusProcessingProject));
            }

            //Изменяем процент выполнения в зависимости от типа операции
            if (filesChange.IsStatusProcessingProjectChanged || _statusProcessingInformation.HasStatusProcessPercentage)
            {
                RaisePropertyChanged(nameof(StatusProcessingProjectName));
            }
            if (_statusProcessingInformation.IsConvertingChanged)
            {
                RaisePropertyChanged(nameof(IsConverting));
            }
        }

        /// <summary>
        /// Реализация Drag Drop для ссылки на файлы
        /// </summary>       
        public void DragOver(IDropInfo dropInfo)
        {
            if (!(dropInfo?.Data is IDataObject dataObject)) return;

            dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
            if (dataObject.GetDataPresent(DataFormats.FileDrop))
            {
                dropInfo.Effects = DragDropEffects.Copy;
            }
        }

        /// <summary>
        /// Реализация Drag Drop для ссылки на файлы
        /// </summary>
        [Logger]
        public void Drop(IDropInfo dropInfo)
        {
            if (!(dropInfo?.Data is DataObject dataObject) ||
                !dataObject.ContainsFileDropList()) return;

            var filePaths = dataObject.GetFileDropList().Cast<string>().ToList();
            AddFromFilesAndFolders(filePaths).WaitAndUnwrapException();
        }
    }

}
