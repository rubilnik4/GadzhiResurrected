using System;
using GadzhiModules.Infrastructure.Interfaces;
using GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Base;

namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs
{
    /// <summary>
    /// История
    /// </summary>
    public class HistoryViewModel : ViewModelBase
    {
        public HistoryViewModel(IDialogService dialogService)
        {
            DialogService = dialogService;
        }
        /// <summary>
        /// Стандартные диалоговые окна
        /// </summary>
        protected override IDialogService DialogService { get; }

        /// <summary>
        /// Название
        /// </summary>
        public override string Title => "История";

        /// <summary>
        /// Дата с
        /// </summary>
        public DateTime DateTimeFrom { get; set; } = DateTimeNow;

        /// <summary>
        /// Дата по
        /// </summary>
        public DateTime DateTimeTo { get; set; } = DateTimeNow;

        /// <summary>
        /// Текущее время
        /// </summary>
        public static DateTime DateTimeNow =>
            DateTime.Now;
    }
}