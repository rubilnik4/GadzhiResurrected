using GadzhiConverting.Models.Interfaces.Printers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Enums.Printers;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
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
            Map(printer => new ResultValue<IPrinterInformation>(printer, new ErrorCommon(ErrorConvertingType.PrinterNotInstall,
                                                                                         "Отсутствуют PDF принтеры")));

        /// <summary>
        /// Принтер больших форматов черно-белые
        /// </summary>
        public IResultValue<IPrinterInformation> BigBlackPrinter =>
            Printers.Where(printer => printer.PrinterType == PrinterType.BlackAndWhite &&
                                      printer.PrinterFormatType == PrinterFormatType.Big).
            WhereContinue(printers => printers.Any(),
                          printers => new ResultCollection<IPrinterInformation>(printers),
                          printers => new ResultCollection<IPrinterInformation>(new ErrorCommon(ErrorConvertingType.PrinterNotInstall,
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
                          printers => new ResultCollection<IPrinterInformation>(new ErrorCommon(ErrorConvertingType.PrinterNotInstall,
                                                                                                "Отсутствуют малые черно-белые принтеры"))).
            ResultValueOk(printers => printers[new Random().Next(printers.Count)]);

        /// <summary>
        /// Принтер больших форматов цветные
        /// </summary>
        public IResultValue<IPrinterInformation> BigColorPrinter =>
            Printers.Where(printer => printer.PrinterType == PrinterType.Color &&
                                      printer.PrinterFormatType == PrinterFormatType.Big).
            WhereContinue(printers => printers.Any(),
                          printers => new ResultCollection<IPrinterInformation>(printers),
                          printers => new ResultCollection<IPrinterInformation>(new ErrorCommon(ErrorConvertingType.PrinterNotInstall,
                                                                                                "Отсутствуют большие цветные принтеры"))).
            ResultValueOk(printers => printers[new Random().Next(printers.Count)]);

        /// <summary>
        /// Принтер малых форматов цветные
        /// </summary>
        public IResultValue<IPrinterInformation> SmallColorPrinter =>
            Printers.Where(printer => printer.PrinterType == PrinterType.Color &&
                                      printer.PrinterFormatType == PrinterFormatType.Small).
            WhereContinue(printers => printers.Any(),
                          printers => new ResultCollection<IPrinterInformation>(printers),
                          printers => new ResultCollection<IPrinterInformation>(new ErrorCommon(ErrorConvertingType.PrinterNotInstall,
                                                                                                "Отсутствуют малые цветные принтеры"))).
            ResultValueOk(printers => printers[new Random().Next(printers.Count)]);

        /// <summary>
        /// Получить принтер по типу конвертации и формату
        /// </summary>
        public IResultValue<IPrinterInformation> GetPrinter(ConvertingModeType convertingModeType, ColorPrintType colorPrintType,
                                                            StampPaperSizeType paperSize) =>
            convertingModeType switch
            {
                ConvertingModeType.Pdf => PdfPrinter,
                ConvertingModeType.Print => GetPrinterByPaperSize(colorPrintType, paperSize),
                _ => new ResultValue<IPrinterInformation>(new ErrorCommon(ErrorConvertingType.PrinterNotInstall,
                                                                          "Для данного типа конвертирования принтер не предусмотрен"))
            };

        /// <summary>
        /// Получить принтер по формату
        /// </summary>
        private IResultValue<IPrinterInformation> GetPrinterByPaperSize(ColorPrintType colorPrintType, StampPaperSizeType paperSize) =>
            (colorPrintType, paperSize) switch
            {
                (_, _) when !IsColorPrinter(colorPrintType) && IsSmallPaperSize(paperSize) => SmallBlackPrinter,
                (_, _) when !IsColorPrinter(colorPrintType) && IsBigPaperSize(paperSize) => BigBlackPrinter,
                (_, _) when IsColorPrinter(colorPrintType) && IsSmallPaperSize(paperSize) => SmallColorPrinter,
                (_, _) when IsColorPrinter(colorPrintType) && IsBigPaperSize(paperSize) => BigColorPrinter,
                (_,_) => new ResultValue<IPrinterInformation>(new ErrorCommon(ErrorConvertingType.PrinterNotInstall,
                                                                              "Принтер данной конфигурации не найден")),
            };

        /// <summary>
        /// Цветной ли принтер
        /// </summary>
        private static bool IsColorPrinter(ColorPrintType colorPrintType) =>
            colorPrintType == ColorPrintType.Color;

        /// <summary>
        /// Является ли малым форматом
        /// </summary>
        private static bool IsSmallPaperSize(StampPaperSizeType paperSize) =>
            paperSize == StampPaperSizeType.A3 || paperSize == StampPaperSizeType.A4;

        /// <summary>
        /// Является ли малым форматом
        /// </summary>
        private static bool IsBigPaperSize(StampPaperSizeType paperSize) =>
            !IsSmallPaperSize(paperSize) && paperSize != StampPaperSizeType.Undefined!;
    }
}
