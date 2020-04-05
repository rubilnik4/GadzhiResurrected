using ConvertingModels.Models.Interfaces.FilesConvert;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extentions.Functional;
using GadzhiCommon.Extentions.Functional.Result;
using GadzhiCommon.Extentions.StringAdditional;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Infrastructure.Interfaces.ApplicationConvertingPartial;
using GadzhiConverting.Models.Implementations;
using GadzhiConverting.Models.Implementations.FilesConvert;
using GadzhiConverting.Models.Interfaces;
using GadzhiConverting.Models.Interfaces.Printers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GadzhiConverting.Infrastructure.Implementations.ExecuteBindHandler;
using static GadzhiCommon.Infrastructure.Implementations.ExecuteAndCatchErrors;

namespace GadzhiConverting.Infrastructure.Implementations.ApplicationConvertingPartial
{
    /// <summary>
    /// Подкласс для работы с документом
    /// </summary>
    public partial class ApplicationConverting : IApplicationConvertingDocument
    {
        /// <summary>
        /// Открыть документ
        /// </summary>
        public IErrorConverting OpenDocument(string filePath)
        {
            (FileExtention? documentExtension, IErrorConverting openError) = IsDocumentValid(filePath);
            if (documentExtension != null)
            {
                openError = SetActiveLibraryByExtension(documentExtension.Value);
                if (openError == null)
                {
                    _executeAndCatchErrors.ExecuteAndHandleError(
                        () => ActiveLibrary.OpenDocument(filePath),
                        applicationCatchMethod: () => openError = new ErrorConverting(FileConvertErrorType.FileNotOpen,
                                                                                      $"Ошибка открытия файла {filePath}"));
                }
            };
            return openError;
        }

        /// <summary>
        /// Сохранить документ
        /// </summary>
        public IResultFileDataSource SaveDocument(string filePath) =>
        //{
        //    IEnumerable<IFileDataSourceServer> filesDataSourceServer = null;
        //    IEnumerable<IErrorConverting> savingErrors = null;

        //    if (ActiveLibrary.IsDocumentValid)
        //    {
        //        var executeError = _executeAndCatchErrors.ExecuteAndHandleError(() =>
        //        {
        //            ActiveLibrary.SaveDocument(filePath);
        //            filesDataSourceServer = new FileDataSourceServer(filePath, FileExtention.docx).
        //                                    Map(dataSource => new List<IFileDataSourceServer>() { dataSource });
        //        },
        //        applicationCatchMethod: () => ActiveLibrary.CloseDocument());
        //        savingErrors = executeError?.
        //                       Map(error => new ErrorConverting(FileConvertErrorType.FileNotSaved,
        //                                                        $"Ошибка сохранения основного файла {filePath}",
        //                                                        error.ExceptionMessage, error.StackTrace));
        //    }

        //    return (filesDataSourceServer, savingErrors);
        //}

        /// <summary>
        /// Сохранить файл PDF
        /// </summary>
        public IResultFileDataSource CreatePdfFile(string filePath, ColorPrint colorPrint, IPrinterInformation pdfPrinterInformation) =>
             ExecuteBindFileDataErrors(() => CreatePdfInDocument(filePath, colorPrint, pdfPrinterInformation),
                                       new ErrorConverting(FileConvertErrorType.PdfPrintingError, $"Ошибка сохранения файла PDF {filePath}"));

        /// <summary>
        /// Закрыть файл
        /// </summary>
        public IResultConverting CloseDocument() =>
             ExecuteAndHandleError(() => ActiveLibrary.CloseAndSaveDocument()).
             WhereBad(result => result.HasErrors,
                 badFunc: result => new ErrorConverting(FileConvertErrorType.FileNotSaved,$"Ошибка закрытия файла {ActiveLibrary.ActiveDocument.FullName}").
                                    ToResultConverting ().
                                    Map(errorResult => result.ConcatResult(errorResult))); 

        /// <summary>
        /// Проверить корректность файла. Записать ошибки
        /// </summary>    
        private IResultConvertingValue<FileExtention> IsDocumentValid(string filePathDocument) =>
            filePathDocument.
            WhereContinue(filePath => _fileSystemOperations.IsFileExist(filePath),
                okFunc: filePath => new ResultConvertingValue<string>(filePath),
                badFunc: filePath => new ErrorConverting(FileConvertErrorType.FileNotFound, $"Файл {filePath} не найден").
                                     ToResultConvertingValue<string>()).
            ResultMap(filePath => FileSystemOperations.ExtensionWithoutPointFromPath(filePath)).
            ResultValueOkBind(fileExtension => ValidateFileExtension(fileExtension));       

        /// <summary>
        /// Проверить допустимость использования расширения файла
        /// </summary>           
        private IResultConvertingValue<FileExtention> ValidateFileExtension(string fileExtension) =>
            ValidFileExtentions.DocAndDgnFileTypes.Select(pair => pair.Key).
            FirstOrDefault(extensionString => extensionString.ContainsIgnoreCase(fileExtension)).
            WhereContinue(extensionKey => !String.IsNullOrWhiteSpace(extensionKey),
                   okFunc: extensionKey => new ResultConvertingValue<FileExtention>(ValidFileExtentions.DocAndDgnFileTypes[extensionKey]),
                   badFunc: _ => new ErrorConverting(FileConvertErrorType.IncorrectExtension,
                                   $"Расширение файла {fileExtension} не соответствует типам расширений doc или dgn").
                                 ToResultConvertingValue<FileExtention>());          
    }
}
