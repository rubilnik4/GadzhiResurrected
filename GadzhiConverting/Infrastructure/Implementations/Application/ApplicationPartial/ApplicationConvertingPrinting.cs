using ConvertingModels.Models.Enums;
using ConvertingModels.Models.Interfaces.FilesConvert;
using ConvertingModels.Models.Interfaces.StampCollections;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiConverting.Infrastructure.Interfaces.Application.ApplicationPartial;
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

namespace GadzhiConverting.Infrastructure.Implementations.Application.ApplicationPartial
{
    /// <summary>
    /// Подкласс для выполнения печати
    /// </summary>
    public partial class ApplicationConverting : IApplicationConvertingPrinting
    {
        /// <summary>
        /// Найти все доступные штампы на всех листах. Начать обработку каждого из них
        /// </summary>       
        private (IEnumerable<IFileDataSourceServer>, IEnumerable<ErrorConverting>) CreatePdfInDocument(string filePath, ColorPrint colorPrint, string pdfPrinterName)
        {
            if (_applicationLibrary.StampWord.IsValid)
            {
                var fileDataSourceAndErrors = _applicationLibrary.StampWord.Stamps?.Where(stamp => stamp.StampType == StampType.Main).
                                                               Select(stamp => CreatePdfWithSignatures(stamp, filePath, colorPrint, pdfPrinterName));
                return (fileDataSourceAndErrors.Select(fileWithErrors => fileWithErrors.fileSource),
                        fileDataSourceAndErrors.SelectMany(fileWithErrors => fileWithErrors.errors));
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
        private (IFileDataSourceServer fileSource, IEnumerable<ErrorConverting> errors) CreatePdfWithSignatures(IStamp stamp, string filePath, ColorPrint colorPrint, string pdfPrinterName)
        {
            //_messagingService.ShowAndLogMessage($"Обработка штампа {stamp.Name}");
            //stamp.CompressFieldsRanges();

            _applicationLibrary.InsertStampSignatures();

            //_messagingService.ShowAndLogMessage($"Создание PDF для штампа {stamp.Name}");
            var fileDataSourceAndErrors = CreatePdf(filePath, colorPrint, stamp.PaperSize, pdfPrinterName);

            _applicationLibrary.DeleteStampSignatures();

            return fileDataSourceAndErrors;
        }

        /// <summary>
        /// Печать пдф
        /// </summary>
        private (IFileDataSourceServer, IEnumerable<ErrorConverting>) CreatePdf(string filePath, ColorPrint colorPrint,
                                                                                string paperSize, string pdfPrinterName)
        {
            (IFileDataSourceServer fileDataSourceServer, IEnumerable<ErrorConverting> errorsConverting) = (null, null);

            (bool isSetPrinter, ErrorConverting errorPrinter) = SetDefaultPrinter(pdfPrinterName);
            errorsConverting = errorsConverting.Concat(new List<ErrorConverting>() { errorPrinter });

            if (isSetPrinter)
            {
                var errorPdf = PrintPdfCommand(filePath);
                errorsConverting = errorsConverting.Concat(errorPdf);

                fileDataSourceServer = new FileDataSourceServer(filePath, FileExtention.pdf, paperSize, pdfPrinterName);
            }
            return (fileDataSourceServer, errorsConverting);
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
        private IEnumerable<ErrorConverting> PrintPdfCommand(string filePath) =>
                _pdfCreatorService.PrintPdfWithExecuteAction(filePath, _applicationLibrary.PrintCommand).ErrorsConverting;
    }
}
