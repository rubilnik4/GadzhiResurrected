using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GadzhiWcfHost.Helpers.Converters
{
    /// <summary>
    /// Информация о файле, статусе сохранения, ошибках
    /// </summary>
    public class FileSavedCheck
    {
        public FileSavedCheck()
        {
            IsSaved = false;
            FilePath = String.Empty;
            Errors = new List<FileConvertErrorType>();
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
        public List<FileConvertErrorType> Errors { get; set; }
    }
}