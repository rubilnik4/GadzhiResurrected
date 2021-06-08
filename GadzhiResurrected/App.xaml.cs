using System.Threading.Tasks;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiModules.Modules.GadzhiConvertingModule;
using GadzhiResurrected.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Unity;
using System.Windows;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiResurrected.Infrastructure.Implementations;
using GadzhiResurrected.Infrastructure.Interfaces;
using GadzhiResurrected.Infrastructure.Interfaces.ApplicationGadzhi;
using GadzhiResurrected.Modules.GadzhiConvertingModule;
using GadzhiResurrected.ViewModels;
using Unity;
using ApplicationGadzhi = GadzhiResurrected.Infrastructure.Implementations.ApplicationGadzhi.ApplicationGadzhi;

namespace GadzhiResurrected
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App: PrismApplication
    {
        /// <summary>
        /// Запуск главного окна
        /// </summary>
        protected override Window CreateShell()
        {
            Current.Exit += MainWindow_Closing;
            return Container.Resolve<MainView>();
        }

        /// <summary>
        /// Закрываем сессию на сервер при закрытии программы
        /// </summary>
        [Logger("----------Close application----------")]
        private void MainWindow_Closing(object sender, ExitEventArgs e)
        {
            Container.Resolve<IApplicationGadzhi>()?.Dispose();
        }

        /// <summary>
        /// Регистрирование зависимостей 
        /// </summary>
        [Logger("----------Start application----------")]
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var unityContainer = containerRegistry.GetContainer();

            unityContainer.RegisterType<IDialogService, DialogService>();
            unityContainer.RegisterType<IFileSystemOperations, FileSystemOperations>();
            unityContainer.RegisterType<IFilePathOperations, FilePathOperations>();
            unityContainer.RegisterSingleton<IApplicationGadzhi, ApplicationGadzhi>();
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
            moduleCatalog.AddModule<GadzhiConvertingModule>();
        }
    }
}
