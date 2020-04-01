using ConvertingModels.Models.Interfaces.FilesConvert;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extentions.Functional;
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
        public (IEnumerable<IFileDataSourceServer>, IEnumerable<IErrorConverting>) SaveDocument(string filePath)
        {
            IEnumerable<IFileDataSourceServer> filesDataSourceServer = null;
            IEnumerable<IErrorConverting> savingErrors = null;

            if (ActiveLibrary.IsDocumentValid)
            {
                var executeError = _executeAndCatchErrors.ExecuteAndHandleError(() =>
                {
                    ActiveLibrary.SaveDocument(filePath);
                    filesDataSourceServer = new FileDataSourceServer(filePath, FileExtention.docx).
                                            Map(dataSource => new List<IFileDataSourceServer>() { dataSource });
                },
                applicationCatchMethod: () => ActiveLibrary.CloseDocument());
                savingErrors = executeError?.
                               Map(error => new ErrorConverting(FileConvertErrorType.FileNotSaved,
                                                                $"Ошибка сохранения основного файла {filePath}",
                                                                error.ExceptionMessage, error.StackTrace));
            }

            return (filesDataSourceServer, savingErrors);
        }

        /// <summary>
        /// Сохранить файл PDF
        /// </summary>
        public (IEnumerable<IFileDataSourceServer>, IEnumerable<IErrorConverting>) CreatePdfFile(string filePath, ColorPrint colorPrint,
                                                                                                IPrinterInformation pdfPrinterInformation)
        {
            IEnumerable<IFileDataSourceServer> fileDatasSourceServer = null;
            IEnumerable<IErrorConverting> pdfErrors = null;

            if (ActiveLibrary.IsDocumentValid)
            {
                var executeError = _executeAndCatchErrors.ExecuteAndHandleError(() =>
                  {
                      (fileDatasSourceServer, pdfErrors) = CreatePdfInDocument(filePath, colorPrint, pdfPrinterInformation);
                  });
                pdfErrors = executeError?.
                            Map(error => new ErrorConverting(FileConvertErrorType.PdfPrintingError,
                                                             $"Ошибка сохранения файла PDF {filePath}",
                                                             error.ExceptionMessage, error.StackTrace));
            }
            return (fileDatasSourceServer, pdfErrors);
        }

        /// <summary>
        /// Закрыть файл
        /// </summary>
        public IErrorConverting CloseDocument()
        {
            IErrorConverting closingError = null;

            if (ActiveLibrary.IsDocumentValid)
            {
                var executeError = _executeAndCatchErrors.ExecuteAndHandleError(
                    () => ActiveLibrary.CloseAndSaveDocument(),
                    applicationCatchMethod: () => ActiveLibrary.CloseDocument());

                closingError = executeError?.
                               Map(error => new ErrorConverting(FileConvertErrorType.FileNotSaved,
                                                                $"Ошибка закрытия файла {ActiveLibrary.ActiveDocument.FullName}",
                                                                error.ExceptionMessage, error.StackTrace));
            }

            return closingError;
        }

        /// <summary>
        /// Проверить корректность файла. Записать ошибки
        /// </summary>    
        private (FileExtention?, IErrorConverting) IsDocumentValid(string filePath)
        {
            FileExtention? fileExtention = null;
            IErrorConverting documentError = null;

            if (_fileSystemOperations.IsFileExist(filePath))
            {
                string fileExtentionString = FileSystemOperations.ExtensionWithoutPointFromPath(filePath);
                fileExtention = ValidFileExtentions.DocAndDgnFileTypes.
                                FirstOrDefault(pair => pair.Key.ContainsIgnoreCase(fileExtentionString)).
                                Value;
                if (fileExtention == null)
                {
                    documentError = new ErrorConverting(FileConvertErrorType.IncorrectExtension,
                                        $"Расширение файла {filePath} не соответствует типам расширений doc или dgn");
                }
            }
            else
            {
                documentError = new ErrorConverting(FileConvertErrorType.FileNotFound, $"Файл {filePath} не найден");
            }

            return (fileExtention, documentError);
        }
    }
}
