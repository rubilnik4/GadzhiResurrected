using System.Collections.Generic;
using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiConverting.Models.Interfaces.FilesConvert
{
    /// <summary>
    /// Обработанный файл серверной части
    /// </summary>
    public interface  IFileDataSourceServer: IEnumerable<IFileDataSourceServer>
    {
        /// <summary>
        /// Путь файла
        /// </summary>
        string FilePath { get; }

        /// <summary>
        /// Тип расширения файла
        /// </summary>
        FileExtension FileExtension { get; }

        /// <summary>
        /// Формат печати
        /// </summary>
        string PaperSize { get; }

        /// <summary>
        /// Имя принтера
        /// </summary>
        string PrinterName { get; }
    }
}
