using GadzhiMicrostation.Extentions.StringAdditional;
using GadzhiMicrostation.Infrastructure.Interfaces;
using GadzhiMicrostation.Models.Enums;
using GadzhiMicrostation.Models.Implementations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace GadzhiMicrostation.Infrastructure.Implementations
{
    /// <summary>
    /// Управление печатью пдф
    /// </summary>
    public class PdfCreatorService : IPdfCreatorService
    {
        /// <summary>
        /// Сервис работы с ошибками
        /// </summary>
        private readonly IErrorMessagingMicrostation _errorMessagingMicrostation;
        
        /// <summary>
        /// Библиотека Pdf Creator
        /// </summary>
        private PDFCreator.clsPDFCreator _pdfCreator;

        /// <summary>
        /// Маркер готовности
        /// </summary>
        private bool _readyState = false;

        public PdfCreatorService(IErrorMessagingMicrostation errorMessagingMicrostation)
        {
            _errorMessagingMicrostation = errorMessagingMicrostation;
        }

        /// <summary>
        /// Инициализировать модуль PDFCreator
        /// </summary>
        private void PdrCreatorInitialize()
        {
            _pdfCreator = new PDFCreator.clsPDFCreator();
            _pdfCreator.eReady += new PDFCreator.__clsPDFCreator_eReadyEventHandler(PdfCreatorReady);
        }

        /// <summary>
        /// Время ожидания на создание PDF файла
        /// </summary>
        private int WaitingPrintingLimitInSeconds => 60;

        /// <summary>
        /// Установить опции печати
        /// </summary>   
        public bool SetPrinterOptions(string filePath)
        {
            bool success = false;

            PdrCreatorInitialize();

            if (_pdfCreator.cStart("/NoProcessingAtStartup", false))
            {
                string fileExtension = FileSystemOperationsMicrostation.ExtensionWithoutPointFromPath(filePath);

                if (!String.IsNullOrEmpty(filePath) || !String.IsNullOrEmpty(fileExtension) ||
                    FileExtentionMicrostation.pdf.ToString().ContainsIgnoreCase(fileExtension))
                {
                    _pdfCreator.cOptions = GetPdfPrinterOptions(filePath);
                    _pdfCreator.cClearCache();

                    success = true;
                }
                else
                {
                    _errorMessagingMicrostation.AddError(new ErrorMicrostation(ErrorMicrostationType.PdfPrintingError,
                                                                       $"Некорретно задан путь сохранения {filePath}"));
                }
            }
            else
            {
                _errorMessagingMicrostation.AddError(new ErrorMicrostation(ErrorMicrostationType.PdfPrintingError,
                                                    "Ошибка инициализации PDF принтера"));
            }

            return success;
        }

        /// <summary>
        /// Напечатать PDF
        /// </summary>
        public bool PrintPdf()
        {
            _readyState = false;
            _pdfCreator.cPrinterStop = false;

            int waitingLimit = 0;
            while (!_readyState && waitingLimit < WaitingPrintingLimitInSeconds)
            {
                waitingLimit += 1;
                Thread.Sleep(1000);
            }

            bool success;
            if (!_readyState)
            {
                _errorMessagingMicrostation.AddError(new ErrorMicrostation(ErrorMicrostationType.PdfPrintingError,
                                                                   "Время создания PDF файла истекло"));
                success = false;
            }
            else
            {
                success = true;
            }
            _pdfCreator.cPrinterStop = true;
            _pdfCreator.cClose();

            return success;
        }

        /// <summary>
        /// СОздать PDF файл с выполнением отложенной печати 
        /// </summary>       
        public bool PrintPdfWithExecuteAction(string filePath, Action printAction)
        {
            bool success = false;

            bool isValidSetOptions = SetPrinterOptions(filePath);
            if (isValidSetOptions)
            {
                if (printAction != null)
                {
                    printAction.Invoke();
                }
                else
                {
                    _errorMessagingMicrostation.AddError(new ErrorMicrostation(ErrorMicrostationType.PdfPrintingError,
                                                                                   "Функция печати не задана"));
                }

                success = PrintPdf();
            }

            return success;
        }

        /// <summary>
        /// Установить опции печати PDF
        /// </summary>       
        private PDFCreator.clsPDFCreatorOptions GetPdfPrinterOptions(string filePath)
        {
            PDFCreator.clsPDFCreatorOptions pdfCreatorOptions = _pdfCreator.cOptions;
            pdfCreatorOptions.UseAutosave = 1;
            pdfCreatorOptions.UseAutosaveDirectory = 1;
            pdfCreatorOptions.AutosaveDirectory = Path.GetDirectoryName(filePath);
            pdfCreatorOptions.AutosaveFilename = Path.GetFileNameWithoutExtension(filePath);
            pdfCreatorOptions.AutosaveFormat = 0; // сделать PDF           

            return pdfCreatorOptions;
        }
        /// <summary>
        /// Событие готовности печати PDF
        /// </summary>
        private void PdfCreatorReady()
        {
            _pdfCreator.cPrinterStop = true;
            _readyState = true;
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }
                if (_pdfCreator?.cIsClosed == false)
                {
                    _pdfCreator?.cClose();
                }
                _pdfCreator = null;

                disposedValue = true;
            }
        }

        ~PdfCreatorService()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
