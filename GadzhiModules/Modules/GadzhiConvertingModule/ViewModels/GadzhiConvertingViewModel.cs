using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiModules.Helpers.BaseClasses.ViewModels;
using GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.DialogViewModel;
using GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs;
using GadzhiModules.Modules.GadzhiConvertingModule.Views.DialogViews;
using MaterialDesignThemes.Wpf;
using Nito.AsyncEx.Synchronous;
using Prism.Mvvm;
using Unity;

namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels
{
    public class GadzhiConvertingViewModel : BindableBase, IDisposable
    {
        /// <summary>
        /// Подписка на обновление модели
        /// </summary>
        private readonly IReadOnlyList<IDisposable> _tabViewModelsVisibility;

        public GadzhiConvertingViewModel(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            _tabViewModels = new List<ViewModelBase>
            {
                container.Resolve<FilesConvertingViewModel>(),
                container.Resolve<FilesErrorsViewModel>(),
                container.Resolve<ConvertingSettingsViewModel>(),
            };
            _tabViewModelsVisibility = SubscribeToViewsVisibility(_tabViewModels);

            TabViewModelsVisible = new ObservableCollection<ViewModelBase>();
            BindingOperations.EnableCollectionSynchronization(TabViewModelsVisible, _tabViewModelsVisibleLock);
            UpdateTabViewModelsVisible().WaitAndUnwrapException();
        }

        /// <summary>
        /// Модели вкладок
        /// </summary>
        private readonly IReadOnlyCollection<ViewModelBase> _tabViewModels;

        /// <summary>
        /// Видимые модели вкладок
        /// </summary>
        public ObservableCollection<ViewModelBase> TabViewModelsVisible { get; }

        /// <summary>
        /// Блокировка списка ошибок для других потоков
        /// </summary>
        private readonly object _tabViewModelsVisibleLock = new object();

        /// <summary>
        /// Текущая вкладка
        /// </summary>
        private ViewModelBase _selectedTabViewModel;

        /// <summary>
        /// Текущая вкладка
        /// </summary>
        public ViewModelBase SelectedTabViewModel
        {
            get => _selectedTabViewModel;
            set => SetProperty(ref _selectedTabViewModel, value);
        }

        /// <summary>
        /// Обновить видимые модели
        /// </summary>
        private async Task UpdateTabViewModelsVisible()
        {
            TabViewModelsVisible.Clear();
            var tabsVisible = _tabViewModels.Where(tab => tab.Visibility);
            TabViewModelsVisible.AddRange(tabsVisible);

            await ShowResultConvertingDialog(_tabViewModels.OfType<FilesConvertingViewModel>().FirstOrDefault(),
                                             _tabViewModels.OfType<FilesErrorsViewModel>().FirstOrDefault());
        }

        /// <summary>
        /// Показать диалоговое окно после завершения конвертации
        /// </summary>
        private async Task ShowResultConvertingDialog(FilesConvertingViewModel filesConvertingViewModel, FilesErrorsViewModel filesErrorsViewModel)
        {
            if (filesConvertingViewModel?.StatusProcessingProject == StatusProcessingProject.End)
            {
                await Application.Current.Dispatcher.InvokeAsync(async () =>
                {
                    bool hasErrors = filesConvertingViewModel.FilesDataCollection.Any(fileData => fileData.IsCriticalError);
                    var successDialogViewModel = new SuccessDialogViewModel(hasErrors);
                    var successDialogView = new SuccessDialogView(successDialogViewModel);
                    var showErrors = await DialogHost.Show(successDialogView, "RootDialog");
                    if ((bool)showErrors)
                    {
                        SelectedTabViewModel = filesErrorsViewModel;
                    }
                });
            }
        }

        /// <summary>
        /// Подписаться на обновление свойства видимости отображения
        /// </summary>
        private IReadOnlyList<IDisposable> SubscribeToViewsVisibility(IEnumerable<ViewModelBase> tabViewModels) =>
            tabViewModels.
            Select(tabViewModel => tabViewModel.VisibilityChange.
                                                Select(_ => Observable.FromAsync(UpdateTabViewModelsVisible)).
                                                Concat().
                                                Subscribe()).
           ToList();

        #region IDisposable Support
        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;

            if (disposing)
            {
                if (_tabViewModelsVisibility != null)
                {
                    foreach (var tabViewModel in _tabViewModelsVisibility)
                    {
                        tabViewModel.Dispose();
                    }
                }
            }

            _disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}