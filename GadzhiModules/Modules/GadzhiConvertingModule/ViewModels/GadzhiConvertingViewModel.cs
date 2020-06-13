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
                container.Resolve<FilesErrorsViewModel>(),
                container.Resolve<ConvertingSettingsViewModel>(),
            };
        }

        public ObservableCollection<BindableBase> TabViewModels { get; }
    }
}