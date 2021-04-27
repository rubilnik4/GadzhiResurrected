using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Base;
using GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.DialogViewModel;
using GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs;
using Nito.AsyncEx.Synchronous;
using Prism.Mvvm;
using Unity;

namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels
{
    public class GadzhiConvertingViewModel : BindableBase, IDisposable
    {
        public GadzhiConvertingViewModel(IUnityContainer container)
        {
            _tabViewModels = GetTabViewModels(container);
            _tabViewModelsVisibility = SubscribeToViewsVisibility(_tabViewModels);

            BindingOperations.EnableCollectionSynchronization(TabViewModelsVisible, _tabViewModelsVisibleLock);
        }

        /// <summary>
        /// Подписка на обновление модели
        /// </summary>
        private readonly IReadOnlyList<IDisposable> _tabViewModelsVisibility;

        /// <summary>
        /// Модели вкладок
        /// </summary>
        private readonly IReadOnlyCollection<ViewModelBase> _tabViewModels;

        /// <summary>
        /// Видимые модели вкладок
        /// </summary>
        public IReadOnlyCollection<ViewModelBase> TabViewModelsVisible =>
            _tabViewModels.Where(tab => tab.Visibility).ToList();

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
        [Logger]
        public ViewModelBase SelectedTabViewModel
        {
            get => _selectedTabViewModel;
            set
            {
                if (_selectedTabViewModel != null) _selectedTabViewModel.IsSelected = false;
                SetProperty(ref _selectedTabViewModel, value);
                if (_selectedTabViewModel != null) _selectedTabViewModel.IsSelected = true;
            }

        }

        /// <summary>
        /// Обновить видимые модели
        /// </summary>
        private async Task UpdateTabViewModelsVisible()
        {
            RaisePropertyChanged(nameof(TabViewModelsVisible));
            await CheckResultConvertingDialog(_tabViewModels.OfType<FilesConvertingViewModel>().FirstOrDefault(),
                                              _tabViewModels.OfType<FilesErrorsViewModel>().FirstOrDefault());
        }

        /// <summary>
        /// Проверить необходимость вызова диалогового окна результатов конвертации
        /// </summary>
        private async Task CheckResultConvertingDialog(FilesConvertingViewModel filesConvertingViewModel, FilesErrorsViewModel filesErrorsViewModel)
        {
            if (filesConvertingViewModel?.StatusProcessingProject == StatusProcessingProject.End)
            {
                await ShowResultConvertingDialog(filesConvertingViewModel, filesErrorsViewModel);
            }
        }

        /// <summary>
        /// Вызвать диалоговое окно результатов конвертирования
        /// </summary>
        [Logger]
        private async Task ShowResultConvertingDialog(FilesConvertingViewModel filesConvertingViewModel, FilesErrorsViewModel filesErrorsViewModel)
        {
            bool hasErrors = filesConvertingViewModel.FilesDataCollection.Any(fileData => fileData.IsCriticalError);
            bool showErrors = await DialogFactory.GetResultDialog(hasErrors);
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                if (showErrors) SelectedTabViewModel = filesErrorsViewModel;
            });
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

        /// <summary>
        /// Получить список вкладок
        /// </summary>
        private static IReadOnlyCollection<ViewModelBase> GetTabViewModels(IUnityContainer container) =>
            new List<ViewModelBase>
            {
                container.Resolve<FilesConvertingViewModel>(),
                container.Resolve<FilesErrorsViewModel>(),
                container.Resolve<ConvertingSettingsViewModel>(),
                container.Resolve<ServerViewModel>(),
            };

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