using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Models.Interfaces.Errors;

namespace GadzhiConverting.Models.Interfaces.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах на серверной части
    /// </summary>
    public interface IPackageServer
    {
        /// <summary>
        /// ID Идентификатор
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Файлы для конвертирования
        /// </summary>
        IReadOnlyCollection<IFileDataServer> FilesDataServer { get; }

        /// <summary>
        /// Параметры конвертации
        /// </summary>
        IConvertingPackageSettings ConvertingPackageSettings { get; }

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
        bool IsFilesDataValid { get; }

        /// <summary>
        /// Не превышает ли количество попыток конвертирования
        /// </summary>
        bool IsValidByAttemptingCount { get; }

        /// <summary>
        /// Присвоить статус ошибки обработки для всех файлов
        /// </summary>
        IPackageServer SetErrorToAllFiles(IErrorCommon filesError);

        /// <summary>
        /// Присвоить статус обработки проекта
        /// </summary>     
        IPackageServer SetStatusProcessingProject(StatusProcessingProject statusProcessingProject);

        /// <summary>
        /// Заменить файл после конвертирования в пакете
        /// </summary>      
        IPackageServer ChangeFileDataServer(IFileDataServer fileDataServer);
    }
}
