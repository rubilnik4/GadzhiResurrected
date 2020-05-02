using ConvertingModels.Models.Interfaces.FilesConvert;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extentions;
using GadzhiCommon.Extentions.Functional;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Models.Interfaces.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Extensions;
using GadzhiCommon.Extensions.Functional;

namespace GadzhiConverting.Models.Implementations.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах на серверной части
    /// </summary>
    public class PackageServer : IPackageServer
    {
        public PackageServer(IPackageServer packageServer, IEnumerable<IFileDataServer> fileDatasServer)
            : this(packageServer.NonNull().Id, packageServer.NonNull().AttemptingConvertCount,
                  packageServer.NonNull().StatusProcessingProject, fileDatasServer)
        { }

        public PackageServer(Guid id, int attemptingConvertCount, StatusProcessingProject statusProcessingProject,
                               IEnumerable<IFileDataServer> fileDatasServer)
        {
            Id = id;
            AttemptingConvertCount = attemptingConvertCount;
            StatusProcessingProject = statusProcessingProject;
            FilesDataServer = fileDatasServer ?? throw new ArgumentNullException(nameof(fileDatasServer));
        }

        /// <summary>
        /// ID идентефикатор
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Файлы для конвертирования
        /// </summary>
        public IEnumerable<IFileDataServer> FilesDataServer { get; }

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
        public bool IsValid => IsValidByFileDatas &&
                               IsValidByAttemptingCount;

        /// <summary>
        /// Присутствуют ли файлы для конвертации
        /// </summary>
        public bool IsValidByFileDatas => FilesDataServer?.Any() == true;

        /// <summary>
        /// Не превышает ли количество попыток конвертирования
        /// </summary>
        public bool IsValidByAttemptingCount => AttemptingConvertCount <= 2;

        /// <summary>
        /// Присвоить статус ошибки обработки для всех файлов
        /// </summary>
        public IPackageServer SetErrorToAllFiles() =>
            FilesDataServer.Select(fileData => new FileDataServer(fileData, StatusProcessing.ConvertingComplete,
                                                                  FileConvertErrorType.UnknownError)).
            Map(fileDatas => new PackageServer(Id, AttemptingConvertCount, StatusProcessingProject, fileDatas));

        /// <summary>
        /// Присвоить статус обработки проекта
        /// </summary>     
        public IPackageServer SetStatusProcessingProject(StatusProcessingProject statusProcessingProject) =>
            statusProcessingProject != StatusProcessingProject ?
            new PackageServer(Id, AttemptingConvertCount, statusProcessingProject, FilesDataServer) :
            this;

        /// <summary>
        /// Заменить файл после конвертирования в пакете
        /// </summary>      
        public IPackageServer ChangeFileDataServer(IFileDataServer fileDataServer) =>
            FilesDataServer.
            Where(fileData => !fileData.Equals(fileDataServer)).
            Append(fileDataServer).
            Map(fileDatas => new PackageServer(this, fileDatas));
    }
}
