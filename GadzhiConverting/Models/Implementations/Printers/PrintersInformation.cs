using GadzhiConverting.Models.Interfaces.Printers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Enums.Printers;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions.Functional.Result;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;

namespace GadzhiConverting.Models.Implementations.Printers
{
    /// <summary>
    /// Информация о принтерах
    /// </summary>
    public class PrintersInformation : IPrintersInformation
    {
        public PrintersInformation(IEnumerable<IPrinterInformation> printersPdf)
        {
            Printers = printersPdf ?? throw new ArgumentNullException(nameof(printersPdf));
        }

        /// <summary>
        /// Список принтеров для печати PDF
        /// </summary>
        public IEnumerable<IPrinterInformation> Printers { get; }

        /// <summary>
        /// Принтер PDF
        /// </summary>
        public IResultValue<IPrinterInformation> PdfPrinter =>
            Printers.FirstOrDefault(printer => printer.PrinterType == PrinterType.Pdf).
            Map(printer => new ResultValue<IPrinterInformation>(printer, new ErrorCommon(ErrorConvertingType.ValueNotInitialized,
                                                                                         "Отсутствуют PDF принтеры")));

        /// <summary>
        /// Принтер больших форматов черно-белые
        /// </summary>
        public IResultValue<IPrinterInformation> BigBlackPrinter =>
            Printers.Where(printer => printer.PrinterType == PrinterType.BlackAndWhite &&
                                      printer.PrinterFormatType == PrinterFormatType.Big).
            WhereContinue(printers => printers.Any(),
                          printers => new ResultCollection<IPrinterInformation>(printers),
                          printers => new ResultCollection<IPrinterInformation>(new ErrorCommon(ErrorConvertingType.ValueNotInitialized,
                                                                                                "Отсутствуют большие черно-белые принтеры"))).
            ResultValueOk(printers => printers[new Random().Next(printers.Count)]);

        /// <summary>
        /// Принтер малых форматов черно-белые
        /// </summary>
        public IResultValue<IPrinterInformation> SmallBlackPrinter =>
            Printers.Where(printer => printer.PrinterType == PrinterType.BlackAndWhite &&
                                      printer.PrinterFormatType == PrinterFormatType.Small).
            WhereContinue(printers => printers.Any(),
                          printers => new ResultCollection<IPrinterInformation>(printers),
                          printers => new ResultCollection<IPrinterInformation>(new ErrorCommon(ErrorConvertingType.ValueNotInitialized,
                                                                                                "Отсутствуют малые черно-белые принтеры"))).
            ResultValueOk(printers => printers[new Random().Next(printers.Count)]);
    }
}
