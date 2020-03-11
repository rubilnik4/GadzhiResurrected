using ConvertingModels.Models.Interfaces.ApplicationLibrary.Document;
using ConvertingModels.Models.Interfaces.FilesConvert;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extentions.StringAdditional;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiConverting.Infrastructure.Interfaces.Application.ApplicationPartial;
using GadzhiConverting.Models.Implementations.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Implementations.Application.ApplicationPartial
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
            (bool isDocumentValid, ErrorConverting openError) = IsDocumentValid(filePath);
            if (isDocumentValid)
            {
                _executeAndCatchErrors.ExecuteAndHandleError(
                    () => _applicationLibrary.OpenDocument(filePath),
                    applicationCatchMethod: () => openError = new ErrorConverting(FileConvertErrorType.FileNotOpen,
                                                                                  $"Ошибка открытия файла {filePath}"));
            };
            return openError;
        }

        /// <summary>
        /// Сохранить документ
        /// </summary>
        public (IFileDataSourceServer, ErrorConverting) SaveDocument(string filePath)
        {
            IFileDataSourceServer fileDataSourceServer = null;
            ErrorConverting savingError = null;

            if (_applicationLibrary.IsDocumentValid)
            {
                _executeAndCatchErrors.ExecuteAndHandleError(() =>
                {
                    _applicationLibrary.SaveDocument(filePath);
                    fileDataSourceServer = new FileDataSourceServer(filePath, FileExtention.docx);
                },
                applicationCatchMethod: () =>
                {
                    savingError = new ErrorConverting(FileConvertErrorType.FileNotSaved,
                                                      $"Ошибка сохранения основного файла {filePath}");
                    _applicationLibrary.CloseDocument();
                });
            }

            return (fileDataSourceServer, savingError);
        }

        /// <summary>
        /// Сохранить файл PDF
        /// </summary>
        public (IEnumerable<IFileDataSourceServer>, IEnumerable<ErrorConverting>) CreatePdfFile(string filePath, ColorPrint colorPrint, string pdfPrinterName)
        {
            IEnumerable<IFileDataSourceServer> fileDatasSourceServer = null;
            IEnumerable<ErrorConverting> savingErrors = null;

            if (_applicationLibrary.IsDocumentValid)
            {
                _executeAndCatchErrors.ExecuteAndHandleError(() =>
                {
                    (fileDatasSourceServer, savingErrors) = CreatePdfInDocument(filePath, colorPrint, pdfPrinterName);
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

            if (_applicationLibrary.IsDocumentValid)
            {
                _executeAndCatchErrors.ExecuteAndHandleError(
                    () => _applicationLibrary.CloseAndSaveDocument(),
                    applicationCatchMethod: () =>
                    {
                        closingError = new ErrorConverting(FileConvertErrorType.FileNotSaved,
                                                   $"Ошибка закрытия файла {_applicationLibrary.ActiveDocument.FullName}");
                        _applicationLibrary.CloseDocument();
                    });
            }

            return closingError;
        }

        /// <summary>
        /// Проверить корректность файла. Записать ошибки
        /// </summary>    
        private (bool, ErrorConverting) IsDocumentValid(string filePath)
        {
            bool isValid = false;
            ErrorConverting documentError = null;

            if (_fileSystemOperations.IsFileExist(filePath))
            {
                string fileExtension = FileSystemOperations.ExtensionWithoutPointFromPath(filePath);
                if (FileExtention.docx.ToString().ContainsIgnoreCase(fileExtension))
                {
                    isValid = true;
                }
                else
                {
                    documentError = new ErrorConverting(FileConvertErrorType.IncorrectExtension,
                                        $"Расширение файла {filePath} не соответствует типу .doc или .docx");
                }
            }
            else
            {
                documentError = new ErrorConverting(FileConvertErrorType.FileNotFound, $"Файл {filePath} не найден");
            }

            return (isValid, documentError);
        }
    }
}
