using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Interfaces.Histories;
using GadzhiModules.Infrastructure.Implementations.Converters.Histories;

namespace GadzhiModules.Modules.GadzhiConvertingModule.ViewModels.Tabs.HistoryViewModels.HistoryViewModelItems
{
    /// <summary>
    /// Модель отконвертированного файла
    /// </summary>
    public class HistoryFileDataSourceViewModelItem
    {
        public HistoryFileDataSourceViewModelItem(IHistoryFileDataSource historyFileDataSource)
        {
            _historyFileDataSource = historyFileDataSource;
        }

        /// <summary>
        /// Данные истории конвертации ресурсов файла
        /// </summary>
        private readonly IHistoryFileDataSource _historyFileDataSource;

        /// <summary>
        /// Имя файла
        /// </summary>
        public string FileName =>
            _historyFileDataSource.FileName;

        /// <summary>
        /// Типы допустимых расширений
        /// </summary>   
        public string FileExtensionType =>
            FileExtensionTypeConverter.FileExtensionToString(_historyFileDataSource.FileExtensionType);

        /// <summary>
        /// Принтер
        /// </summary>
        public string PrinterName =>
            _historyFileDataSource.PrinterName;

        /// <summary>
        /// Формат
        /// </summary>
        public string PaperSize =>
            _historyFileDataSource.PaperSize != "Undefined"
                ? _historyFileDataSource.PaperSize
                : "-";
    }
}