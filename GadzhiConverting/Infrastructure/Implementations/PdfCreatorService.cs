using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extentions.Functional;
using GadzhiCommon.Extentions.StringAdditional;
using GadzhiCommon.Functional;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Extensions;
using GadzhiConverting.Infrastructure.Interfaces;
using GadzhiConverting.Models.Implementations;
using GadzhiConverting.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        { }

        /// <summary>
        /// Инициализировать модуль PDFCreator
        /// </summary>
        private PDFCreator.clsPDFCreator PdrCreatorInitialize()
        {
            KillAllPreviousProcess();
            _pdfCreator = new PDFCreator.clsPDFCreator();
            _pdfCreator.eReady += new PDFCreator.__clsPDFCreator_eReadyEventHandler(PdfCreatorReady);
            return _pdfCreator;
        }

        /// <summary>
        /// Время ожидания на создание PDF файла
        /// </summary>
        private int WaitingPrintingLimitInSeconds => 60;

        /// <summary>
        /// Создать PDF файл с выполнением отложенной печати 
        /// </summary>       
        public IResultConverting PrintPdfWithExecuteAction(string filePath, Func<IResultConverting> printFunction) =>
            SetPrinterOptions(filePath).
            WhereContinue(result => result.OkStatus,
            okFunc: result => printFunction?.Invoke().
                              WhereContinue(resultPrint => resultPrint.OkStatus,
                                            okFunc: resultPrint => PrintPdf(),
                                            badFunc: resultPrint => resultPrint)
                              ?? new ErrorConverting(FileConvertErrorType.PdfPrintingError, "Функция печати не задана").
                                Map(error => new ResultConverting(error)),
            badFunc: result => result);

        /// <summary>
        /// Установить опции печати
        /// </summary>   
        private IResultConverting SetPrinterOptions(string filePath) =>
            PdrCreatorInitialize().
            WhereNull(pdfCreator => pdfCreator.cStart("/NoProcessingAtStartup", false))?.
            Map(pdfCreator =>
                FileSystemOperations.ExtensionWithoutPointFromPath(filePath).
                WhereNull(fileExtension => !String.IsNullOrEmpty(filePath) || !String.IsNullOrEmpty(fileExtension) ||
                                          FileExtention.pdf.ToString().ContainsIgnoreCase(fileExtension))?.
                Map(fileExtension =>
                {
                    pdfCreator.cOptions = GetPdfPrinterOptions(filePath);
                    pdfCreator.cClearCache();
                    return new ResultConverting();
                })
                ?? new ErrorConverting(FileConvertErrorType.PdfPrintingError, $"Некорретно задан путь сохранения {filePath}").
                   Map(error => new ResultConverting(error))
            ) ?? new ErrorConverting(FileConvertErrorType.PdfPrintingError, "Ошибка инициализации PDF принтера").
                Map(error => new ResultConverting(error));

        /// <summary>
        /// Напечатать PDF
        /// </summary>
        private IResultConverting PrintPdf()
        {
            _readyState = false;
            _pdfCreator.cPrinterStop = false;

            int waitingLimit = 0;
            while (!_readyState && waitingLimit < WaitingPrintingLimitInSeconds)
            {
                waitingLimit += 1;
                Thread.Sleep(1000);
            }

            _pdfCreator.cPrinterStop = true;
            _pdfCreator.cClose();

            return !_readyState ?
                   new ResultConverting() :
                   new ResultConverting(new ErrorConverting(FileConvertErrorType.PdfPrintingError, "Время создания PDF файла истекло"));
        }

        /// <summary>
        /// Установить опции печати PDF
        /// </summary>       
        private PDFCreator.clsPDFCreatorOptions GetPdfPrinterOptions(string filePath)
        {
            var pdfCreatorOptions = _pdfCreator.cOptions;
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

        /// <summary>
        /// Уничтожить все предыдущие процессы
        /// </summary>
        private static IEnumerable<string> KillAllPreviousProcess() =>
            Process.GetProcesses().
            Where(process => process.ProcessName.ContainsIgnoreCase("pdfcreator")).
            Select(process =>
            {
                string proccessName = process.ProcessName;
                process.Kill();
                return proccessName;
            }).
            ToList();

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
