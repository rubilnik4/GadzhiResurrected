﻿using System.Collections.Generic;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiConverting.Models.Implementations.FilesConvert;

namespace GadzhiConverting.Models.Interfaces.FilesConvert
{
    /// <summary>
    /// Класс для хранения информации о конвертируемом файле
    /// </summary>
    public interface IFileDataServer: IFilePath
    {
        /// <summary>
        /// Цвет печати
        /// </summary>
        ColorPrint ColorPrint { get; }

        /// <summary>
        /// Статус обработки файла
        /// </summary>
        StatusProcessing StatusProcessing { get; }

        /// <summary>
        /// Завершена ли обработка файла
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// Количество попыток конвертирования
        /// </summary>      
        int AttemptingConvertCount { get; }

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
        /// Тип ошибки при конвертации файла
        /// </summary>
        IEnumerable<FileConvertErrorType> FileConvertErrorTypes { get; }

        /// <summary>
        /// Путь и тип отконвертированных файлов
        /// </summary>
        IEnumerable<IFileDataSourceServer> FilesDataSourceServer { get; }

        /// <summary>
        /// Установить количество попыток конвертирования
        /// </summary>       
        IFileDataServer SetAttemptingCount(int attemptingCount);
    }
}
