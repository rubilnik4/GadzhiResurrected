using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;

namespace GadzhiResurrected.Infrastructure.Implementations.Validates
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
        public static IResultError ValidateFilesData(IReadOnlyCollection<string> filePaths, IFilePathOperations filePathOperations) =>
            filePaths.
            Select(filePath => ValidateByPath(filePath, filePathOperations)).
            Aggregate((IResultError)new ResultError(), (first, second) => first.ConcatErrors(second.Errors)).
            ConcatErrors(ValidateByCount(filePaths).Errors).
            ConcatErrors(ValidateBySize(filePaths, filePathOperations).Errors);

        /// <summary>
        /// Проверить файлы на наличие
        /// </summary>
        public static IResultError ValidateByPath(string filePath, IFilePathOperations filePathOperations) =>
            filePathOperations.IsFileExist(filePath)
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
        public static IResultError ValidateBySize(IReadOnlyCollection<string> filePaths, IFilePathOperations filePathOperations) =>
            filePaths.
            Select(filePathOperations.GetFileSize).
            DefaultIfEmpty(0).
            Sum() <= FILES_SIZE_MAX * 1_000_000
                ? new ResultError()
                : new ResultError(new ErrorCommon(ErrorConvertingType.ArgumentOutOfRange,
                                                  $"Превышен допустимый размер файлов для конвертации. Максимально {FILES_SIZE_MAX} Мб"));
    }
}