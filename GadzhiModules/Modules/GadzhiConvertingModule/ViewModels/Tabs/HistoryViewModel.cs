using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiModules.Infrastructure.Implementations.Converters.Histories;
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
        /// Типы режимов историй
        /// </summary>
        public IReadOnlyCollection<string> HistoryTypes =>
            HistoryTypeConverter.HistoriesString;

        /// <summary>
        /// Текущий режим типа истории
        /// </summary>
        public string SelectedHistoryType { get; set; } = 
            HistoryTypeConverter.HistoriesString.FirstOrDefault();

        /// <summary>
        /// Типы расширений
        /// </summary>
        public IReadOnlyCollection<string> FileExtensionTypes =>
            ConvertingFileTypeConverter.FileExtensionsString;

        /// <summary>
        /// Текущее расширение
        /// </summary>
        public string SelectedFileExtensionType { get; set; } =
            ConvertingFileTypeConverter.FileExtensionsString.FirstOrDefault();

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