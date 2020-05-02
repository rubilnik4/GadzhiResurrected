using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GadzhiCommon.Helpers.FileSystem
{
    /// <summary>
    /// Информация о файле, статусе сохранения, ошибках
    /// </summary>
    public class FileSavedCheck
    {
        public FileSavedCheck(string filePath)
        {
            if (String.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));
            FilePath = filePath;
        }

        public FileSavedCheck(FileConvertErrorType error)
            : this(new List<FileConvertErrorType>() {error})
        { }

        public FileSavedCheck(IEnumerable<FileConvertErrorType> errors)
        {
            if (errors == null || errors.Equals(Enumerable.Empty<FileConvertErrorType>())) throw new ArgumentNullException(nameof(errors));
            Errors = errors.ToList();
        }

        /// <summary>
        /// Путь сохранения
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// Список ошибок
        /// </summary>
        public IEnumerable<FileConvertErrorType> Errors { get; }

        /// <summary>
        /// Сохранен ли файл
        /// </summary>
        public bool IsSaved => Errors.Any();
    }
}