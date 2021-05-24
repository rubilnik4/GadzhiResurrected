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
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections.StampPartial;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Infrastructure.Implementations.Reflection;
using GadzhiCommon.Models.Enums;
using GadzhiConverting.Infrastructure.Implementations.Converting;
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
        private IResultCollection<IFileDataSourceServer> CreateProcessingDocument(IDocumentLibrary documentLibrary, IFilePath filePathMain, IFilePath filePathPdf,
                                                                                 IConvertingSettings convertingSettings, ColorPrintType colorPrintType) =>
            documentLibrary.
            Void(_ => _messagingService.ShowMessage("Обработка штампов...")).
            GetStampContainer(convertingSettings.ToApplication()).
            Map(stampContainer => StampContainerProcessing(stampContainer, documentLibrary, filePathMain,
                                                           filePathPdf, convertingSettings, colorPrintType));

        /// <summary>
        /// Обработать штампы и начать печать
        /// </summary>
        private IResultCollection<IFileDataSourceServer> StampContainerProcessing(IStampContainer stampContainer, IDocumentLibrary documentLibrary,
                                                                                  IFilePath filePathMain, IFilePath filePathPdf,
                                                                                  IConvertingSettings convertingSettings, ColorPrintType colorPrintType) =>
            stampContainer.
            Void(_ => _messagingService.ShowMessage("Подключение дополнительных элементов...")).
            Void(_ => documentLibrary.AttachAdditional()).
            Void(_ => _messagingService.ShowMessage("Форматирование полей...")).
            CompressFieldsRanges().
            Void(_ => _loggerService.LogByObject(LoggerLevel.Debug, LoggerAction.Operation, ReflectionInfo.GetMethodBase(this), nameof(stampContainer.CompressFieldsRanges))).
            Map(_ => GetSavedFileDataSource(stampContainer, documentLibrary, filePathMain)).
            WhereContinue(_ => ConvertingModeChoice.IsPdfConvertingNeed(convertingSettings.ConvertingModeTypes),
                          filesDataSource => StampContainerCreatePdf(stampContainer, documentLibrary, filePathPdf,
                                                                     convertingSettings, ConvertingModeType.Pdf, colorPrintType).
                                            Map(filesDataSource.ConcatResult),
                          filesDataSource => filesDataSource).
            WhereContinue(_ => ConvertingModeChoice.IsPrintConvertingNeed(convertingSettings.ConvertingModeTypes),
                         filesDataSource => StampContainerPrint(stampContainer, documentLibrary, filePathPdf,
                                                                convertingSettings, ConvertingModeType.Print, colorPrintType).
                                            Map(filesDataSource.ConcatResult),
                         filesDataSource => filesDataSource).
            Void(_ => documentLibrary.DetachAdditional());


        /// <summary>
        /// Получить путь к сохраненному файлу для обработки
        /// </summary>        
        private static IResultCollection<IFileDataSourceServer> GetSavedFileDataSource(IStampContainer stampContainer, IDocumentLibrary documentLibrary,
                                                                                       IFilePath filePath) =>
            new FileDataSourceServer(documentLibrary.FullName, filePath.FilePathClient, ConvertingModeType.Main,
                                     stampContainer.GetStampsToPrint().Value?.Select(stamp => stamp.PaperSize) ?? Enumerable.Empty<StampPaperSizeType>()).
            Map(fileDataSource => new ResultValue<IFileDataSourceServer>(fileDataSource).ToResultCollection());

        /// <summary>
        /// Произвести печать PDF
        /// </summary>
        private IResultCollection<IFileDataSourceServer> StampContainerCreatePdf(IStampContainer stampContainer, IDocumentLibrary documentLibrary,
                                                                                 IFilePath filePath, IConvertingSettings convertingSettings,
                                                                                 ConvertingModeType convertingModeType, ColorPrintType colorPrintType) =>
            stampContainer.
            Void(_ => _messagingService.ShowMessage("Вставка подписей...")).
            Map(_ => stampContainer.InsertSignatures().ToResultCollectionFromApplication()).
            ResultVoidOk(signatures => _loggerService.LogByObjects(LoggerLevel.Debug, LoggerAction.Operation, ReflectionInfo.GetMethodBase(this),
                                                                   signatures.Select(signature => signature.SignatureLibrary.PersonInformation.FullName))).
            ResultValueOkBind(signatures => StampContainerPrinting(stampContainer.GetStampsToPrint().ToResultCollectionFromApplication(),
                                                                   documentLibrary, filePath, convertingSettings, convertingModeType, colorPrintType).
                                            Void(_ => _messagingService.ShowMessage("Удаление подписей...")).
                                            Void(_ => stampContainer.DeleteSignatures(signatures)).
                                            Void(_ => _loggerService.LogByObject(LoggerLevel.Debug, LoggerAction.Operation,
                                                                                 ReflectionInfo.GetMethodBase(this), nameof(stampContainer.DeleteSignatures)))).
            ToResultCollection();

        /// <summary>
        /// Произвести печать
        /// </summary>
        private IResultCollection<IFileDataSourceServer> StampContainerPrint(IStampContainer stampContainer, IDocumentLibrary documentLibrary,
                                                                             IFilePath filePath, IConvertingSettings convertingSettings,
                                                                             ConvertingModeType convertingModeType, ColorPrintType colorPrintType) =>
            stampContainer.GetStampsToPrint().ToResultCollectionFromApplication().
            ResultValueOkBind(signatures => StampContainerPrinting(stampContainer.GetStampsToPrint().ToResultCollectionFromApplication(),
                                                                   documentLibrary, filePath, convertingSettings, convertingModeType,
                                                                   colorPrintType)).
            ToResultCollection();

        /// <summary>
        /// Печать штампов
        /// </summary>
        [Logger]
        private IResultCollection<IFileDataSourceServer> StampContainerPrinting(IResultCollection<IStamp> stampsToPrint, IDocumentLibrary documentLibrary,
                                                                                IFilePath filePath, IConvertingSettings convertingSettings,
                                                                                ConvertingModeType convertingModeType, ColorPrintType colorPrintType) =>
            stampsToPrint.
            ResultValueOkBind(stamps => StampFilePath.GetFileNamesByNamingType(stamps, filePath.FileNameWithoutExtensionClient, convertingSettings.PdfNamingType).
                                        ResultValueOk(fileNames => stamps.Zip(fileNames, (stamp, fileName) => (stamp, fileName)))).
            ResultValueOkBind(stampsFileName => CreatePrintingCollection(stampsFileName, documentLibrary, filePath, 
                                                                         convertingModeType, colorPrintType, 
                                                                         convertingSettings.PrintersInformation)).
            ToResultCollection();

        /// <summary>
        /// Создать коллекцию для печати
        /// </summary>
        private IResultCollection<IFileDataSourceServer> CreatePrintingCollection(IEnumerable<(IStamp Stamp, string FileName)> stampsFileName,
                                                                                  IDocumentLibrary documentLibrary, IFilePath filePath,
                                                                                  ConvertingModeType convertingModeType, ColorPrintType colorPrintType,
                                                                                  IResultValue<IPrintersInformation> printersInformation) =>
            stampsFileName.
            Select(stampFileName => filePath.ChangeServerName(stampFileName.FileName).ChangeClientName(stampFileName.FileName).
                Map(filePathChanged => CreatePrintingService(documentLibrary, stampFileName.Stamp, filePathChanged,
                                                             convertingModeType, colorPrintType, 
                                                             printersInformation.ResultValueOkBind(printers => 
                                                                printers.GetPrinter(convertingModeType, colorPrintType,
                                                                                    stampFileName.Stamp.PaperSize))))).
            ToResultCollection();

        /// <summary>
        /// Печать
        /// </summary>
        private IResultValue<IFileDataSourceServer> CreatePrintingService(IDocumentLibrary documentLibrary, IStamp stamp, IFilePath filePath,
                                                                          ConvertingModeType convertingModeType, ColorPrintType colorPrintType, 
                                                                          IResultValue<IPrinterInformation> printerInformation) =>
            printerInformation.
            ResultVoidOk(printer => _messagingService.ShowMessage($"Установка принтера {printer.Name}")).
            ResultVoidOk(printer => SetDefaultPrinter(printer.Name)).
            ResultVoidOk(printer => _messagingService.ShowMessage($"Печать файла {filePath.FileNameClient}")).
            ResultValueOkBind(_ => PrintCommand(documentLibrary, stamp, filePath.FilePathServer, convertingModeType, colorPrintType,
                                                printerInformation.Value.PrefixSearchPaperSize)).
            ResultVoidOk(_ => _loggerService.LogByObject(LoggerLevel.Debug, LoggerAction.Operation, ReflectionInfo.GetMethodBase(this), filePath.FilePathServer)).
            ResultValueOk(_ => new FileDataSourceServer(filePath.FilePathServer, filePath.FilePathClient, convertingModeType,
                                                        stamp.PaperSize, printerInformation.Value.Name));

        /// <summary>
        /// Команда печати
        /// </summary>
        [Logger]
        private IResultError PrintCommand(IDocumentLibrary documentLibrary, IStamp stamp, string filePath,
                                          ConvertingModeType convertingModeType, ColorPrintType colorPrintType,
                                          string prefixSearchPaperSize) =>
            PrintCommandFunc(documentLibrary, stamp, colorPrintType, prefixSearchPaperSize).
            Map(printCommand => convertingModeType switch
            {
                ConvertingModeType.Pdf => _pdfCreatorService.PrintPdfWithExecuteAction(filePath, printCommand).ToResult(),
                ConvertingModeType.Print => printCommand(),
                _ => new ResultError(new ErrorCommon(ErrorConvertingType.PdfPrintingError, "Режим печати не установлен"))
            });

        /// <summary>
        /// Функция печати
        /// </summary>
        private static Func<IResultError> PrintCommandFunc(IDocumentLibrary documentLibrary, IStamp stamp,
                                                       ColorPrintType colorPrintType, string prefixSearchPaperSize) =>
            () => documentLibrary.PrintStamp(stamp, colorPrintType.ToApplication(), prefixSearchPaperSize).ToResultFromApplication();

        /// <summary>
        /// Установить принтер по умолчанию
        /// </summary>       
        private static void SetDefaultPrinter(string printerName) =>
            PrinterSettings.InstalledPrinters.Cast<string>().
            WhereNull(printersInstall => printersInstall.Any(printerInstall => printerInstall.ContainsIgnoreCase(printerName)) &&
                                         NativeMethods.SetDefaultPrinter(printerName).
                                         Void(_ => _loggerService.DebugLog($"Set printer {printerName}")));
    }
}
