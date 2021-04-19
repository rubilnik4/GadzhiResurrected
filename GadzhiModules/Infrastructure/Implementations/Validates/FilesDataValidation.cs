using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;

namespace GadzhiModules.Infrastructure.Implementations.Validates
{
    /// <summary>
    /// Проверка данных перед отправкой
    /// </summary>
    public static class FilesDataValidation
    {
        /// <summary>
        /// Максимальное количество файлов для конвертации
        /// </summary>
        public const int FILES_COUNT_MAX = 20;

        /// <summary>
        /// Максимальное количество файлов для конвертации
        /// </summary>
        public const int FILES_SIZE_MAX = 25;

        /// <summary>
        /// Проверить файлы перед отправкой на корректность
        /// </summary>
        public static IResultError ValidateFilesData(IReadOnlyCollection<string> filePaths, IFileSystemOperations fileSystemOperations) =>
            filePaths.
            Select(filePath => ValidateByPath(filePath, fileSystemOperations)).
            Aggregate((IResultError)new ResultError(), (first, second) => first.ConcatErrors(second.Errors)).
            ConcatErrors(ValidateByCount(filePaths).Errors).
            ConcatErrors(ValidateBySize(filePaths, fileSystemOperations).Errors);

        /// <summary>
        /// Проверить файлы на наличие
        /// </summary>
        public static IResultError ValidateByPath(string filePath, IFileSystemOperations fileSystemOperations) =>
            fileSystemOperations.IsFileExist(filePath)
                ? new ResultError()
                : new ResultError(new ErrorCommon(ErrorConvertingType.FileNotFound, $"Файл {filePath} не найден"));

        /// <summary>
        /// Проверить на количество
        /// </summary>
        public static IResultError ValidateByCount(IReadOnlyCollection<string> filePaths) =>
            filePaths.Count switch
            {
                0 => new ResultError(new ErrorCommon(ErrorConvertingType.FileNotFound, "Отсутствуют файлы для конвертации")),
                _ when filePaths.Count > FILES_COUNT_MAX => new ResultError(new ErrorCommon(ErrorConvertingType.ArgumentOutOfRange,
                                                                            $"Превышено число допустимых файлов для конвертации. Максимально {FILES_COUNT_MAX}")),
                _ => new ResultError(),
            };

            /// <summary>
            /// Проверить на объем данных
            /// </summary>
        public static IResultError ValidateBySize(IReadOnlyCollection<string> filePaths, IFileSystemOperations fileSystemOperations) =>
            filePaths.
            Select(fileSystemOperations.GetFileSize).
            DefaultIfEmpty(0).
            Sum() <= FILES_SIZE_MAX * 1_000_000
                ? new ResultError()
                : new ResultError(new ErrorCommon(ErrorConvertingType.ArgumentOutOfRange,
                                                  $"Превышен допустимый размер файлов для конвертации. Максимально {FILES_SIZE_MAX} Мб"));
    }
}