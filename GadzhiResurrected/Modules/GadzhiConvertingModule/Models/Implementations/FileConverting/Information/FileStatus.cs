using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Interfaces.Errors;

namespace GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting.Information
{
    /// <summary>
    /// Класс содержащий статус и ошибки при конвертировании
    /// </summary>
    public class FileStatus
    {
        public FileStatus(string filePath, StatusProcessing statusProcessing)
           : this(filePath, statusProcessing, new List<IErrorCommon>())
        { }

        public FileStatus(string filePath, StatusProcessing statusProcessing, IErrorCommon error)
            :this(filePath, statusProcessing, new List<IErrorCommon>() { error })
        { }

        public FileStatus(string filePath, StatusProcessing statusProcessing, IEnumerable<IErrorCommon> errors)
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
        public IReadOnlyCollection<IErrorCommon> Errors { get; }

        /// <summary>
        /// Получить статус с измененным состоянием
        /// </summary>
        public FileStatus GetWithStatusProcessing(StatusProcessing statusProcessing) =>
            new FileStatus(FilePath, statusProcessing, Errors);
    }
}
