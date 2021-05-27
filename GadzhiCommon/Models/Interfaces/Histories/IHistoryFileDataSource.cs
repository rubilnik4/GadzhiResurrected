using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiCommon.Models.Interfaces.Histories
{
    /// <summary>
    /// Данные истории конвертации ресурсов файла
    /// </summary>
    public interface IHistoryFileDataSource
    {
        /// <summary>
        /// Имя файла
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// Типы допустимых расширений
        /// </summary>   
        FileExtensionType FileExtensionType { get; }

        /// <summary>
        /// Принтер
        /// </summary>
        string PrinterName { get; }

        /// <summary>
        /// Формат
        /// </summary>
        string PaperSize { get; }
    }
}