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
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;

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
        public IResultValue<IDocumentLibrary> OpenDocument(string filePath) =>
            IsDocumentValid(filePath).
            ResultValueOkBind(extension => SetActiveLibraryByExtension(extension)).
            ResultValueOkTry(activeLibrary => new ResultValue<IDocumentLibrary>(activeLibrary.OpenDocument(filePath)),
                             new ErrorCommon(FileConvertErrorType.FileNotOpen, $"Ошибка открытия файла {filePath}"));

        /// <summary>
        /// Сохранить документ
        /// </summary>
        public IResultValue<IFileDataSourceServer> SaveDocument(string filePath) =>
            ExecuteBindFileDataErrors<IDocumentLibrary, IResultValue<IDocumentLibrary>>(()
                => new ResultValue<IDocumentLibrary>(ActiveLibrary.SaveDocument(filePath))).
            ResultValueOk(document => (IFileDataSourceServer)new FileDataSourceServer(document.FullName, FileExtention.docx));

        /// <summary>
        /// Сохранить файл PDF
        /// </summary>
        public IResultCollection<IFileDataSourceServer> CreatePdfFile(string filePath, ColorPrint colorPrint, IPrinterInformation pdfPrinterInformation) =>
             ExecuteBindFileDataErrors<IEnumerable<IFileDataSourceServer>, IResultCollection<IFileDataSourceServer>>(() =>
                CreatePdfInDocument(filePath, colorPrint, pdfPrinterInformation),
                                    new ErrorCommon(FileConvertErrorType.PdfPrintingError, $"Ошибка сохранения файла PDF {filePath}"));

        /// <summary>
        /// Закрыть файл
        /// </summary>
        public IResultError CloseDocument() =>
             ExecuteAndHandleError(() => ActiveLibrary.CloseAndSaveDocument(),
                    errorMessage: new ErrorCommon(FileConvertErrorType.FileNotSaved,$"Ошибка закрытия файла {ActiveLibrary.ActiveDocument.FullName}"));           

        /// <summary>
        /// Проверить корректность файла. Записать ошибки
        /// </summary>    
        private IResultValue<FileExtention> IsDocumentValid(string filePathDocument) =>
            filePathDocument.
            WhereContinue(filePath => _fileSystemOperations.IsFileExist(filePath),
                okFunc: filePath => new ResultValue<string>(filePath),
                badFunc: filePath => new ErrorCommon(FileConvertErrorType.FileNotFound, $"Файл {filePath} не найден").
                                     ToResultValue<string>()).
            ResultMapOk(filePath => FileSystemOperations.ExtensionWithoutPointFromPath(filePath)).
            ResultValueOkBind(fileExtension => ValidateFileExtension(fileExtension));

        /// <summary>
        /// Проверить допустимость использования расширения файла
        /// </summary>           
        private IResultValue<FileExtention> ValidateFileExtension(string fileExtension) =>
            ValidFileExtentions.DocAndDgnFileTypes.Select(pair => pair.Key).
            FirstOrDefault(extensionString => extensionString.ContainsIgnoreCase(fileExtension)).
            WhereContinue(extensionKey => !String.IsNullOrWhiteSpace(extensionKey),
                   okFunc: extensionKey => new ResultValue<FileExtention>(ValidFileExtentions.DocAndDgnFileTypes[extensionKey]),
                   badFunc: _ => new ErrorCommon(FileConvertErrorType.IncorrectExtension,
                                   $"Расширение файла {fileExtension} не соответствует типам расширений doc или dgn").
                                 ToResultValue<FileExtention>());
    }
}
