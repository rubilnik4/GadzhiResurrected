using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Interfaces.Errors;

namespace GadzhiConverting.Models.Interfaces.Printers
{
    /// <summary>
    /// Информация о принтерах
    /// </summary>
    public interface IPrintersInformation
    {
        /// <summary>
        /// Список принтеров для печати PDF
        /// </summary>
        IEnumerable<IPrinterInformation> Printers { get; }

        /// <summary>
        /// Принтер PDF
        /// </summary>
        IResultValue<IPrinterInformation> PdfPrinter { get; }

        /// <summary>
        /// Принтер больших форматов черно-белые
        /// </summary>
        IResultValue<IPrinterInformation> BigBlackPrinter { get; }

        /// <summary>
        /// Принтер малых форматов черно-белые
        /// </summary>
        IResultValue<IPrinterInformation> SmallBlackPrinter { get; }

        /// <summary>
        /// Принтер больших форматов цветные
        /// </summary>
        IResultValue<IPrinterInformation> BigColorPrinter { get; }

        /// <summary>
        /// Принтер малых форматов цветные
        /// </summary>
        IResultValue<IPrinterInformation> SmallColorPrinter { get; }

        /// <summary>
        /// Получить принтер по типу конвертации и формату
        /// </summary>
        IResultValue <IPrinterInformation> GetPrinter(ConvertingModeType convertingModeType, ColorPrintType colorPrintType,
                                                      StampPaperSizeType paperSize);
    }
}
