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
using static GadzhiCommon.Extensions.Functional.Result.ExecuteResultHandler;
using static GadzhiCommon.Infrastructure.Implementations.ExecuteAndCatchErrors;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiCommon.Infrastructure.Implementations.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations.Reflection;
using GadzhiCommon.Models.Enums;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiConvertingLibrary.Extensions;

namespace GadzhiConverting.Infrastructure.Implementations.ApplicationConvertingPartial
{
    /// <summary>
    /// Подкласс для работы с документом
    /// </summary>
    public partial class ApplicationConverting
    {
        /// <summary>
        /// Открыть документ
        /// </summary>
        [Logger]
        public IResultValue<IDocumentLibrary> OpenDocument(string filePath) =>
            IsDocumentValid(filePath).
            ResultValueOkBind(GetActiveLibraryByExtension).
            ResultValueOkTry(activeLibrary => activeLibrary.OpenDocument(filePath).ToResultValueFromApplication(),
                             new ErrorCommon(ErrorConvertingType.FileNotOpen, $"Ошибка открытия файла {filePath}")).
            ResultVoidOk(_ => _loggerService.LogByObject(LoggerLevel.Debug, LoggerAction.Operation, ReflectionInfo.GetMethodBase(this), filePath));

        /// <summary>
        /// Сохранить документ
        /// </summary>
        [Logger]
        public IResultValue<IFileDataSourceServer> SaveDocument(IDocumentLibrary documentLibrary, IFilePath filePath) =>
            ExecuteAndHandleError(() => documentLibrary.SaveAs(filePath.FilePathServer),
                                  errorMessage: new ErrorCommon(ErrorConvertingType.PdfPrintingError, $"Ошибка сохранения файла {filePath.FileNameClient}")).
            ResultValueOk(_ => new FileDataSourceServer(filePath.FilePathServer, filePath.FilePathClient));

        /// <summary>
        /// Сохранить файл PDF
        /// </summary>
        [Logger]
        public IResultCollection<IFileDataSourceServer> CreateProcessingFile(IDocumentLibrary documentLibrary, IFilePath filePathMain, IFilePath filePathPdf,
                                                                      IConvertingSettings convertingSettings, ColorPrintType colorPrintType) =>
            ExecuteBindResultValue(() => CreateProcessingDocument(documentLibrary, filePathMain, filePathPdf, convertingSettings, colorPrintType),
                                         new ErrorCommon(ErrorConvertingType.PdfPrintingError, $"Ошибка обработки файла {filePathMain.FileNameClient}")).
            ToResultCollection();

        /// <summary>
        /// Экспортировать файл
        /// </summary>
        [Logger]
        public IResultValue<IFileDataSourceServer> CreateExportFile(IDocumentLibrary documentLibrary, IFilePath filePath,
                                                                    StampDocumentType stampDocumentType) =>
           ExecuteAndHandleError(() => documentLibrary.Export(filePath.FilePathServer, stampDocumentType),
                         errorMessage: new ErrorCommon(ErrorConvertingType.ExportError, $"Ошибка экспорта файла {filePath.FileNameClient}")).
            ResultValueOk(fileExportPath => (IFileDataSourceServer)new FileDataSourceServer(filePath.FilePathServer, filePath.FilePathClient));

        /// <summary>
        /// Закрыть файл
        /// </summary>
        [Logger]
        public IResultError CloseDocument(IDocumentLibrary documentLibrary, string filePath) =>
            ExecuteAndHandleError(documentLibrary.Close,
                          catchMethod: documentLibrary.CloseApplication,
                          errorMessage: new ErrorCommon(ErrorConvertingType.FileNotSaved, $"Ошибка закрытия файла {filePath}")).
            ToResult();

        /// <summary>
        /// Проверить корректность файла. Записать ошибки
        /// </summary>    
        private IResultValue<FileExtensionType> IsDocumentValid(string filePathDocument) =>
            filePathDocument.
            WhereContinue(filePath => _fileSystemOperations.IsFileExist(filePath),
                okFunc: filePath => new ResultValue<string>(filePath),
                badFunc: filePath => new ErrorCommon(ErrorConvertingType.FileNotFound, $"Файл {filePath} не найден").
                                     ToResultValue<string>()).
            ResultValueOk(FileSystemOperations.ExtensionWithoutPointFromPath).
            ResultValueOkBind(ValidateFileExtension);

        /// <summary>
        /// Проверить допустимость использования расширения файла
        /// </summary>           
        private static IResultValue<FileExtensionType> ValidateFileExtension(string fileExtension) =>
            fileExtension.
            WhereContinue(_ => ValidFileExtensions.ContainsInDocAndDgnFileTypes(fileExtension),
                   okFunc: extensionKey => new ResultValue<FileExtensionType>(ValidFileExtensions.GetDocAndDgnFileTypes(extensionKey)),
                   badFunc: _ => new ErrorCommon(ErrorConvertingType.IncorrectExtension,
                                                 $"Расширение файла {fileExtension} не соответствует типам расширений doc или Dgn").
                                 ToResultValue<FileExtensionType>());
    }
}
