using ConvertingModels.Models.Implementations.FilesConvert;
using ConvertingModels.Models.Interfaces.FilesConvert;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Models.Interfaces.FilesConvert
{
    /// <summary>
    /// Класс для хранения информации о конвертируемом файле для приложения
    /// </summary>
    public interface IFileDataServerConverting : IFileDataServer
    {

        /// <summary>
        /// Статус обработки файла
        /// </summary>
        StatusProcessing StatusProcessing { get; set; }

        /// <summary>
        /// Завершена ли обработка файла
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// Количество попыток конвертирования
        /// </summary>      
        int AttemptingConvertCount { get; set; }

        /// <summary>
        /// Корректна ли модель
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Присутствуют ли ошибки конвертирования
        /// </summary>
        bool IsValidByErrorType { get; }

        /// <summary>
        /// Не превышает ли количество попыток конвертирования
        /// </summary>
        bool IsValidByAttemptingCount { get; }

        /// <summary>
        /// Установить пути для отконвертированных файлов
        /// </summary>
        void SetFileDatasSourceServerConverting(IEnumerable<IFileDataSourceServerConverting> fileDatasSourceServerConverting);
    }
}
