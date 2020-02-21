using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;

namespace GadzhiCommon.Helpers.FileSystem
{
    /// <summary>
    /// Информация о файле, статусе сохранения, ошибках
    /// </summary>
    public class FileSavedCheck
    {
        /// <summary>
        /// Список ошибок
        /// </summary>
        private List<FileConvertErrorType> _errors;

        public FileSavedCheck()
        {
            IsSaved = false;
            FilePath = String.Empty;
            _errors = new List<FileConvertErrorType>();
        }

        /// <summary>
        /// Сохранен ли файл
        /// </summary>
        public bool IsSaved { get; set; }

        /// <summary>
        /// Путь сохранения
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Список ошибок
        /// </summary>
        public IReadOnlyList<FileConvertErrorType> Errors => _errors;

        /// <summary>
        /// Добавить ошибку в список
        /// </summary>      
        public void AddError(FileConvertErrorType error)
        {
            _errors.Add(error);
        }

        /// <summary>
        /// Добавить ошибки в список
        /// </summary>      
        public void AddErrors(IEnumerable<FileConvertErrorType> errors)
        {
            _errors.AddRange(errors);
        }
    }
}