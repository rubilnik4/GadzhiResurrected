using ConvertingModels.Models.Interfaces.FilesConvert;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiConverting.Models.Interfaces.Printers;
using GadzhiWord.Helpers.Implementations;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Implementations.Application.ApplicationWordPartial
{
    /// <summary>
    /// Печать Word
    /// </summary>
    public partial class ApplicationConverting : IApplicationConvertingPrinting
    {
        /// <summary>
        /// Установить принтер по умолчанию
        /// </summary>       
        public bool SetDefaultPrinter(IPrinterInformation printerInformation)
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

        ///// <summary>
        ///// Установить принтер PDF по умолчанию
        ///// </summary>       
        //public string SetDefaultPdfPrinter()
        //{
        //    IPrinterInformation pdfPrinterInformation = _wordProject.PrintersInformation.PrintersPdf.FirstOrDefault();
        //    SetDefaultPrinter(pdfPrinterInformation);
        //    return pdfPrinterInformation.Name;
        //}       

        /// <summary>
        /// Команда печати PDF
        /// </summary>
        public bool PrintPdfCommand(string filePath) => _pdfCreatorService.PrintPdfWithExecuteAction(filePath, _applicationLibrary.PrintCommand);

        /// <summary>
        /// Найти все доступные штампы на всех листах. Начать обработку каждого из них
        /// </summary>       
        public IEnumerable<IFileDataSourceServer> CreatePdfInDocument(string filePath, ColorPrint colorPrint)
        {
            if (StampWord.IsValid)
            {
                return StampWord.Stamps?.Where(stamp => stamp.StampType == StampType.Main).
                                         Select(stamp => CreatePdfWithSignatures(stamp, filePath, colorPrint));
            }
            else
            {
                //_messagingService.ShowAndLogError(new ErrorConverting(FileConvertErrorType.StampNotFound,
                //                                                  $"Штампы в файле {Path.GetFileName(filePath)} не найдены"));
                return null;
            }
        }

        /// <summary>
        /// Создать PDF для штампа, вставить подписи
        /// </summary>       
        private IFileDataSourceServerWord CreatePdfWithSignatures(IStamp stamp, string filePath, ColorPrint colorPrint)
        {
            //_messagingService.ShowAndLogMessage($"Обработка штампа {stamp.Name}");
            //stamp.CompressFieldsRanges();

            InsertStampSignatures();

            //_messagingService.ShowAndLogMessage($"Создание PDF для штампа {stamp.Name}");
            IFileDataSourceServerWord fileDataSourceServerWord = CreatePdfByStamp(stamp, filePath, colorPrint);

            DeleteStampSignatures();

            return fileDataSourceServerWord;
        }

        /// <summary>
        /// Создать пдф по координатам и формату
        /// </summary>
        private IFileDataSourceServerWord CreatePdfByStamp(IStamp stamp, string filePath, ColorPrint colorPrint)
        {
            if (stamp != null)
            {
                return CreatePdf(filePath, colorPrint, stamp.PaperSize);
            }
            else
            {
                throw new ArgumentNullException(nameof(stamp));
            }
        }


        /// <summary>
        /// Создать пдф по координатам и формату
        /// </summary>
        private IFileDataSourceServerWord CreatePdf(string filePath, ColorPrint colorPrint, string paperSize)
        {
            string pdfPrinterName = _applicationWord.SetDefaultPdfPrinter();
            if (!String.IsNullOrWhiteSpace(pdfPrinterName))
            {
                _applicationWord.PrintPdfCommand(filePath);
                return new FileDataSourceServerWord(filePath, FileExtention.pdf, paperSize, pdfPrinterName);
            }
            return null;
        }
    }
}
