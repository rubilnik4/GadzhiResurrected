using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Modules.FilesConvertModule.Models.Implementations.Information
{
    /// <summary>
    /// Класс содержащий статус и ошибки при конвертировании
    /// </summary>
    public class FileStatus
    {
        public FileStatus(string filePath,
                          StatusProcessing statusProcessing,
                          FileConvertErrorType error)           
        {
            var errors = new List<FileConvertErrorType>();
            if (error != FileConvertErrorType.NoError)
            {
                errors.Add(error);
            }

            Initialize(filePath, statusProcessing, errors);
        }

        public FileStatus(string filePath,
                          StatusProcessing statusProcessing,
                          IEnumerable<FileConvertErrorType> errors)
        {
            Initialize(filePath, statusProcessing, errors);
        }

        private void Initialize(string filePath,
                                StatusProcessing statusProcessing,
                                IEnumerable<FileConvertErrorType> errors)
        {
            FilePath = filePath;
            StatusProcessing = statusProcessing;
            Errors = errors;
        }

        /// <summary>
        /// Путь файла
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// Статус обработки файла
        /// </summary>
        public StatusProcessing StatusProcessing { get; private set; }

        /// <summary>
        /// Список ошибок
        /// </summary>
        public IEnumerable<FileConvertErrorType> Errors { get; private set; }
    }
}
