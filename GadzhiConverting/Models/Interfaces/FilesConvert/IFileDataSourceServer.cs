using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertingModels.Models.Interfaces.FilesConvert
{
    /// <summary>
    /// Отконвертированный файл серверной части
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
        FileExtention FileExtensionType { get; }

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
