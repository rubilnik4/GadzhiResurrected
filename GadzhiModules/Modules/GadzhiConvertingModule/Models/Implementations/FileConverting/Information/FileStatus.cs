using System.Collections.Generic;
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
            FilePath = filePath;
            StatusProcessing = statusProcessing;
            Errors = errors;
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
        public IEnumerable<FileConvertErrorType> Errors { get; }
    }
}
