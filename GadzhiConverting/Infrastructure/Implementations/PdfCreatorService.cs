using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extentions.Functional;
using GadzhiCommon.Extentions.Functional.Result;
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
            var pdfCreator = new PDFCreator.clsPDFCreator();
            return pdfCreator;
        }

        /// <summary>
        /// Время ожидания на создание PDF файла
        /// </summary>
        private int _waitingPrintingLimitInSeconds => 60;

        /// <summary>
        /// Создать PDF файл с выполнением отложенной печати 
        /// </summary>       
        public IResultError PrintPdfWithExecuteAction(string filePath, Func<IResultError> printFunction) =>
            SetPrinterOptions(filePath).
            ResultValueOkBind(pdfCreator => new ResultValue<PDFCreator.clsPDFCreator>(pdfCreator)).
            ResultValueOkBind(pdfCreator => new ResultValue<Unit>(Unit.Value, PrintPdf(pdfCreator, printFunction).Errors)).
            ToResult();

        /// <summary>
        /// Установить опции печати
        /// </summary>   
        private IResultValue<PDFCreator.clsPDFCreator> SetPrinterOptions(string filePath) =>
            new ResultValue<PDFCreator.clsPDFCreator>(PdrCreatorInitialize()).
            ResultValueContinue(pdfCreator => pdfCreator.cStart("/NoProcessingAtStartup", false),
                okFunc: pdfCreator => pdfCreator,
                badFunc: _ => new ErrorCommon(FileConvertErrorType.PdfPrintingError, "Ошибка инициализации PDF принтера")).
            ResultValueContinue(pdfCreator => PdfPathValid(filePath),
                okFunc: pdfCreator =>
                {
                    pdfCreator.cOptions = GetPdfPrinterOptions(pdfCreator, filePath);
                    pdfCreator.cClearCache();
                    return pdfCreator;
                },
                badFunc: _ => new ErrorCommon(FileConvertErrorType.PdfPrintingError, $"Некорретно задан путь сохранения {filePath}"));

        /// <summary>
        /// Корректнет ли путь для сохранения pdf
        /// </summary>        
        private bool PdfPathValid(string filePath) =>
            FileSystemOperations.ExtensionWithoutPointFromPath(filePath).
            Map(fileExtension => !String.IsNullOrEmpty(filePath) && !String.IsNullOrEmpty(fileExtension) &&
                                  FileExtention.pdf.ToString().ContainsIgnoreCase(fileExtension));

        /// <summary>
        /// Напечатать PDF
        /// </summary>
        private IResultError PrintPdf(PDFCreator.clsPDFCreator pdfCreator, Func<IResultError> printFunction)
        {
            pdfCreator.eReady += new PDFCreator.__clsPDFCreator_eReadyEventHandler(PdfCreatorReady);
            _readyState = false;
            pdfCreator.cPrinterStop = false;

            var printFunctionResult = printFunction.Invoke();
            int waitingLimit = 0;
            while (!_readyState && waitingLimit < _waitingPrintingLimitInSeconds)
            {
                waitingLimit += 1;
                Thread.Sleep(1000);
            }

            pdfCreator.cPrinterStop = true;
            pdfCreator.cClose();
            pdfCreator = null;

            return _readyState ?
                   new ResultError() :
                   new ErrorCommon(FileConvertErrorType.PdfPrintingError, "Время создания PDF файла истекло").
                   ToResult().
                   ConcatErrors(printFunctionResult.Errors);
        }

        /// <summary>
        /// Установить опции печати PDF
        /// </summary>       
        private PDFCreator.clsPDFCreatorOptions GetPdfPrinterOptions(PDFCreator.clsPDFCreator pdfCreator, string filePath)
        {
            var pdfCreatorOptions = pdfCreator.cOptions;
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
    }
}
