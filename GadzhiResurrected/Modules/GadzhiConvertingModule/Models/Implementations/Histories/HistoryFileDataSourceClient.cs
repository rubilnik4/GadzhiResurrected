using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Interfaces.Histories;

namespace GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Implementations.Histories
{
    /// <summary>
    /// Данные истории конвертации ресурсов файла
    /// </summary>
    public class HistoryFileDataSourceClient : IHistoryFileDataSource
    {
        public HistoryFileDataSourceClient(string fileName, FileExtensionType fileExtensionType, 
                                           string printerName, string paperSize)
        {
            FileName = fileName;
            FileExtensionType = fileExtensionType;
            PrinterName = printerName;
            PaperSize = paperSize;
        }

        /// <summary>
        /// Имя файла
        /// </summary>
        public string FileName { get; }
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