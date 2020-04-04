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
        private int WaitingPrintingLimitInSeconds => 60;

        /// <summary>
        /// Создать PDF файл с выполнением отложенной печати 
        /// </summary>       
        public IResultConverting PrintPdfWithExecuteAction(string filePath, Func<IResultConverting> printFunction) =>
            SetPrinterOptions(filePath).
            ResultValueOkBind(pdfCreator => new ResultConvertingValue<PDFCreator.clsPDFCreator>(pdfCreator, printFunction.Invoke().Errors)).
            ResultValueOkBind(pdfCreator => new ResultConvertingValue<Unit>(Unit.Value, PrintPdf(pdfCreator).Errors));

        /// <summary>
        /// Установить опции печати
        /// </summary>   
        private IResultConvertingValue<PDFCreator.clsPDFCreator> SetPrinterOptions(string filePath) =>
            new ResultConvertingValue<PDFCreator.clsPDFCreator>(PdrCreatorInitialize()).
            ResultContinue(pdfCreator => pdfCreator.cStart("/NoProcessingAtStartup", false),
                okFunc: pdfCreator => pdfCreator,
                badFunc: _ => new ErrorConverting(FileConvertErrorType.PdfPrintingError, "Ошибка инициализации PDF принтера")).
            ResultContinue(pdfCreator => PdfPathValid(filePath),
                okFunc: pdfCreator =>
                {
                    pdfCreator.cOptions = GetPdfPrinterOptions(pdfCreator, filePath);
                    pdfCreator.cClearCache();
                    return pdfCreator;
                },
                badFunc: _ => new ErrorConverting(FileConvertErrorType.PdfPrintingError, $"Некорретно задан путь сохранения {filePath}"));

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
        private IResultConverting PrintPdf(PDFCreator.clsPDFCreator pdfCreator)
        {
            pdfCreator.eReady += new PDFCreator.__clsPDFCreator_eReadyEventHandler(PdfCreatorReady);
            _readyState = false;
            pdfCreator.cPrinterStop = false;

            int waitingLimit = 0;
            while (!_readyState && waitingLimit < WaitingPrintingLimitInSeconds)
            {
                waitingLimit += 1;
                Thread.Sleep(1000);
            }

            pdfCreator.cPrinterStop = true;
            pdfCreator.cClose();
            pdfCreator = null;

            return !_readyState ?
                   new ResultConverting() :
                   new ErrorConverting(FileConvertErrorType.PdfPrintingError, "Время создания PDF файла истекло").ToResultConverting();
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
