using ConvertingModels.Models.Interfaces.FilesConvert;
using GadzhiApplicationCommon.Models.Enums;
using GadzhiApplicationCommon.Models.Interfaces.StampCollections;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Implementations.Errors;
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
        private (IEnumerable<IFileDataSourceServer>, IEnumerable<ErrorConverting>) CreatePdfInDocument(string filePath, ColorPrint colorPrint, 
                                                                                                       IPrinterInformation pdfPrinterInformation)
        {
            if (_applicationLibrary.StampContainer.IsValid)
            {
                var fileDataSourceAndErrors = _applicationLibrary.StampContainer.Stamps?.Where(stamp => stamp.StampType == StampType.Main).
                                                                  Select(stamp => CreatePdfWithSignatures(stamp, filePath, colorPrint, pdfPrinterInformation));
                return (fileDataSourceAndErrors.Select(fileWithErrors => fileWithErrors.fileSource),
                        fileDataSourceAndErrors.Select(fileWithErrors => fileWithErrors.errors));
            }
            else
            {
                return (null, new List<ErrorConverting>() { new ErrorConverting(FileConvertErrorType.StampNotFound,
                                                            $"Штампы в файле {Path.GetFileName(filePath)} не найдены")});
            }
        }

        /// <summary>
        /// Создать PDF для штампа, вставить подписи
        /// </summary>       
        private (IFileDataSourceServer fileSource, ErrorConverting errors) CreatePdfWithSignatures(IStamp stamp, string filePath,
                                                                                                   ColorPrint colorPrint, 
                                                                                                   IPrinterInformation pdfPrinterInformation)
        {
            stamp.CompressFieldsRanges();

            stamp.InsertSignatures();
            var fileDataSourceAndError = CreatePdf(stamp, filePath, colorPrint, stamp.PaperSize, pdfPrinterInformation);
            stamp.DeleteSignatures();

            return fileDataSourceAndError;
        }

        /// <summary>
        /// Печать пдф
        /// </summary>
        private (IFileDataSourceServer, ErrorConverting) CreatePdf(IStamp stamp, string filePath, ColorPrint colorPrint,
                                                                   string paperSize, IPrinterInformation pdfPrinterInformation)
        {
            IFileDataSourceServer fileDataSourceServer = null;

            (bool isSetPrinter, ErrorConverting errorConverting) = SetDefaultPrinter(pdfPrinterInformation.Name);
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
        private (bool, ErrorConverting) SetDefaultPrinter(string printerName)
        {
            bool success = false;
            ErrorConverting errorConverting = null;

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
        private ErrorConverting PrintPdfCommand(IStamp stamp, string filePath, ColorPrint colorPrint, string prefixSearchPaperSize)
        {
            var printCommand = new Action(() => _applicationLibrary.PrintStamp(stamp, colorPrint.ToApplication(), prefixSearchPaperSize));
            return _pdfCreatorService.PrintPdfWithExecuteAction(filePath, printCommand).ErrorConverting;
        }       
    }
}
