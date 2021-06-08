using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiResurrected.Infrastructure.Implementations.Converters;
using GadzhiResurrected.Infrastructure.Interfaces;
using GadzhiResurrected.Infrastructure.Interfaces.ApplicationGadzhi;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting.ReactiveSubjects;
using GadzhiResurrected.Modules.GadzhiConvertingModule.ViewModels.Base;
using GadzhiResurrected.Modules.GadzhiConvertingModule.ViewModels.DialogViewModel;
using GadzhiResurrected.Modules.GadzhiConvertingModule.ViewModels.Tabs.FilesConvertingViewModelItems;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Views.DialogViews;
using GongSolutions.Wpf.DragDrop;
using MaterialDesignThemes.Wpf;
using Nito.AsyncEx.Synchronous;
using Prism.Commands;

namespace GadzhiResurrected.Modules.GadzhiConvertingModule.ViewModels.Tabs
{
    /// <summary>
    /// Представление конвертации файлов
    /// </summary>
    public class FilesConvertingViewModel : ViewModelBase, IDropTarget
    {
        public FilesConvertingViewModel(IApplicationGadzhi applicationGadzhi, IStatusProcessingInformation statusProcessingInformation,
                                        IDialogService dialogService)
        {
            _applicationGadzhi = applicationGadzhi;
            _statusProcessingInformation = statusProcessingInformation;
            DialogService = dialogService;

            FilesDataCollection = new ObservableCollection<FileDataViewModelItem>();
            applicationGadzhi.FileDataChange.Subscribe(OnFilesInfoUpdated);

            InitializeDelegateCommands();
        }

        /// <summary>
        /// Слой инфраструктуры
        /// </summary>        
        private readonly IApplicationGadzhi _applicationGadzhi;

        /// <summary>
        /// Текущий статус конвертирования
        /// </summary>        
        private readonly IStatusProcessingInformation _statusProcessingInformation;

