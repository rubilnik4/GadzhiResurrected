using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Extensions;
using GadzhiConverting.Models.Implementations.FilesConvert;
using GadzhiConverting.Models.Interfaces.Printers;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Extensions.Functional.Result;
using GadzhiCommon.Extensions.StringAdditional;
using GadzhiConverting.Helpers;
using GadzhiConverting.Models.Interfaces.FilesConvert;
using GadzhiCommon.Enums.ConvertingSettings;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiApplicationCommon.Extensions.Functional.Result;

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
        private IResultCollection<IFileDataSourceServer> CreatePdfInDocument(IDocumentLibrary documentLibrary, IFilePath filePath,
                                                                             IConvertingSettings convertingSettings, ColorPrint colorPrint, 
                                                                             IPrinterInformation pdfPrinterInformation) =>
            documentLibrary.GetStampContainer(convertingSettings.ToApplication()).
            Stamps.ToResultCollectionFromApplication().
            ResultValueOkBind(stamps => stamps.
                                        Select(stamp => CreatePdfWithFilePath(stamp, documentLibrary, filePath, convertingSettings, 
                                                                              colorPrint, pdfPrinterInformation)).
                                        ToList().ToResultCollection()).
            ToResultCollection();

        /// <summary>
        /// Создать пдф и сохранить согласно типу именования
        /// </summary>
        private IResultValue<IFileDataSourceServer> CreatePdfWithFilePath(IStamp stamp, IDocumentLibrary documentLibrary, IFilePath filePath,
                                                                          IConvertingSettings convertingSettings,
                                                                          ColorPrint colorPrint, IPrinterInformation pdfPrinterInformation) =>
            new FilePath(StampFilePath.GetFilePathByNamingType(filePath.FilePathServer, convertingSettings.PdfNamingType, stamp),
                         StampFilePath.GetFilePathByNamingType(filePath.FilePathClient, convertingSettings.PdfNamingType, stamp)).
            Map(stampFilePath => CreatePdfWithSignatures(documentLibrary, stamp, stampFilePath, colorPrint, pdfPrinterInformation));

        /// <summary>
        /// Создать PDF для штампа, вставить подписи
        /// </summary>       
        private IResultValue<IFileDataSourceServer> CreatePdfWithSignatures(IDocumentLibrary documentLibrary, IStamp stamp, IFilePath filePath, ColorPrint colorPrint,
                                                                            IPrinterInformation pdfPrinterInformation) =>
            stamp.CompressFieldsRanges().
            Map(_ => stamp.InsertSignatures()).
            Map(signatures => CreatePdf(documentLibrary, stamp, filePath, colorPrint, stamp.PaperSize, pdfPrinterInformation).
                              ConcatErrors(signatures.Errors.ToErrorsConverting()).
                              Void(_ => stamp.DeleteSignatures(signatures.Value)));

        /// <summary>
        /// Печать пдф
        /// </summary>
        private IResultValue<IFileDataSourceServer> CreatePdf(IDocumentLibrary documentLibrary, IStamp stamp, IFilePath filePath,
                                                              ColorPrint colorPrint, string paperSize, IPrinterInformation pdfPrinterInformation) =>
            SetDefaultPrinter(pdfPrinterInformation.Name).
            ResultValueOkBind(_ => PrintPdfCommand(documentLibrary, stamp, filePath.FilePathServer, colorPrint, pdfPrinterInformation.PrefixSearchPaperSize)).
            ResultValueOk(_ => (IFileDataSourceServer)new FileDataSourceServer(filePath.FilePathServer, filePath.FilePathClient,
                                                                               paperSize, pdfPrinterInformation.Name));

        /// <summary>
        /// Установить принтер по умолчанию
        /// </summary>       
        private static IResultError SetDefaultPrinter(string printerName) =>
            PrinterSettings.InstalledPrinters.Cast<string>().
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
            IResultError PrintPdfCommand() => documentLibrary.PrintStamp(stamp, colorPrint.ToApplication(), prefixSearchPaperSize).
                                              ToResultFromApplication();
            return _pdfCreatorService.PrintPdfWithExecuteAction(filePath, PrintPdfCommand);
        }
    }
}
