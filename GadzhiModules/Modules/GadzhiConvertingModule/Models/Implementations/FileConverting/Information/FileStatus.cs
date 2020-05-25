using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting.Information
{
    /// <summary>
    /// Класс содержащий статус и ошибки при конвертировании
    /// </summary>
    public class FileStatus
    {
        public FileStatus(string filePath, StatusProcessing statusProcessing, FileConvertErrorType error)
            :this(filePath, statusProcessing, new List<FileConvertErrorType>() { error })
        { }

        public FileStatus(string filePath, StatusProcessing statusProcessing, IEnumerable<FileConvertErrorType> errors)
        {
            FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            StatusProcessing = statusProcessing;
            Errors = errors?.ToList().AsReadOnly() ?? throw new ArgumentNullException(nameof(errors)); ;
        }

        /// <summary>
        /// Путь файла
        /// </summary>
        public string FilePath { get;  }

        /// <summary>
        /// Статус обработки файла
        /// </summary>
        public StatusProcessing StatusProcessing { get;  }

        /// <summary>
        /// Список ошибок
        /// </summary>
        public IReadOnlyCollection<FileConvertErrorType> Errors { get; }
    }
}
