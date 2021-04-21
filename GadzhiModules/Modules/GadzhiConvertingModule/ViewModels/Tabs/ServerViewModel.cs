using System;
using GadzhiModules.Infrastructure.Implementations.Services;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Base;
using GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs.ServerViewModels;

namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs
{
    public class ServerViewModel : ViewModelBase
    {
        public ServerViewModel(IDialogService dialogService, ServerStateClientServiceFactory serverStateClientServiceFactory)
        {
            DialogService = dialogService;
            ServerTotalViewModel = new ServerTotalViewModel(serverStateClientServiceFactory);
        }

        /// <summary>
        /// Стандартные диалоговые окна
        /// </summary>
        protected override IDialogService DialogService { get; }

        /// <summary>
        /// Название
        /// </summary>
        public override string Title => "Сервер";

        /// <summary>
        /// Статистика обработанных файлов
        /// </summary>
        public ServerTotalViewModel ServerTotalViewModel { get; }

        #region IDisposable Support
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            ServerTotalViewModel.Dispose();
        }
        #endregion
    }
}