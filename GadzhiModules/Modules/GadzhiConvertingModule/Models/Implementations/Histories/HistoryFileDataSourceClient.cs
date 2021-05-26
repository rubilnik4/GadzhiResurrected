using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Interfaces.Histories;

namespace GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.Histories
{
    /// <summary>
    /// Данные истории конвертации ресурсов файла
    /// </summary>
    public class HistoryFileDataSourceClient : IHistoryFileDataSource
    {
        public HistoryFileDataSourceClient(FileExtensionType fileExtensionType, string printerName, string paperSize)
        {
            FileExtensionType = fileExtensionType;
            PrinterName = printerName;
            PaperSize = paperSize;
        }

        /// <summary>
        /// Типы допустимых расширений
        /// </summary>   
        public FileExtensionType FileExtensionType { get; }

        /// <summary>
        /// Принтер
        /// </summary>
        public string PrinterName { get; }

        /// <summary>
        /// Формат
        /// </summary>
        public string PaperSize { get; }
    }
}