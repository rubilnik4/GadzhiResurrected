using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extentions.Collection;
using GadzhiCommon.Extentions.Functional;
using GadzhiCommon.Extentions.Functional.Result;
using GadzhiCommon.Extentions.StringAdditional;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Extensions;
using GadzhiConverting.Infrastructure.Interfaces.ApplicationConvertingPartial;
using GadzhiConverting.Models.Implementations;
using GadzhiConverting.Models.Implementations.FilesConvert;
using GadzhiConverting.Models.Interfaces;
using GadzhiConverting.Models.Interfaces.Printers;
using GadzhiWord.Helpers.Implementations;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Implementations.ApplicationConvertingPartial
{
    /// <summary>
    /// Подкласс для выполнения печати
    /// </summary>
    public partial class ApplicationConverting
    {
        /// <summary>
        /// Найти все доступные штампы на всех листах. Начать обработку каждого из них
        /// </summary>       
        private IResultFileDataSource CreatePdfInDocument(string filePath, ColorPrint colorPrint, IPrinterInformation pdfPrinterInformation) =>
            ActiveLibrary.StampContainer.
            WhereContinue(stampContainer => stampContainer.IsValid,
                okFunc: stampContainer => stampContainer.Stamps?.
                                          Select(stamp => CreatePdfWithSignatures(stamp, filePath, colorPrint, pdfPrinterInformation)).
                                          Aggregate((resultPrevious, resultNext) => resultPrevious.ConcatResult(resultNext)),
                badFunc: _ => new ErrorConverting(FileConvertErrorType.StampNotFound, $"Штампы в файле {Path.GetFileName(filePath)} не найдены").
                              Map(error => new ResultFileDataSource(error)));

        /// <summary>
        /// Создать PDF для штампа, вставить подписи
        /// </summary>       
        private IResultFileDataSource CreatePdfWithSignatures(IStamp stamp, string filePath, ColorPrint colorPrint,
                                                          IPrinterInformation pdfPrinterInformation) =>
            stamp.CompressFieldsRanges().
            Map(_ => stamp.InsertSignatures().ToResultFileDataSource()).
            ConcatResult(CreatePdf(stamp, filePath, colorPrint, stamp.PaperSize, pdfPrinterInformation)).
            Map(result => { stamp.DeleteSignatures(); return result; });

        /// <summary>
        /// Печать пдф
        /// </summary>
        private IResultFileDataSource CreatePdf(IStamp stamp, string filePath, ColorPrint colorPrint,
                                                string paperSize, IPrinterInformation pdfPrinterInformation) =>
            SetDefaultPrinter(pdfPrinterInformation.Name).
            ResultOkBind(() => PrintPdfCommand(stamp, filePath, colorPrint, pdfPrinterInformation.PrefixSearchPaperSize)).
            WhereContinue(resultCommand => resultCommand.OkStatus,
                okFunc: resultCommand => new FileDataSourceServer(filePath, FileExtention.pdf, paperSize, pdfPrinterInformation.Name).
                                         Map(fileDataSource => new ResultFileDataSource(fileDataSource)),
                badFunc: result => new ResultFileDataSource(result.Errors));
            
        /// <summary>
        /// Установить принтер по умолчанию
        /// </summary>       
        private IResultConverting SetDefaultPrinter(string printerName) =>
            PrinterSettings.InstalledPrinters?.Cast<string>()?.
            WhereNull(printersInstall => printersInstall.Any(printerInstall => printerInstall.ContainsIgnoreCase(printerName)) &&
                                         NativeMethods.SetDefaultPrinter(printerName))?.
            Map(_ => new ResultConverting())
            ?? new ErrorConverting(FileConvertErrorType.PrinterNotInstall, $"Принтер {printerName} не установлен в системе").
               ToResultConverting();

        /// <summary>
        /// Команда печати PDF
        /// </summary>
        private IResultConverting PrintPdfCommand(IStamp stamp, string filePath, ColorPrint colorPrint, string prefixSearchPaperSize)
        {
            IResultConverting printPdfCommand() => ActiveLibrary.PrintStamp(stamp, colorPrint.ToApplication(), prefixSearchPaperSize).
                                                   ToResultFileDataSource();
            return _pdfCreatorService.PrintPdfWithExecuteAction(filePath, printPdfCommand);
        }
    }
}
