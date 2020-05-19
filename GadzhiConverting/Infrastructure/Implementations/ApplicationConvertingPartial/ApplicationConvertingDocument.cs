using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions.Functional;
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
using GadzhiCommon.Extensions.Functional.Result;
using GadzhiConverting.Extensions;
using GadzhiConverting.Models.Interfaces.FilesConvert;
using static GadzhiCommon.Extensions.Functional.ExecuteBindHandler;
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
            ResultValueOkBind(GetActiveLibraryByExtension).
            ResultValueOkTry(activeLibrary => activeLibrary.OpenDocument(filePath).ToResultValueFromApplication(),
                             new ErrorCommon(FileConvertErrorType.FileNotOpen, $"Ошибка открытия файла {filePath}"));

        /// <summary>
        /// Сохранить документ
        /// </summary>
        public IResultValue<IFileDataSourceServer> SaveDocument(IDocumentLibrary documentLibrary, IFilePath filePath) =>
            ExecuteAndHandleError(() => documentLibrary.SaveAs(filePath.FilePathServer),
                                  errorMessage: new ErrorCommon(FileConvertErrorType.PdfPrintingError, $"Ошибка сохранения файла {filePath.FileNameClient}")).
            ResultValueOk(_ => new FileDataSourceServer(filePath.FilePathServer, filePath.FilePathClient));

        /// <summary>
        /// Сохранить файл PDF
        /// </summary>
        public IResultCollection<IFileDataSourceServer> CreatePdfFile(IDocumentLibrary documentLibrary, IFilePath filePath,
                                                                      ColorPrint colorPrint, IPrinterInformation pdfPrinterInformation) =>
            ExecuteBindResultValue(() => CreatePdfInDocument(documentLibrary, filePath, colorPrint, pdfPrinterInformation),
                                         new ErrorCommon(FileConvertErrorType.PdfPrintingError, $"Ошибка сохранения файла PDF {filePath.FileNameClient}")).
            ToResultCollection();

        /// <summary>
        /// Экспортировать файл
        /// </summary>
        public IResultValue<IFileDataSourceServer> CreateExportFile(IDocumentLibrary documentLibrary, IFilePath filePath) =>
           ExecuteAndHandleError(() => documentLibrary.Export(filePath.FilePathServer),
                         errorMessage: new ErrorCommon(FileConvertErrorType.ExportError, $"Ошибка экспорта файла {filePath.FileNameClient}")).
            ResultValueOk(fileExportPath => (IFileDataSourceServer)new FileDataSourceServer(filePath.FilePathServer, filePath.FilePathClient));

        /// <summary>
        /// Закрыть файл
        /// </summary>
        public IResultError CloseDocument(IDocumentLibrary documentLibrary, string filePath) =>
            ExecuteAndHandleError(documentLibrary.Close,
                          catchMethod: documentLibrary.CloseApplication,
                          errorMessage: new ErrorCommon(FileConvertErrorType.FileNotSaved, $"Ошибка закрытия файла {filePath}")).
            ToResult();

        /// <summary>
        /// Проверить корректность файла. Записать ошибки
        /// </summary>    
        private IResultValue<FileExtension> IsDocumentValid(string filePathDocument) =>
            filePathDocument.
            WhereContinue(filePath => _fileSystemOperations.IsFileExist(filePath),
                okFunc: filePath => new ResultValue<string>(filePath),
                badFunc: filePath => new ErrorCommon(FileConvertErrorType.FileNotFound, $"Файл {filePath} не найден").
                                     ToResultValue<string>()).
            ResultValueOk(FileSystemOperations.ExtensionWithoutPointFromPath).
            ResultValueOkBind(ValidateFileExtension);

        /// <summary>
        /// Проверить допустимость использования расширения файла
        /// </summary>           
        private static IResultValue<FileExtension> ValidateFileExtension(string fileExtension) =>
            fileExtension.
            WhereContinue(_ => ValidFileExtensions.ContainsInDocAndDgnFileTypes(fileExtension),
                   okFunc: extensionKey => new ResultValue<FileExtension>(ValidFileExtensions.GetDocAndDgnFileTypes(extensionKey)),
                   badFunc: _ => new ErrorCommon(FileConvertErrorType.IncorrectExtension,
                                                 $"Расширение файла {fileExtension} не соответствует типам расширений doc или Dgn").
                                 ToResultValue<FileExtension>());
    }
}
