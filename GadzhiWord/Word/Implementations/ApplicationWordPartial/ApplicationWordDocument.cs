﻿using ConvertingModels.Models.Implementations.FilesConvert;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extentions.StringAdditional;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiWord.Word.Interfaces;
using GadzhiWord.Word.Interfaces.ApplicationWordPartial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiWord.Word.Implementations.ApplicationWordPartial
{
    /// <summary>
    /// Подкласс приложения Word для работы с документом
    /// </summary>
    public partial class ApplicationWord : IApplicationWordDocument
    {
        /// <summary>
        /// Открыть документ
        /// </summary>
        public IDocumentWord OpenDocument(string filePath)
        {
            if (IsDesingFileValidAndSetErrors(filePath))
            {
                _executeAndCatchErrors.ExecuteAndHandleError(
                    () => Application.Documents.Open(filePath),
                    applicationCatchMethod: () => new ErrorConverting(FileConvertErrorType.FileNotOpen,
                                                                      $"Ошибка открытия файла {filePath}"));
            };
            return ActiveDocument;
        }

        /// <summary>
        /// Сохранить документ
        /// </summary>
        public void SaveDocument(string filePath)
        {
            if (ActiveDocument.IsDocumentValid)
            {
                _executeAndCatchErrors.ExecuteAndHandleError(() =>
                {
                    ActiveDocument.SaveAs(filePath);
                    _wordProject.FileDataServer.
                        AddConvertedFilePath(new FileDataSourceServerBase(filePath, FileExtention.docx));
                },
                applicationCatchMethod: () =>
                {
                    ActiveDocument.Close();
                    return new ErrorConverting(FileConvertErrorType.FileNotSaved,
                                               $"Ошибка сохранения файла DGN {filePath}");
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
