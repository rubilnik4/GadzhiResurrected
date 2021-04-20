using System;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Base;

namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs
{
    public class ServerViewModel : ViewModelBase
    {
        public ServerViewModel(IDialogService dialogService)
        {
            DialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        }

        /// <summary>
        /// Стандартные диалоговые окна
        /// </summary>
        protected override IDialogService DialogService { get; }

        /// <summary>
        /// Название
        /// </summary>
        public override string Title => "Сервер";
    }
}