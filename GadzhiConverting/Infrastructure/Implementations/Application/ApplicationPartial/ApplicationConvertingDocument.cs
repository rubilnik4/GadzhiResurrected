using ConvertingModels.Models.Interfaces.ApplicationLibrary.Document;
using ConvertingModels.Models.Interfaces.FilesConvert;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extentions.StringAdditional;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiConverting.Models.Implementations.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Implementations.Application.ApplicationWordPartial
{
    /// <summary>
    /// Подкласс приложения Word для работы с документом
    /// </summary>
    public partial class ApplicationConverting : IApplicationConvertingDocument
    {
        /// <summary>
        /// Открыть документ
        /// </summary>
        public IDocumentLibrary OpenDocument(string filePath)
        {
            IDocumentLibrary documentLibrary = null;
            if (IsDesingFileValidAndSetErrors(filePath))
            {
                _executeAndCatchErrors.ExecuteAndHandleError(
                    () => documentLibrary = _applicationLibrary.OpenDocument(filePath),
                    applicationCatchMethod: () => new ErrorConverting(FileConvertErrorType.FileNotOpen,
                                                                      $"Ошибка открытия файла {filePath}"));
            };
            return documentLibrary;
        }

        /// <summary>
        /// Сохранить документ
        /// </summary>
        public IFileDataSourceServer SaveDocument(string filePath)
        {
            IFileDataSourceServer fileDataSourceServer = null;

            if (_applicationLibrary.IsDocumentValid)
            {
                _executeAndCatchErrors.ExecuteAndHandleError(() =>
                {
                    _applicationLibrary.SaveDocument(filePath);
                    fileDataSourceServer = new FileDataSourceServerConverting(filePath, FileExtention.docx);
                },
                applicationCatchMethod: () =>
                {
                    _applicationLibrary.CloseDocument();
                    return new ErrorConverting(FileConvertErrorType.FileNotSaved,
                                               $"Ошибка сохранения файла DGN {filePath}");
                });
            }

            return fileDataSourceServer;
        }

        /// <summary>
        /// Сохранить файл PDF
        /// </summary>
        public IEnumerable<IFileDataSourceServer> CreatePdfFile(string filePath, ColorPrint colorPrint)
        {
            IEnumerable<IFileDataSourceServer> fileDataSourceServer = null;

            if (_applicationLibrary.IsDocumentValid)
            {
                _executeAndCatchErrors.ExecuteAndHandleError(() =>
                {
                    fileDataSourceServer = CreatePdfInDocument(filePath, colorPrint);                   
                },
                    applicationCatchMethod: () => new ErrorConverting(FileConvertErrorType.PdfPrintingError,
                                                                      $"Ошибка сохранения файла PDF {filePath}"));
            }
            return fileDataSourceServer;
        }

        /// <summary>
        /// Закрыть файл
        /// </summary>
        public void CloseDocument()
        {
            if (_applicationLibrary.IsDocumentValid)
            {
                _executeAndCatchErrors.ExecuteAndHandleError(
                    () => _applicationLibrary.CloseAndSaveDocument(),
                    applicationCatchMethod: () =>
                    {
                        _applicationLibrary.CloseDocument();
                        return new ErrorConverting(FileConvertErrorType.FileNotSaved,
                                                   $"Ошибка закрытия файла {ActiveDocument.FullName}");

                    });
            }
        }

        /// <summary>
        /// Проверить корректность файла. Записать ошибки
        /// </summary>    
        private bool IsDesingFileValidAndSetErrors(string filePath)
        {
            bool isValid = false;

            if (_fileSystemOperations.IsFileExist(filePath))
            {
                string fileExtension = FileSystemOperations.ExtensionWithoutPointFromPath(filePath);
                if (FileExtention.docx.ToString().ContainsIgnoreCase(fileExtension))
                {
                    isValid = true;
                }
                else
                {
                    _messagingService.ShowAndLogError(new ErrorConverting(FileConvertErrorType.IncorrectExtension,
                                                      $"Расширение файла {filePath} не соответствует типу .doc или .docx"));
                }
            }
            else
            {
                _messagingService.ShowAndLogError(new ErrorConverting(FileConvertErrorType.FileNotFound,
                                                                      $"Файл {filePath} не найден"));
            }

            return isValid;
        }
    }
}
