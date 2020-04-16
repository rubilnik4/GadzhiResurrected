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

namespace GadzhiConverting.Models.Implementations.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах на серверной части
    /// </summary>
    public class FilesDataServer : IFilesDataServer
    {
        public FilesDataServer(IFilesDataServer filesDataServer, IEnumerable<IFileDataServer> fileDatasServer)
            : this(filesDataServer.NonNull().Id, filesDataServer.NonNull().AttemptingConvertCount,
                  filesDataServer.NonNull().StatusProcessingProject, fileDatasServer)
        { }

        public FilesDataServer(Guid id, int attemptingConvertCount, StatusProcessingProject statusProcessingProject,
                               IEnumerable<IFileDataServer> fileDatasServer)
        {
            Id = id;
            AttemptingConvertCount = attemptingConvertCount;
            StatusProcessingProject = statusProcessingProject;
            FileDatasServer = fileDatasServer ?? throw new ArgumentNullException(nameof(fileDatasServer));
        }

        /// <summary>
        /// ID идентефикатор
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Файлы для конвертирования
        /// </summary>
        public IEnumerable<IFileDataServer> FileDatasServer { get; }

        /// <summary>
        /// Статус выполнения проекта
        /// </summary>      
        public StatusProcessingProject StatusProcessingProject { get; }

        /// <summary>
        /// Завершена ли обработка
        /// </summary>
        public bool IsCompleted => CheckStatusProcessing.CompletedStatusProcessingProjectServer.Contains(StatusProcessingProject);

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
        public bool IsValidByFileDatas => FileDatasServer?.Any() == true;

        /// <summary>
        /// Не превышает ли количество попыток конвертирования
        /// </summary>
        public bool IsValidByAttemptingCount => AttemptingConvertCount <= 2;

        /// <summary>
        /// Присвоить статус ошибки обработки для всех файлов
        /// </summary>
        public IFilesDataServer SetErrorToAllFiles() =>
            FileDatasServer.Select(fileData => new FileDataServer(fileData, StatusProcessing.ConvertingComplete,
                                                                  FileConvertErrorType.UnknownError)).
            Map(fileDatas => new FilesDataServer(Id, AttemptingConvertCount, StatusProcessingProject, fileDatas));

        /// <summary>
        /// Присвоить статус обработки проекта
        /// </summary>     
        public IFilesDataServer SetStatusProcessingProject(StatusProcessingProject statusProcessingProject) =>
            statusProcessingProject != StatusProcessingProject ?
            new FilesDataServer(Id, AttemptingConvertCount, statusProcessingProject, FileDatasServer) :
            this;

        /// <summary>
        /// Заменить файл после конвертирования в пакете
        /// </summary>      
        public IFilesDataServer ChangeFileDataServer(IFileDataServer fileDataServer) =>
            FileDatasServer.
            Where(fileData => fileData != fileDataServer).
            Append(fileDataServer).
            Map(fileDatas => new FilesDataServer(this, fileDatas));
    }
}
