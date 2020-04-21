using ConvertingModels.Models.Interfaces.FilesConvert;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extentions.Collection;
using GadzhiCommon.Extentions.Functional;
using GadzhiCommon.Extentions.Functional.Result;
using GadzhiCommon.Extentions.StringAdditional;
using GadzhiCommon.Functional;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Extensions;
using GadzhiConverting.Infrastructure.Interfaces.ApplicationConvertingPartial;
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
        private IResultCollection<IFileDataSourceServer> CreatePdfInDocument(IDocumentLibrary documentLibrary, string filePath,
                                                                             ColorPrint colorPrint, IPrinterInformation pdfPrinterInformation) =>
            documentLibrary.StampContainer.Stamps.ToResultCollectionFromApplication().
            ResultValueOkBind(stamps => stamps.Select(stamp =>
                                        CreatePdfWithSignatures(documentLibrary, stamp, filePath, colorPrint, pdfPrinterInformation)).
                                        ToResultCollection()).
            ToResultCollection();

        /// <summary>
        /// Создать PDF для штампа, вставить подписи
        /// </summary>       
        private IResultValue<IFileDataSourceServer> CreatePdfWithSignatures(IDocumentLibrary documentLibrary, IStamp stamp, string filePath, ColorPrint colorPrint,
                                                                            IPrinterInformation pdfPrinterInformation) =>
            stamp.CompressFieldsRanges().
            Map(_ => stamp.InsertSignatures().ToResultFromApplication()).
            Map(errors => CreatePdf(documentLibrary, stamp, filePath, colorPrint, stamp.PaperSize, pdfPrinterInformation).
                          ConcatErrors(errors.Errors)).
            Map(result => { stamp.DeleteSignatures(); return result; });

        /// <summary>
        /// Печать пдф
        /// </summary>
        private IResultValue<IFileDataSourceServer> CreatePdf(IDocumentLibrary documentLibrary, IStamp stamp, string filePath, ColorPrint colorPrint,
                                                              string paperSize, IPrinterInformation pdfPrinterInformation) =>
            SetDefaultPrinter(pdfPrinterInformation.Name).
            ResultValueOkBind(_ => PrintPdfCommand(documentLibrary, stamp, filePath, colorPrint, pdfPrinterInformation.PrefixSearchPaperSize)).
            ResultValueOk(_ => (IFileDataSourceServer)new FileDataSourceServer(filePath, paperSize, pdfPrinterInformation.Name));

        /// <summary>
        /// Установить принтер по умолчанию
        /// </summary>       
        private IResultError SetDefaultPrinter(string printerName) =>
            PrinterSettings.InstalledPrinters?.Cast<string>()?.
            WhereNull(printersInstall => printersInstall.Any(printerInstall => printerInstall.ContainsIgnoreCase(printerName)) &&
                                         NativeMethods.SetDefaultPrinter(printerName))?.
            Map(_ => new ResultError())
            ?? new ErrorCommon(FileConvertErrorType.PrinterNotInstall, $"Принтер {printerName} не установлен в системе").
               ToResult();

        /// <summary>
        /// Команда печати PDF
        /// </summary>
        private IResultError PrintPdfCommand(IDocumentLibrary documentLibrary, IStamp stamp, string filePath, 
                                             ColorPrint colorPrint, string prefixSearchPaperSize)
        {
            IResultError printPdfCommand() => documentLibrary.PrintStamp(stamp, colorPrint.ToApplication(), prefixSearchPaperSize).
                                         ToResultFromApplication();
            return _pdfCreatorService.PrintPdfWithExecuteAction(filePath, printPdfCommand);
        }
    }
}
