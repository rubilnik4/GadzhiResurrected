using GadzhiConverting.Configuration;
using GadzhiConverting.Models.Implementations.Printers;
using GadzhiConverting.Models.Interfaces.Printers;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiApplicationCommon.Extensions.Functional;
using GadzhiApplicationCommon.Models.Enums.Printers;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions.Functional.Result;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;

namespace GadzhiConverting.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Заполнить модель с информацией о принтерах
    /// </summary>
    public static class ConverterPrintingConfiguration
    {
        [Logger]
        public static IResultValue<IPrintersInformation> ToPrintersInformation() =>
            RegisterPrintersInformationSection.GetConfig().
            Map(printerConfig =>
                PrinterSettings.InstalledPrinters.Cast<string>().
                Map(systemPrinters => printerConfig.PrintersCollection.
                                      Where(printer => systemPrinters.Contains(printer.Name, StringComparer.OrdinalIgnoreCase)))).
            Select(GetPrinter).
            ToResultCollection().
            ResultValueOk(printers => new PrintersInformation(printers));

        /// <summary>
        /// Функция создания информации о принтере
        /// </summary>
        private static Func<string, PrinterType, PrinterFormatType, string, IPrinterInformation> PrinterFunc() =>
            (name, printerType, printerFormatType, prefix) =>
                new PrinterInformation(name, printerType, printerFormatType, prefix);

        /// <summary>
        /// Получить информацию о принтере
        /// </summary>
        private static IResultValue<IPrinterInformation> GetPrinter(PrinterInformationElement printerInformation) =>
            new ResultValue<Func<string, PrinterType, PrinterFormatType, string, IPrinterInformation>>(PrinterFunc()).
            ResultCurryOkBind(new ResultValue<string>(printerInformation.Name, new ErrorCommon(ErrorConvertingType.ValueNotInitialized,
                                                                                                     "Отсутствует наименование принтера"))).
            ResultCurryOkBind(printerInformation.PrinterType.HasValue
                                  ? new ResultValue<PrinterType>(printerInformation.PrinterType.Value)
                                  : new ResultValue<PrinterType>(new ErrorCommon(ErrorConvertingType.ValueNotInitialized, "Отсутствует тип принтера"))).
            ResultCurryOkBind(printerInformation.PrinterFormatType.HasValue
                                  ? new ResultValue<PrinterFormatType>(printerInformation.PrinterFormatType.Value)
                                  : new ResultValue<PrinterFormatType>(new ErrorCommon(ErrorConvertingType.ValueNotInitialized, "Отсутствует формат принтера"))).
            ResultCurryOkBind(new ResultValue<string>(printerInformation.Name, new ErrorCommon(ErrorConvertingType.ValueNotInitialized,
                                                                                                     "Отсутствует префикс принтера"))).
            ResultValueOk(func => func());
    }
}
