using ConvertingModels.Models.Implementations.FilesConvert;
using ConvertingModels.Models.Interfaces.FilesConvert;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiConverting.Models.Interfaces.FilesConvert;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Models.Implementations.FilesConvert
{
    /// <summary>
    /// Класс для хранения информации о конвертируемом файле для приложения
    /// </summary>
    public class FileDataServerConverting : FileDataServer, IFileDataServerConverting
    {
        public FileDataServerConverting(string filePathServer, string filePathClient, ColorPrint colorPrint)
              : this(filePathServer, filePathClient, colorPrint, new List<FileConvertErrorType>())
        {

        }

        public FileDataServerConverting(string filePathServer, string filePathClient,
                                  ColorPrint colorPrint, IEnumerable<FileConvertErrorType> fileConvertErrorType)
            : base(filePathServer, filePathClient, colorPrint, fileConvertErrorType)
        {

        }

        /// <summary>
        /// Статус обработки файла
        /// </summary>
        public StatusProcessing StatusProcessing { get; set; }

        /// <summary>
        /// Завершена ли обработка файла
        /// </summary>
        public bool IsCompleted => CheckStatusProcessing.CompletedStatusProcessingServer.Contains(StatusProcessing);

        /// <summary>
        /// Количество попыток конвертирования
        /// </summary>      
        public int AttemptingConvertCount { get; set; }

        /// <summary>
        /// Корректна ли модель
        /// </summary>
        public bool IsValid => IsValidByErrorType && IsValidByAttemptingCount;

        /// <summary>
        /// Присутствуют ли ошибки конвертирования
        /// </summary>
        public bool IsValidByErrorType => FileConvertErrorTypesBase == null || FileConvertErrorTypesBase.Count == 0;

        /// <summary>
        /// Не превышает ли количество попыток конвертирования
        /// </summary>
        public bool IsValidByAttemptingCount => AttemptingConvertCount <= 2;

        /// <summary>
        /// Установить пути для отконвертированных файлов
        /// </summary>
        public void SetFileDatasSourceServerConverting(IEnumerable<IFileDataSourceServerConverting> fileDatasSourceServerConverting)
        {
            ClearConvertedFilePathBase();
            AddRangeConvertedFilePathBase(fileDatasSourceServerConverting?.Cast<IFileDataSourceServer>());
        }
    }
}
