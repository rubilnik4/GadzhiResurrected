using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Models.Interfaces.Errors;

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
            Errors = new List<IErrorCommon>().AsReadOnly();
        }

        public FileSavedCheck(IErrorCommon error)
            : this(new List<IErrorCommon>() { error })
        { }

        public FileSavedCheck(IEnumerable<IErrorCommon> errors)
        {
            if (errors == null || errors.Equals(Enumerable.Empty<IErrorCommon>())) throw new ArgumentNullException(nameof(errors));
            Errors = errors.ToList().AsReadOnly();
        }

        /// <summary>
        /// Путь сохранения
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// Список ошибок
        /// </summary>
        public IReadOnlyCollection<IErrorCommon> Errors { get; }

        /// <summary>
        /// Сохранен ли файл
        /// </summary>
        public bool IsSaved => Errors.Any();
    }
}