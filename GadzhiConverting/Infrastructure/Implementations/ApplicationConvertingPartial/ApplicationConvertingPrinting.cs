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
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.StampPartial;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Infrastructure.Implementations.Reflection;
using GadzhiCommon.Models.Enums;
using GadzhiConvertingLibrary.Extensions;

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
        [Logger]
        private IResultCollection<IFileDataSourceServer> CreatePdfInDocument(IDocumentLibrary documentLibrary, IFilePath filePath,
                                                                             IConvertingSettings convertingSettings, ColorPrint colorPrint) =>
            documentLibrary.
            Void(_ => _messagingService.ShowMessage("Обработка штампов...")).
            GetStampContainer(convertingSettings.ToApplication()).
            Map(stampContainer => StampContainerPdfActions(stampContainer, documentLibrary, filePath, convertingSettings, colorPrint));

        /// <summary>
        /// Обработать штампы и начать печать
        /// </summary>
        private IResultCollection<IFileDataSourceServer> StampContainerPdfActions(IStampContainer stampContainer, IDocumentLibrary documentLibrary,
                                                                                  IFilePath filePath, IConvertingSettings convertingSettings,
                                                                                  ColorPrint colorPrint) =>
            stampContainer.
            Void(_ => _messagingService.ShowMessage("Форматирование полей...")).
            CompressFieldsRanges().
            Void(_ => _loggerService.LogByObject(LoggerLevel.Debug, LoggerAction.Operation, ReflectionInfo.GetMethodBase(this), nameof(stampContainer.CompressFieldsRanges))).
            Void(_ => _messagingService.ShowMessage("Вставка подписей...")).
            Map(_ => stampContainer.InsertSignatures().ToResultCollectionFromApplication()).
            ResultVoidOk(signatures => _loggerService.LogByObjects(LoggerLevel.Debug, LoggerAction.Operation, ReflectionInfo.GetMethodBase(this),
                                                                   signatures.Select(signature => signature.SignatureLibrary.PersonInformation.FullName))).
            ResultValueOkBind(signatures => StampContainerPdfPrinting(stampContainer.GetStampsToPrint().ToResultCollectionFromApplication(),
                                                                      documentLibrary, filePath, convertingSettings, colorPrint).
                                            Void(_ => _messagingService.ShowMessage("Удаление подписей...")).
                                            Void(_ => stampContainer.DeleteSignatures(signatures)).
                                            Void(_ => _loggerService.LogByObject(LoggerLevel.Debug, LoggerAction.Operation,
                                                                                 ReflectionInfo.GetMethodBase(this), nameof(stampContainer.DeleteSignatures)))).
            ToResultCollection();

        /// <summary>
        /// Печать штампов
        /// </summary>
        [Logger]
        private IResultCollection<IFileDataSourceServer> StampContainerPdfPrinting(IResultCollection<IStamp> stampsToPrint, IDocumentLibrary documentLibrary,
                                                                                   IFilePath filePath, IConvertingSettings convertingSettings,
                                                                                   ColorPrint colorPrint) =>
            stampsToPrint.
            ResultValueOkBind(stamps => StampFilePath.GetFileNamesByNamingType(stamps, filePath.FileNameWithoutExtensionClient, convertingSettings.PdfNamingType).
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
            ResultVoidOk(pdfPrinter => _messagingService.ShowMessage($"Установка принтера {pdfPrinter.Name}")).
            ResultVoidOk(pdfPrinter => SetDefaultPrinter(pdfPrinter.Name)).
            ResultVoidOk(pdfPrinter => _messagingService.ShowMessage($"Печать файла {filePath.FileNameClient}")).
            ResultValueOkBind(_ => PrintPdfCommand(documentLibrary, stamp, filePath.FilePathServer, colorPrint,
                                                   pdfPrinterInformation.Value.PrefixSearchPaperSize)).
            ResultVoidOk(_ => _loggerService.LogByObject(LoggerLevel.Debug, LoggerAction.Operation, ReflectionInfo.GetMethodBase(this), filePath.FilePathServer)).
            ResultValueOk(_ => new FileDataSourceServer(filePath.FilePathServer, filePath.FilePathClient, stamp.PaperSize,
                                                        pdfPrinterInformation.Value.Name));

        /// <summary>
        /// Установить принтер по умолчанию
        /// </summary>       
        private static void SetDefaultPrinter(string printerName) =>
            PrinterSettings.InstalledPrinters.Cast<string>().
            WhereNull(printersInstall => printersInstall.Any(printerInstall => printerInstall.ContainsIgnoreCase(printerName)) &&
                                         NativeMethods.SetDefaultPrinter(printerName).
                                         Void(_ => _loggerService.DebugLog($"Set printer {printerName}")));

        /// <summary>
        /// Команда печати PDF
        /// </summary>
        [Logger]
        private IResultError PrintPdfCommand(IDocumentLibrary documentLibrary, IStamp stamp, string filePath,
                                             ColorPrint colorPrint, string prefixSearchPaperSize)
        {
            IResultError PrintPdfCommandLocal() => documentLibrary.PrintStamp(stamp, colorPrint.ToApplication(), prefixSearchPaperSize).ToResultFromApplication();
            return _pdfCreatorService.PrintPdfWithExecuteAction(filePath, PrintPdfCommandLocal).ToResult();
        }
    }
}
