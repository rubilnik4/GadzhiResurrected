using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations.Information;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations.ReactiveSubjects;
// ReSharper disable UnusedMemberInSuper.Global

namespace GadzhiModules.Modules.FilesConvertModule.Models.Interfaces
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах
    /// </summary>
    public interface IPackageData : IDisposable
    {
        /// <summary>
        /// ID идентификатор
        /// </summary>    
        Guid Id { get; }

        /// <summary>
        /// Подписка на изменение коллекции
        /// </summary>
        ISubject<FilesChange> FileDataChange { get; }

        /// <summary>
        /// Данные о конвертируемых файлах
        /// </summary>
        IReadOnlyList<FileData> FilesData { get; }

        /// <summary>
        /// Пути конвертируемых файлов
        /// </summary>     
        IEnumerable<string> FilesDataPath { get; }

        /// <summary>
        /// Статус выполнения проекта
        /// </summary>
        StatusProcessingProject StatusProcessingProject { get; }

        /// <summary>
        /// Информация о количестве файлов в очереди на сервере
        /// </summary>
        FilesQueueInfo FilesQueueInfo { get; }

        /// <summary>
        /// Сгенерировать идентификатор
        /// </summary>
        public Guid GenerateId();

        /// <summary>
        /// Добавить файл
        /// </summary>
        void AddFile(FileData file);

        /// <summary>
        /// Добавить файлы
        /// </summary>
        void AddFiles(IEnumerable<FileData> filesData);

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
        void RemoveFiles(IEnumerable<FileData> filesData);

        /// <summary>
        /// Изменить статус файлов и присвоить при необходимости ошибку
        /// </summary>
        void ChangeFilesStatus(PackageStatus packageStatus);

        /// <summary>
        /// Изменить статус всех файлов и присвоить ошибку
        /// </summary>
        void ChangeAllFilesStatusAndMarkError();
    }
}
