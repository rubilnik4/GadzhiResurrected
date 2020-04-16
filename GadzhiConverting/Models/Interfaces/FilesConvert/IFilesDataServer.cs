using ConvertingModels.Models.Interfaces.FilesConvert;
using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GadzhiConverting.Models.Interfaces.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах на серверной части
    /// </summary>
    public interface IFilesDataServer
    {
        /// <summary>
        /// ID идентефикатор
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Файлы для конвертирования
        /// </summary>
        IEnumerable<IFileDataServer> FileDatasServer { get; }

        /// <summary>
        /// Статус выполнения проекта
        /// </summary>      
        StatusProcessingProject StatusProcessingProject { get; }

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
        /// Присвоить статус ошибки обработки для всех файлов
        /// </summary>
        IFilesDataServer SetErrorToAllFiles();

        /// <summary>
        /// Присвоить статус обработки проекта
        /// </summary>     
        IFilesDataServer SetStatusProcessingProject(StatusProcessingProject statusProcessingProject);

        /// <summary>
        /// Заменить файл после конвертирования в пакете
        /// </summary>      
        IFilesDataServer ChangeFileDataServer(IFileDataServer fileDataServer);
    }
}
