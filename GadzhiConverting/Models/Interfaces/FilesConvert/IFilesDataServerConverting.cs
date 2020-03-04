using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GadzhiConverting.Models.Interfaces.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах на серверной части
    /// </summary>
    public interface IFilesDataServerConverting
    {
        /// <summary>
        /// ID идентефикатор
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Файлы для конвертирования
        /// </summary>
        IReadOnlyList<IFileDataServerConverting> FileDatasServerConverting { get; }

        /// <summary>
        /// Статус выполнения проекта
        /// </summary>      
        StatusProcessingProject StatusProcessingProject { get; set; }

        /// <summary>
        /// Завершена ли обработка
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
        /// Присутствуют ли файлы для конвертации
        /// </summary>
        bool IsValidByFileDatas { get; }

        /// <summary>
        /// Не превышает ли количество попыток конвертирования
        /// </summary>
        bool IsValidByAttemptingCount { get; }

        /// <summary>
        /// Изменить статус обработки для всех файлов
        /// </summary>
        void SetErrorToAllUncompletedFiles();
    }
}
