using System;
using GadzhiApplicationCommon.Infrastructure.Interfaces;
using GadzhiApplicationCommon.Models.Implementation.Errors;
using GadzhiApplicationCommon.Models.Interfaces.Errors;

namespace GadzhiApplicationCommon.Infrastructure.Implementations
{
    /// <summary>
    /// Управление печатью пдф
    /// </summary>
    public class PdfCreatorService : IPdfCreatorService
    {
        /// <summary>
        /// Маркер готовности
        /// </summary>
        private bool _readyState;

        /// <summary>
        /// Время ожидания на создание PDF файла
        /// </summary>
        private const int WAITING_PRINTING_LIMIT_IN_SECONDS = 60;

        /// <summary>
        /// Инициализировать модуль PDFCreator
        /// </summary>
        //private static PDFCreator.clsPDFCreator PdfCreatorInitialize()
        //{
        //    KillAllPreviousProcess();
        //    var pdfCreator = new PDFCreator.clsPDFCreator();
        //    return pdfCreator;
        //}

        /// <summary>
        /// Создать PDF файл с выполнением отложенной печати 
        /// </summary>       
        public IResultApplication PrintPdfWithExecuteAction(string filePath, Func<IResultApplication> printFunction) => new ResultApplication();
            //SetPrinterOptions(filePath).
            //ResultValueOkBind(pdfCreator => new ResultValue<PDFCreator.clsPDFCreator>(pdfCreator)).
            //ResultValueOkBind(pdfCreator => new ResultValue<Unit>(Unit.Value, PrintPdf(pdfCreator, printFunction).Errors)).
            //ToResult();

        ///// <summary>
        ///// Установить опции печати
        ///// </summary>   
        //private static IResultValue<PDFCreator.clsPDFCreator> SetPrinterOptions(string filePath) =>
        //    new ResultValue<PDFCreator.clsPDFCreator>(PdfCreatorInitialize()).
        //    ResultValueContinue(pdfCreator => pdfCreator.cStart("/NoProcessingAtStartup"),
        //        okFunc: pdfCreator => pdfCreator,
        //        badFunc: _ => new ErrorCommon(ErrorConvertingType.PdfPrintingError, "Ошибка инициализации PDF принтера")).
        //    ResultValueContinue(pdfCreator => PdfPathValid(filePath),
        //        okFunc: pdfCreator =>
        //        {
        //            pdfCreator.cOptions = GetPdfPrinterOptions(pdfCreator, filePath);
        //            pdfCreator.cClearCache();
        //            return pdfCreator;
        //        },
        //        badFunc: _ => new ErrorCommon(ErrorConvertingType.PdfPrintingError, $"Путь сохранения задан неверно {filePath}"));

        ///// <summary>
        ///// Корректность пути сохранения Pdf
        ///// </summary>        
        //private static bool PdfPathValid(string filePath) =>
        //    FileSystemOperations.ExtensionWithoutPointFromPath(filePath).
        //    Map(fileExtension => !String.IsNullOrEmpty(filePath) && !String.IsNullOrEmpty(fileExtension) &&
        //                          FileExtension.Pdf.ToString().ContainsIgnoreCase(fileExtension));

        ///// <summary>
        ///// Напечатать PDF
        ///// </summary>
        //private IResultError PrintPdf(PDFCreator.clsPDFCreator pdfCreator, Func<IResultError> printFunction)
        //{
        //    pdfCreator.eReady += PdfCreatorReady;
        //    _readyState = false;
        //    pdfCreator.cPrinterStop = false;

        //    var printFunctionResult = printFunction.Invoke();
        //    int waitingLimit = 0;
        //    while (!_readyState && waitingLimit < WAITING_PRINTING_LIMIT_IN_SECONDS && printFunctionResult.OkStatus)
        //    {
        //        waitingLimit += 1;
        //        Thread.Sleep(1000);
        //    }

        //    pdfCreator.cPrinterStop = true;
        //    pdfCreator.cClose();

        //    return _readyState ?
        //           new ResultError() :
        //           new ErrorCommon(ErrorConvertingType.PdfPrintingError, "Время создания PDF файла истекло").
        //           ToResult().
        //           ConcatErrors(printFunctionResult.Errors);
        //}

        ///// <summary>
        ///// Установить опции печати PDF
        ///// </summary>       
        //private static PDFCreator.clsPDFCreatorOptions GetPdfPrinterOptions(PDFCreator.clsPDFCreator pdfCreator, string filePath)
        //{
        //    var pdfCreatorOptions = pdfCreator.cOptions;
        //    pdfCreatorOptions.UseAutosave = 1;
        //    pdfCreatorOptions.UseAutosaveDirectory = 1;
        //    pdfCreatorOptions.AutosaveDirectory = Path.GetDirectoryName(filePath);
        //    pdfCreatorOptions.AutosaveFilename = Path.GetFileNameWithoutExtension(filePath);
        //    pdfCreatorOptions.AutosaveFormat = 0; // сделать PDF 
        //    return pdfCreatorOptions;
        //}
        ///// <summary>
        ///// Событие готовности печати PDF
        ///// </summary>
        //private void PdfCreatorReady() => _readyState = true;

        ///// <summary>
        ///// Уничтожить все предыдущие процессы
        ///// </summary>
        //private static void KillAllPreviousProcess()
        //{
        //    foreach (var process in Process.GetProcesses())
        //    {
        //        if (process.ProcessName.ContainsIgnoreCase("pdfcreator"))
        //        {
        //            process.Kill();
        //        }
        //    }
        //}
    }
}
