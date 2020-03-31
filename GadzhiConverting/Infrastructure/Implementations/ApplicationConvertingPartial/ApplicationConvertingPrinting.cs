using ConvertingModels.Models.Interfaces.FilesConvert;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extentions.Collection;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Extensions;
using GadzhiConverting.Infrastructure.Interfaces.ApplicationConvertingPartial;
using GadzhiConverting.Models.Implementations.FilesConvert;
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
        private (IEnumerable<IFileDataSourceServer>, IEnumerable<IErrorConverting>) CreatePdfInDocument(string filePath, ColorPrint colorPrint,
                                                                                                       IPrinterInformation pdfPrinterInformation)
        {
            if (ActiveLibrary.StampContainer.IsValid)
            {
                var fileDataSourceAndErrors = ActiveLibrary.StampContainer.Stamps?.
                                              Select(stamp => CreatePdfWithSignatures(stamp, filePath, colorPrint, pdfPrinterInformation)).
                                              ToList();
                return (fileDataSourceAndErrors.Select(fileWithErrors => fileWithErrors.fileSource),
                        fileDataSourceAndErrors.SelectMany(fileWithErrors => fileWithErrors.errors));
            }
            else
            {
                return (null, new List<IErrorConverting>() { new ErrorConverting(FileConvertErrorType.StampNotFound,
                                                            $"Штампы в файле {Path.GetFileName(filePath)} не найдены")});
            }
        }

        /// <summary>
        /// Создать PDF для штампа, вставить подписи
        /// </summary>       
        private (IFileDataSourceServer fileSource, IEnumerable<IErrorConverting> errors) CreatePdfWithSignatures(IStamp stamp, string filePath,
                                                                                                                 ColorPrint colorPrint,
                                                                                                                 IPrinterInformation pdfPrinterInformation)
        {
            stamp.CompressFieldsRanges();

            var signaturesErrors = stamp.InsertSignatures().Select(error => error.ToErrorConverting());
            var fileDataSourceAndError = CreatePdf(stamp, filePath, colorPrint, stamp.PaperSize, pdfPrinterInformation);
            stamp.DeleteSignatures();

            var errors = (fileDataSourceAndError.Errors == null) ?
                          signaturesErrors :
                          signaturesErrors.UnionNotNull(fileDataSourceAndError.Errors);

            return (fileDataSourceAndError.FileDataSource, errors);
        }

        /// <summary>
        /// Печать пдф
        /// </summary>
        private (IFileDataSourceServer FileDataSource, IEnumerable<IErrorConverting> Errors) CreatePdf(IStamp stamp, string filePath, ColorPrint colorPrint,
                                                                                        string paperSize, IPrinterInformation pdfPrinterInformation)
        {
            IFileDataSourceServer fileDataSourceServer = null;

            (bool isSetPrinter, IEnumerable<IErrorConverting> errorConverting) = SetDefaultPrinter(pdfPrinterInformation.Name);
            if (isSetPrinter)
            {
                errorConverting = PrintPdfCommand(stamp, filePath, colorPrint, pdfPrinterInformation.PrefixSearchPaperSize);
                fileDataSourceServer = new FileDataSourceServer(filePath, FileExtention.pdf, paperSize, pdfPrinterInformation.Name);
            }
            return (fileDataSourceServer, errorConverting);
        }

        /// <summary>
        /// Установить принтер по умолчанию
        /// </summary>       
        private (bool, IErrorConverting) SetDefaultPrinter(string printerName)
        {
            bool success = false;
            IErrorConverting errorConverting = null;

            if (PrinterSettings.InstalledPrinters?.Cast<string>()?.Contains(printerName, StringComparer.OrdinalIgnoreCase) == true)
            {
                if (NativeMethods.SetDefaultPrinter(printerName))
                {
                    success = true;
                }
                else
                {
                    errorConverting = new ErrorConverting(FileConvertErrorType.PrinterNotInstall,
                                                          $"Не удалось установить принтер {printerName}");
                }
            }
            else
            {
                errorConverting = new ErrorConverting(FileConvertErrorType.PrinterNotInstall,
                                                      $"Принтер {printerName} не установлен в системе");
            }

            return (success, errorConverting);
        }

        /// <summary>
        /// Команда печати PDF
        /// </summary>
        private IEnumerable<IErrorConverting> PrintPdfCommand(IStamp stamp, string filePath, ColorPrint colorPrint, string prefixSearchPaperSize)
        {
            var printCommand = new Func<IEnumerable<IErrorApplication>>(() => ActiveLibrary.PrintStamp(stamp, colorPrint.ToApplication(), prefixSearchPaperSize));
            return _pdfCreatorService.PrintPdfWithExecuteAction(filePath, printCommand).ErrorsConverting;
        }
    }
}
