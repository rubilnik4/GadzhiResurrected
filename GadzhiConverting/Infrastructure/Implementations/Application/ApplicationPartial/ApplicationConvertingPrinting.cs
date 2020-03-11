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
        /// Установить принтер по умолчанию
        /// </summary>       
        private bool SetDefaultPrinter(IPrinterInformation printerInformation)
        {
            bool success = false;
            if (PrinterSettings.InstalledPrinters?.Cast<string>()?.Contains(printerInformation?.Name,
                                                                          StringComparer.OrdinalIgnoreCase) == true)
            {
                if (NativeMethods.SetDefaultPrinter(printerInformation?.Name))
                {
                    success = true;
                }
                else
                {
                    _messagingService.ShowAndLogError(new ErrorConverting(FileConvertErrorType.PrinterNotInstall,
                                                                      $"Не удалось установить принтер {printerInformation?.Name}"));
                }
            }
            else
            {
                _messagingService.ShowAndLogError(new ErrorConverting(FileConvertErrorType.PrinterNotInstall,
                                                                        $"Принтер {printerInformation?.Name} не установлен в системе"));
            }

            return success;
        }

        /// <summary>
        /// Команда печати PDF
        /// </summary>
        private bool PrintPdfCommand(string filePath) => _pdfCreatorService.PrintPdfWithExecuteAction(filePath, _applicationLibrary.PrintCommand);

        /// <summary>
        /// Найти все доступные штампы на всех листах. Начать обработку каждого из них
        /// </summary>       
        private IEnumerable<IFileDataSourceServer> CreatePdfInDocument(string filePath, ColorPrint colorPrint, string pdfPrinterName)
        {
            if (_applicationLibrary.StampWord.IsValid)
            {
                return _applicationLibrary.StampWord.Stamps?.Where(stamp => stamp.StampType == StampType.Main).
                                                     Select(stamp => CreatePdfWithSignatures(stamp, filePath, colorPrint, pdfPrinterName));
            }
            else
            {
                _messagingService.ShowAndLogError(new ErrorConverting(FileConvertErrorType.StampNotFound,
                                                                  $"Штампы в файле {Path.GetFileName(filePath)} не найдены"));
                return null;
            }
        }

        /// <summary>
        /// Создать PDF для штампа, вставить подписи
        /// </summary>       
        private IFileDataSourceServer CreatePdfWithSignatures(IStamp stamp, string filePath, ColorPrint colorPrint, string pdfPrinterName)
        {
            _messagingService.ShowAndLogMessage($"Обработка штампа {stamp.Name}");
            //stamp.CompressFieldsRanges();

            _applicationLibrary.InsertStampSignatures();

            _messagingService.ShowAndLogMessage($"Создание PDF для штампа {stamp.Name}");
            IFileDataSourceServer fileDataSourceServerWord = CreatePdfByStamp(stamp, filePath, colorPrint, pdfPrinterName);

            _applicationLibrary.DeleteStampSignatures();

            return fileDataSourceServerWord;
        }

        /// <summary>
        /// Создать пдф по координатам и формату
        /// </summary>
        private IFileDataSourceServer CreatePdfByStamp(IStamp stamp, string filePath, ColorPrint colorPrint, string pdfPrinterName)
        {
            if (stamp != null)
            {
                return CreatePdf(filePath, colorPrint, stamp.PaperSize, pdfPrinterName);
            }
            else
            {
                throw new ArgumentNullException(nameof(stamp));
            }
        }


        /// <summary>
        /// Создать пдф по координатам и формату
        /// </summary>
        private IFileDataSourceServer CreatePdf(string filePath, ColorPrint colorPrint, string paperSize, string pdfPrinterName)
        {           
            if (NativeMethods.SetDefaultPrinter(pdfPrinterName))
            {
                _applicationLibrary.PrintCommand();
                return new FileDataSourceServerConverting(filePath, FileExtention.pdf, paperSize, pdfPrinterName);
            }
            return null;
        }
    }
}
