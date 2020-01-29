using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Modules.FilesConvertModule.Models.Implementations
{
    /// <summary>
    /// Класс содержащий статус и ошибки при конвертировании
    /// </summary>
    public class FileStatus
    {
        public FileStatus(string filePath, StatusProcessing statusProcessing)
            :this (filePath, statusProcessing, new List<FileConvertErrorType> ())
        {
           
        }

        public FileStatus(string filePath, StatusProcessing statusProcessing, IEnumerable<FileConvertErrorType> fileConvertErrorType)
        {
            FilePath = filePath;
            StatusProcessing = statusProcessing;
            FileConvertErrorType = fileConvertErrorType;
        }

        /// <summary>
        /// Путь файла
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// Статус обработки файла
        /// </summary>
        public StatusProcessing StatusProcessing { get; }

        /// <summary>
        /// Тип ошибки при конвертации файла
        /// </summary>
        public IEnumerable<FileConvertErrorType> FileConvertErrorType { get; }
    }
}
