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
            ServerTotalViewModel = new ServerTotalViewModel(serverStateClientServiceFactory, () => IsSelected);
            ServerInfoViewModel = new ServerInfoViewModel(serverStateClientServiceFactory, () => IsSelected);
            ServerDetailViewModel = new ServerDetailViewModel(serverStateClientServiceFactory, () => IsSelected);
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
        /// Выбор вкладки
        /// </summary>
        private bool _isSelected;

        /// <summary>
        /// Выбор вкладки
        /// </summary>
        public override bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                ServerDetailViewModel.OnInitialize();
            }
        }

        /// <summary>
        /// Статистика обработанных файлов
        /// </summary>
        public ServerTotalViewModel ServerTotalViewModel { get; }

        /// <summary>
        /// Информация о серверах
        /// </summary>
        public ServerInfoViewModel ServerInfoViewModel { get; }

        /// <summary>
        /// Подробная информация о сервере
        /// </summary>
        public ServerDetailViewModel ServerDetailViewModel { get; }

        #region IDisposable Support
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            ServerTotalViewModel.Dispose();
            ServerInfoViewModel.Dispose();
            ServerDetailViewModel.Dispose();
        }
        #endregion
    }
}