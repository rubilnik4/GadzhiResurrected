using ConvertingModels.Models.Interfaces.FilesConvert;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extentions.Functional;
using GadzhiCommon.Extentions.Functional.Result;
using GadzhiCommon.Extentions.StringAdditional;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Infrastructure.Interfaces.ApplicationConvertingPartial;
using GadzhiConverting.Models.Implementations.FilesConvert;
using GadzhiConverting.Models.Interfaces.Printers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiApplicationCommon.Models.Interfaces.ApplicationLibrary.Document;
using GadzhiConverting.Extensions;
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
        public IResultValue<IDocumentLibrary> OpenDocument(string filePath) =>
            IsDocumentValid(filePath).
            ResultValueOkBind(extension => GetActiveLibraryByExtension(extension)).
            ResultValueOkTry(activeLibrary => activeLibrary.OpenDocument(filePath).ToResultValueFromApplication(),
                             new ErrorCommon(FileConvertErrorType.FileNotOpen, $"Ошибка открытия файла {filePath}"));

        /// <summary>
        /// Сохранить документ
        /// </summary>
        public IResultValue<IFileDataSourceServer> SaveDocument(IDocumentLibrary documentLibrary, string filePath) =>
            ExecuteAndHandleError(() => documentLibrary.SaveAs(filePath),
                                  errorMessage: new ErrorCommon(FileConvertErrorType.PdfPrintingError, $"Ошибка сохранения файла{filePath}")).
            ResultValueOk(_ => new FileDataSourceServer(filePath));

        /// <summary>
        /// Сохранить файл PDF
        /// </summary>
        public IResultCollection<IFileDataSourceServer> CreatePdfFile(IDocumentLibrary documentLibrary, string filePath, 
                                                                      ColorPrint colorPrint, IPrinterInformation pdfPrinterInformation) =>
             ExecuteBindResultValue<IEnumerable<IFileDataSourceServer>, IResultCollection<IFileDataSourceServer>>(() =>
                CreatePdfInDocument(documentLibrary, filePath, colorPrint, pdfPrinterInformation),
                                    new ErrorCommon(FileConvertErrorType.PdfPrintingError, $"Ошибка сохранения файла PDF {filePath}"));

        /// <summary>
        /// Экпортировать файл
        /// </summary>
        public IResultValue<IFileDataSourceServer> CreateExportFile(IDocumentLibrary documentLibrary, string filePath) =>
           ExecuteAndHandleError(() => documentLibrary.Export(filePath),
                         errorMessage: new ErrorCommon(FileConvertErrorType.PdfPrintingError, $"Ошибка экспорта файла {filePath}")).
            ResultValueOk(fileExportPath => (IFileDataSourceServer)new FileDataSourceServer(fileExportPath));
     
        /// <summary>
        /// Закрыть файл
        /// </summary>
        public IResultError CloseDocument(IDocumentLibrary documentLibrary) =>
            ExecuteAndHandleError(() => documentLibrary.CloseWithSaving(),
                          errorMessage: new ErrorCommon(FileConvertErrorType.FileNotSaved, $"Ошибка закрытия файла {documentLibrary?.FullName}")).
            ToResult();

        /// <summary>
        /// Проверить корректность файла. Записать ошибки
        /// </summary>    
        private IResultValue<FileExtention> IsDocumentValid(string filePathDocument) =>
            filePathDocument.
            WhereContinue(filePath => _fileSystemOperations.IsFileExist(filePath),
                okFunc: filePath => new ResultValue<string>(filePath),
                badFunc: filePath => new ErrorCommon(FileConvertErrorType.FileNotFound, $"Файл {filePath} не найден").
                                     ToResultValue<string>()).
            ResultValueOk(filePath => FileSystemOperations.ExtensionWithoutPointFromPath(filePath)).
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
