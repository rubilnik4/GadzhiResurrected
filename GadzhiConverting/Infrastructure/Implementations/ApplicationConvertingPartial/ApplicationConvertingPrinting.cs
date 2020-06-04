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
                                                                             IConvertingSettings convertingSettings, ColorPrint colorPrint) =>
            documentLibrary.GetStampContainer(convertingSettings.ToApplication()).
            Map(stampContainer => StampContainerPdfActions(stampContainer, documentLibrary, filePath, convertingSettings, colorPrint));

        /// <summary>
        /// Обработать штампы и начать печать
        /// </summary>
        private IResultCollection<IFileDataSourceServer> StampContainerPdfActions(IStampContainer stampContainer, IDocumentLibrary documentLibrary,
                                                                                   IFilePath filePath, IConvertingSettings convertingSettings,
                                                                                   ColorPrint colorPrint) =>
            stampContainer.CompressFieldsRanges().
            Map(_ => stampContainer.InsertSignatures().ToResultCollectionFromApplication()).
            ResultValueOkBind(signatures => StampContainerPdfPrinting(stampContainer.GetStampsToPrint().ToResultCollectionFromApplication(),
                                                                      documentLibrary, filePath, convertingSettings, colorPrint).
                                            Void(_ => stampContainer.DeleteSignatures(signatures))).
            ToResultCollection();

        /// <summary>
        /// Печать штампов
        /// </summary>
        private IResultCollection<IFileDataSourceServer> StampContainerPdfPrinting(IResultCollection<IStamp> stampsToPrint, IDocumentLibrary documentLibrary,
                                                                                   IFilePath filePath, IConvertingSettings convertingSettings,
                                                                                   ColorPrint colorPrint) =>
            stampsToPrint.
            ResultValueOkBind(stamps => StampFilePath.GetFileNamesByNamingType(stamps, filePath.FilePathClient, convertingSettings.PdfNamingType).
                                        ResultValueOk(fileNames => stamps.Zip(fileNames, (stamp, fileName) => (stamp, fileName)))).
            ResultValueOkBind(stampsFileName => CreatePdfCollection(stampsFileName, documentLibrary, filePath,
                                                                    colorPrint, convertingSettings.PdfPrinterInformation)).
            ToResultCollection();

        /// <summary>
        /// Создать коллекцию PDF для штампа, вставить подписи
        /// </summary>
        private IResultCollection<IFileDataSourceServer> CreatePdfCollection(IEnumerable<(IStamp Stamp, string FileName)> stampsFileName,
                                                                             IDocumentLibrary documentLibrary, IFilePath filePath,
                                                                             ColorPrint colorPrint, IResultValue<IPrinterInformation> pdfPrinterInformation) =>
            stampsFileName.
            Select(stampFileName => filePath.ChangeServerName(stampFileName.FileName).ChangeClientName(stampFileName.FileName).
                                    Map(filePathChanged => CreatePdf(documentLibrary, stampFileName.Stamp, filePathChanged,
                                                                     colorPrint, pdfPrinterInformation))).
            ToResultCollection();

        /// <summary>
        /// Печать пдф
        /// </summary>
        private IResultValue<IFileDataSourceServer> CreatePdf(IDocumentLibrary documentLibrary, IStamp stamp, IFilePath filePath,
                                                              ColorPrint colorPrint, IResultValue<IPrinterInformation> pdfPrinterInformation) =>
            pdfPrinterInformation.
            ResultVoidOk(pdfPrinter => SetDefaultPrinter(pdfPrinter.Name)).
            ResultValueOkBind(_ => PrintPdfCommand(documentLibrary, stamp, filePath.FilePathServer, colorPrint,
                                                   pdfPrinterInformation.Value.PrefixSearchPaperSize)).
            ResultValueOk(_ => new FileDataSourceServer(filePath.FilePathServer, filePath.FilePathClient, stamp.PaperSize,
                                                        pdfPrinterInformation.Value.Name));

        /// <summary>
        /// Установить принтер по умолчанию
        /// </summary>       
        private static void SetDefaultPrinter(string printerName) =>
            PrinterSettings.InstalledPrinters.Cast<string>().
            WhereNull(printersInstall => printersInstall.Any(printerInstall => printerInstall.ContainsIgnoreCase(printerName)) &&
                                         NativeMethods.SetDefaultPrinter(printerName));

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
