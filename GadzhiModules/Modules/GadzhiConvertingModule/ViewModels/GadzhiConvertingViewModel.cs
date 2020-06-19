using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using GadzhiModules.Helpers.BaseClasses.ViewModels;
using GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs;
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
            _tabViewModelsVisibility = _tabViewModels.
                                       Select(tabViewModel => tabViewModel.VisibilityChange.Subscribe(UpdateTabViewModelsVisible)).
                                       ToList();

            TabViewModelsVisible = new ObservableCollection<ViewModelBase>();
            BindingOperations.EnableCollectionSynchronization(TabViewModelsVisible, _tabViewModelsVisibleLock);
            UpdateTabViewModelsVisible(false);
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
        /// Обновить видимые модели
        /// </summary>
        private void UpdateTabViewModelsVisible(bool visibility)
        {
            TabViewModelsVisible.Clear();
            var tabsVisible = _tabViewModels.Where(tab => tab.Visibility);
            TabViewModelsVisible.AddRange(tabsVisible);
        }

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