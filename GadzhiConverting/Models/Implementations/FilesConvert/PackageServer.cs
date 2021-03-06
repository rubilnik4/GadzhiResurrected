﻿using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiConverting.Models.Interfaces.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiCommon.Models.Interfaces.FilesConvert;

namespace GadzhiConverting.Models.Implementations.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах на серверной части
    /// </summary>
    public class PackageServer : IPackageServer
    {
        public PackageServer(IPackageServer packageServer, IEnumerable<IFileDataServer> filesDataServer)
            : this(packageServer.NonNull().Id, packageServer.NonNull().AttemptingConvertCount,
                  packageServer.NonNull().StatusProcessingProject, packageServer.NonNull().ConvertingPackageSettings,
                  filesDataServer)
        { }

        public PackageServer(Guid id, int attemptingConvertCount, StatusProcessingProject statusProcessingProject,
                             IConvertingPackageSettings convertingSettings, IEnumerable<IFileDataServer> filesDataServer)
        {
            Id = id;
            AttemptingConvertCount = attemptingConvertCount;
            StatusProcessingProject = statusProcessingProject;
            ConvertingPackageSettings = convertingSettings;
            FilesDataServer = filesDataServer.ToList().AsReadOnly();
        }

        /// <summary>
        /// ID Идентификатор
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Файлы для конвертирования
        /// </summary>
        public IReadOnlyCollection<IFileDataServer> FilesDataServer { get; }

        /// <summary>
        /// Параметры конвертации
        /// </summary>
        public IConvertingPackageSettings ConvertingPackageSettings { get; }

        /// <summary>
        /// Статус выполнения проекта
        /// </summary>      
        public StatusProcessingProject StatusProcessingProject { get; }

        /// <summary>
        /// Завершена ли обработка
        /// </summary>
        public bool IsCompleted => CheckStatusProcessing.CompletedStatusProcessingProject.Contains(StatusProcessingProject);

        /// <summary>
        /// Количество попыток конвертирования
        /// </summary>      
        public int AttemptingConvertCount { get; }

        /// <summary>
        /// Корректна ли модель
        /// </summary>
        public bool IsValid => IsFilesDataValid && IsValidByAttemptingCount;

        /// <summary>
        /// Присутствуют ли файлы для конвертации
        /// </summary>
        public bool IsFilesDataValid => FilesDataServer?.Any() == true;

        /// <summary>
        /// Не превышает ли количество попыток конвертирования
        /// </summary>
        public bool IsValidByAttemptingCount => AttemptingConvertCount <= 2;

        /// <summary>
        /// Присвоить статус ошибки обработки для всех файлов
        /// </summary>
        public IPackageServer SetErrorToAllFiles(IErrorCommon filesError) =>
            FilesDataServer.Select(fileData => new FileDataServer(fileData, StatusProcessing.ConvertingComplete, filesError)).
            Map(filesData => new PackageServer(Id, AttemptingConvertCount, StatusProcessingProject, ConvertingPackageSettings, filesData));

        /// <summary>
        /// Присвоить статус обработки проекта
        /// </summary>     
        public IPackageServer SetStatusProcessingProject(StatusProcessingProject statusProcessingProject) =>
            statusProcessingProject != StatusProcessingProject ?
            new PackageServer(Id, AttemptingConvertCount, statusProcessingProject, ConvertingPackageSettings, FilesDataServer) :
            this;

        /// <summary>
        /// Заменить файл после конвертирования в пакете
        /// </summary>      
        public IPackageServer ChangeFileDataServer(IFileDataServer fileDataServer) =>
            FilesDataServer.
            Where(fileData => fileData.FilePathServer != fileDataServer.FilePathServer).
            Append(fileDataServer).
            Map(filesData => new PackageServer(this, filesData));
    }
}
