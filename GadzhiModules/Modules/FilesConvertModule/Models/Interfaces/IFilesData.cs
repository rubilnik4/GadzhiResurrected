using GadzhiCommon.Enums.FilesConvert;
using GadzhiModules.Helpers;
using GadzhiModules.Infrastructure.Implementations.Information;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations.Information;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations.ReactiveSubjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Modules.FilesConvertModule.Models.Implementations
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах
    /// </summary>
    public interface IFilesData
    {
        /// <summary>
        /// ID идентефикатор
        /// </summary>    
        Guid ID { get; }

        /// <summary>
        /// Подписка на изменение коллекции
        /// </summary>
        ISubject<FilesChange> FileDataChange { get; }

        /// <summary>
        /// Данные о конвертируемых файлах
        /// </summary>
        IReadOnlyList<FileData> FilesInfo { get; }

        /// <summary>
        /// Пути конвертируемых файлов
        /// </summary>     
        IEnumerable<string> FilesInfoPath { get; }
        
        /// <summary>
        /// Статус выполнения проекта
        /// </summary>
        StatusProcessingProject StatusProcessingProject { get; }

        /// <summary>
        /// Информация о количестве файлов в очереди на сервере
        /// </summary>
        FilesQueueInfo FilesQueueInfo { get; }

        /// <summary>
        /// Добавить файл
        /// </summary>
        void AddFile(FileData file);

        /// <summary>
        /// Добавить файлы
        /// </summary>
        void AddFiles(IEnumerable<FileData> files);

        /// <summary>
        /// Добавить файлы
        /// </summary>
        void AddFiles(IEnumerable<string> files);

        /// <summary>
        /// Очистить список файлов
        /// </summary>
        void ClearFiles();

        /// <summary>
        /// Удалить файлы
        /// </summary>
        void RemoveFiles(IEnumerable<FileData> files);

        /// <summary>
        /// Измененить статус файлов и присвоить при необходимости ошибку
        /// </summary>
        void ChangeFilesStatus(FilesStatus filesStatus);

        /// <summary>
        /// Измененить статус всех файлов и присвоить ошибку
        /// </summary>
        void ChangeAllFilesStatusAndMarkError();
    }
}
