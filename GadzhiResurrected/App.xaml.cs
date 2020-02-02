using GadzhiModules.Modules.FilesConvertModule;
using GadzhiModules.Infrastructure;
using GadzhiResurrected.ViewModels;
using GadzhiResurrected.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Unity;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Infrastructure.Implementations;

namespace GadzhiResurrected
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        /// <summary>
        /// Запуск главного окна
        /// </summary>       
        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }
      
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {          
            IUnityContainer unityContainer = containerRegistry.GetContainer();
            unityContainer.RegisterSingleton<IApplicationGadzhi, ApplicationGadzhi>();
            unityContainer.RegisterType<IDialogServiceStandard, DialogServiceStandard>();
            unityContainer.RegisterType<IFileSystemOperations, FileSystemOperations>();          
        }

        /// <summary>
        /// Привязка View к ViewModels
        /// </summary>
        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
            ViewModelLocationProvider.Register<MainView, MainViewModel>();
        }

        /// <summary>
        /// Регистрируем отображаемые модули
        /// </summary>
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<FilesConvertModule>();
        }
    }
}