        /// <summary>
        /// Инициализировать команды
        /// </summary>
        private void InitializeDelegateCommands()
        {
            ClearFilesCommand = new DelegateCommand(ClearFiles, () => !IsLoading && !IsConverting).
                                        ObservesProperty(() => IsLoading).
                                        ObservesProperty(() => IsConverting);

            AddFromFilesCommand = new DelegateCommand(AddFromFiles, () => !IsLoading && !IsConverting).
                                          ObservesProperty(() => IsLoading).
                                          ObservesProperty(() => IsConverting);

            AddFromFoldersCommand = new DelegateCommand(AddFromFolders, () => !IsLoading && !IsConverting).
                                            ObservesProperty(() => IsLoading).
                                            ObservesProperty(() => IsConverting);

            RemoveFilesCommand = new DelegateCommand(RemoveFiles, () => !IsLoading && !IsConverting).
                                         ObservesProperty(() => IsLoading).
                                         ObservesProperty(() => IsConverting);

            OpenContainingFolderCommand = new DelegateCommand(OpenContainingFolder, 
                                                              () => SelectedFilesDataItemViewModels?.Count > 0).
                                          ObservesProperty(() => SelectedFilesDataItems);

            ConvertingFilesCommand = new DelegateCommand(async () => await ConvertingFiles(),
                                                                 () => !IsLoading && !IsConverting).
                                             ObservesProperty(() => IsLoading).
                                             ObservesProperty(() => IsConverting);

            CloseApplicationCommand = new DelegateCommand(async () => await CloseApplication());
            AboutApplicationCommand = new DelegateCommand(async () => await AboutApplication());
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
        private IList<object> _selectedFilesDataItems = new List<object>();

        /// <summary>
        /// Выделенные строки
        /// </summary>
        public IList<object> SelectedFilesDataItems
        {
            get => _selectedFilesDataItems;
            set => SetProperty(ref _selectedFilesDataItems, value);
        }

        /// <summary>
        /// Выделенные строки
        /// </summary>
        public IReadOnlyCollection<FileDataViewModelItem> SelectedFilesDataItemViewModels =>
            SelectedFilesDataItems.OfType<FileDataViewModelItem>().ToList();

        /// <summary>
        /// Очистить список файлов
        /// </summary>       
        public DelegateCommand ClearFilesCommand { get; private set; }

        /// <summary>
        /// Добавить файлы для конвертации
        /// </summary>       
        public DelegateCommand AddFromFilesCommand { get; private set; }

        /// <summary>
        /// Добавить папки для конвертации
        /// </summary>       
        public DelegateCommand AddFromFoldersCommand { get; private set; }

        /// <summary>
        /// Удалить файлы из списка
        /// </summary>       
        public DelegateCommand RemoveFilesCommand { get; private set; }

        /// <summary>
        /// Удалить файлы из списка
        /// </summary>       
        public DelegateCommand OpenContainingFolderCommand { get; private set; }

        /// <summary>
        /// Конвертировать файлы
        /// </summary>       
        public DelegateCommand ConvertingFilesCommand { get; private set; }

        /// <summary>
        /// Закрыть приложение
        /// </summary>       
        public DelegateCommand CloseApplicationCommand { get; private set; }

        /// <summary>
        /// О приложении
        /// </summary>       
        public DelegateCommand AboutApplicationCommand { get; private set; }

        /// <summary>
        /// Индикатор конвертирования файлов
        /// </summary>
        public bool IsConverting => _statusProcessingInformation.IsConverting;

        /// <summary>
        /// Типы цветов для печати
        /// </summary>
        public IReadOnlyCollection<string> ColorPrintToString =>
            ColorPrintConverter.ColorPrintsString;

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
        private void AddFromFiles() => 
            ExecuteAndHandleError(_applicationGadzhi.AddFromFiles);

        /// <summary>
        /// Добавить папки для конвертации
        /// </summary>
        [Logger]
        private void AddFromFolders() => 
            ExecuteAndHandleError(_applicationGadzhi.AddFromFolders);

        /// <summary>
        /// Добавить файлы и папки для конвертации
        /// </summary>
        [Logger]
        private void  AddFromFilesAndFolders(IEnumerable<string> fileOrDirectoriesPaths) =>
            ExecuteAndHandleError(() => _applicationGadzhi.AddFromFilesOrDirectories(fileOrDirectoriesPaths));

        /// <summary>
        /// Удалить файлы из списка
        /// </summary>
        [Logger]
        private void RemoveFiles() =>
            SelectedFilesDataItems?.
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
        private async Task CloseApplication() => 
            await ExecuteAndHandleErrorAsync(_applicationGadzhi.CloseApplication);

        /// <summary>
        /// О приложении
        /// </summary>
        [Logger]
        private static async Task AboutApplication()
        {
            //var messageDialogViewModel = new MessageDialogViewModel(MessageDialogType.Information, "");
            //var messageDialogView = new MessageDialogView(messageDialogViewModel);
            //await DialogHost.Show(messageDialogView, "RootDialog");
            var aboutApplicationDialogViewModel = new AboutApplicationDialogViewModel();
            var aboutApplicationDialogView = new AboutApplicationDialogView(aboutApplicationDialogViewModel);
            await DialogHost.Show(aboutApplicationDialogView, "RootDialog");
        }

        /// <summary>
        /// Установка цвета печати выделенных файлов
        /// </summary>
        private void SetColorPrintToSelectedItems(ColorPrintType colorPrintType)
        {
            foreach (var fileDataViewModelItem in SelectedFilesDataItemViewModels)
            {
                fileDataViewModelItem.ChangeColorPrint(colorPrintType);
            }
        }

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
            var fileDataViewModel = filesChange?.FilesData?.Select(fileData => new FileDataViewModelItem(fileData, SetColorPrintToSelectedItems));
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
                var fileDataViewModel = filesChange?.FilesDataProject?.Select(fileData => new FileDataViewModelItem(fileData, SetColorPrintToSelectedItems));
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
            AddFromFilesAndFolders(filePaths);
        }

        /// <summary>
        /// Открыть папку, содержащую файл
        /// </summary>
        private void OpenContainingFolder()
        {
            if (SelectedFilesDataItemViewModels == null || SelectedFilesDataItemViewModels.Count == 0) return;
            string directoryPath = Path.GetDirectoryName(SelectedFilesDataItemViewModels.First().FilePath)?? String.Empty;
            Process.Start(directoryPath);
        }
    }
}
