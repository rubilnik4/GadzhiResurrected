using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using GadzhiModules.Helpers.BaseClasses.ViewModels;
using GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs;
using Prism.Mvvm;
using Unity;

namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels
{
    public class GadzhiConvertingViewModel : BindableBase
    {
        public GadzhiConvertingViewModel(IUnityContainer container)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            TabViewModels = new ObservableCollection<BindableBase>
            {
                container.Resolve<FilesConvertingViewModel>(),
                container.Resolve<ConvertingSettingsViewModel>(),
            };
        }

        public ObservableCollection<BindableBase> TabViewModels { get; }

        /// <summary>
        /// Отображение в окне вкладки
        /// </summary>
        private ViewModelBase _selectedTab;

        /// <summary>
        /// Отображение в окне вкладки
        /// </summary>
        public ViewModelBase SelectedTab
        {
            get => _selectedTab;
            set => _selectedTab = TabItemChangeEvent(value);
        }

        /// <summary>
        /// Действия при смене вкладок
        /// </summary>
        public ViewModelBase TabItemChangeEvent(ViewModelBase tabViewModel)
        {
            if (tabViewModel?.GetType() == _selectedTab?.GetType()) return tabViewModel;

            if (tabViewModel?.GetType() != typeof(ConvertingSettingsViewModel) &&
                _selectedTab?.GetType() == typeof(ConvertingSettingsViewModel))
            {
                var convertingSettingsViewModel = (ConvertingSettingsViewModel)_selectedTab;
                convertingSettingsViewModel.SaveSettingsChanges();
            }
            return tabViewModel;
        }
    }
}