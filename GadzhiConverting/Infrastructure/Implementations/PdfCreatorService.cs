using GadzhiApplicationCommon.Models.Interfaces;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extentions.StringAdditional;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Extensions;
using GadzhiConverting.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace GadzhiConverting.Infrastructure.Implementations
{
    /// <summary>
    /// Управление печатью пдф
    /// </summary>
    public class PdfCreatorService : IPdfCreatorService
    {
        /// <summary>
        /// Библиотека Pdf Creator
        /// </summary>
        private PDFCreator.clsPDFCreator _pdfCreator;

        /// <summary>
        /// Маркер готовности
        /// </summary>
        private bool _readyState = false;

        public PdfCreatorService()
        {

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
        /// Создать PDF файл с выполнением отложенной печати 
        /// </summary>       
        public (bool, IErrorConverting) PrintPdfWithExecuteAction(string filePath, Func<IErrorApplication> printFunction)
        {
            bool success = false;

            (bool isValidSetOptions, IErrorConverting errorConverting) = SetPrinterOptions(filePath);
            if (isValidSetOptions)
            {
                if (printFunction != null)
                {
                    errorConverting = printFunction.Invoke().ToErrorConverting();
                    if (errorConverting == null)
                    {
                        (success, errorConverting) = PrintPdf();
                    }
                }
                else
                {
                    errorConverting = new ErrorConverting(FileConvertErrorType.PdfPrintingError, "Функция печати не задана");
                }
            }

            return (success, errorConverting);
        }

        /// <summary>
        /// Установить опции печати
        /// </summary>   
        private (bool, IErrorConverting) SetPrinterOptions(string filePath)
        {
            bool success = false;
            IErrorConverting errorConverting = null;

            PdrCreatorInitialize();

            if (_pdfCreator.cStart("/NoProcessingAtStartup", false))
            {
                string fileExtension = FileSystemOperations.ExtensionWithoutPointFromPath(filePath);

                if (!String.IsNullOrEmpty(filePath) || !String.IsNullOrEmpty(fileExtension) ||
                    FileExtention.pdf.ToString().ContainsIgnoreCase(fileExtension))
                {
                    _pdfCreator.cOptions = GetPdfPrinterOptions(filePath);
                    _pdfCreator.cClearCache();

                    success = true;
                }
                else
                {
                    errorConverting = new ErrorConverting(FileConvertErrorType.PdfPrintingError, $"Некорретно задан путь сохранения {filePath}");
                }
            }
            else
            {
                errorConverting = new ErrorConverting(FileConvertErrorType.PdfPrintingError, "Ошибка инициализации PDF принтера");
            }

            return (success, errorConverting);
        }

        /// <summary>
        /// Напечатать PDF
        /// </summary>
        private (bool, IErrorConverting) PrintPdf()
        {
            _readyState = false;
            _pdfCreator.cPrinterStop = false;

            int waitingLimit = 0;
            while (!_readyState && waitingLimit < WaitingPrintingLimitInSeconds)
            {
                waitingLimit += 1;
                Thread.Sleep(1000);
            }

            bool success = !_readyState;
            IErrorConverting errorConverting = null;
            if (_readyState)
            {
                errorConverting = (new ErrorConverting(FileConvertErrorType.PdfPrintingError, "Время создания PDF файла истекло"));
            }
            _pdfCreator.cPrinterStop = true;
            _pdfCreator.cClose();

            return (success, errorConverting);
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
