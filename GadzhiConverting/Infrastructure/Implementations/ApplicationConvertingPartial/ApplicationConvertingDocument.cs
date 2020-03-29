using ConvertingModels.Models.Interfaces.FilesConvert;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extentions.Functional;
using GadzhiCommon.Extentions.StringAdditional;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Models.Implementations.Errors;
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
        public ErrorConverting OpenDocument(string filePath)
        {
            (FileExtention? documentExtension, ErrorConverting openError) = IsDocumentValid(filePath);
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
        public (IEnumerable<IFileDataSourceServer>, IEnumerable<ErrorConverting>) SaveDocument(string filePath)
        {
            IEnumerable<IFileDataSourceServer> filesDataSourceServer = null;
            IEnumerable<ErrorConverting> savingErrors = null;

            if (ActiveLibrary.IsDocumentValid)
            {
                _executeAndCatchErrors.ExecuteAndHandleError(() =>
                {
                    ActiveLibrary.SaveDocument(filePath);
                    filesDataSourceServer = new FileDataSourceServer(filePath, FileExtention.docx).
                                            Map(dataSource => new List<IFileDataSourceServer>() { dataSource });
                },
                applicationCatchMethod: () =>
                {
                    savingErrors = new ErrorConverting(FileConvertErrorType.FileNotSaved,
                                                       $"Ошибка сохранения основного файла {filePath}").
                                   Map(error => new List<ErrorConverting>() { error });

                    ActiveLibrary.CloseDocument();
                });
            }

            return (filesDataSourceServer, savingErrors);
        }

        /// <summary>
        /// Сохранить файл PDF
        /// </summary>
        public (IEnumerable<IFileDataSourceServer>, IEnumerable<ErrorConverting>) CreatePdfFile(string filePath, ColorPrint colorPrint,
                                                                                                IPrinterInformation pdfPrinterInformation)
        {
            IEnumerable<IFileDataSourceServer> fileDatasSourceServer = null;
            IEnumerable<ErrorConverting> savingErrors = null;

            if (ActiveLibrary.IsDocumentValid)
            {
                _executeAndCatchErrors.ExecuteAndHandleError(() =>
                {
                    (fileDatasSourceServer, savingErrors) = CreatePdfInDocument(filePath, colorPrint, pdfPrinterInformation);
                },
                applicationCatchMethod: () => savingErrors = new List<ErrorConverting>() {new ErrorConverting(FileConvertErrorType.PdfPrintingError,
                                                                                          $"Ошибка сохранения файла PDF {filePath}")});
            }
            return (fileDatasSourceServer, savingErrors);
        }

        /// <summary>
        /// Закрыть файл
        /// </summary>
        public ErrorConverting CloseDocument()
        {
            ErrorConverting closingError = null;

            if (ActiveLibrary.IsDocumentValid)
            {
                _executeAndCatchErrors.ExecuteAndHandleError(
                    () => ActiveLibrary.CloseAndSaveDocument(),
                    applicationCatchMethod: () =>
                    {
                        closingError = new ErrorConverting(FileConvertErrorType.FileNotSaved,
                                                   $"Ошибка закрытия файла {ActiveLibrary.ActiveDocument.FullName}");
                        ActiveLibrary.CloseDocument();
                    });
            }

            return closingError;
        }

        /// <summary>
        /// Проверить корректность файла. Записать ошибки
        /// </summary>    
        private (FileExtention?, ErrorConverting) IsDocumentValid(string filePath)
        {
            FileExtention? fileExtention = null;
            ErrorConverting documentError = null;

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
