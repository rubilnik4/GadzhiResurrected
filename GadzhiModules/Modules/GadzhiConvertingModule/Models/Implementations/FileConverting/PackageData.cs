using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Infrastructure.Implementations.Reflection;
using GadzhiCommon.Infrastructure.Interfaces.Logger;
using GadzhiCommon.Models.Enums;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting.Information;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting.ReactiveSubjects;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.FileConverting;

namespace GadzhiModules.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах
    /// </summary>
    public class PackageData : IPackageData
    {
        /// <summary>
        /// Журнал системных сообщений
        /// </summary>
        private readonly ILoggerService _loggerService = LoggerFactory.GetFileLogger();

        /// <summary>
        /// Список файлов для обработки
        /// </summary>
        private readonly List<IFileData> _filesData;

        public PackageData()
            : this(new List<IFileData>())
        { }

        public PackageData(IEnumerable<IFileData> filesData)
        {
            Id = Guid.Empty;
            FilesQueueInfo = new FilesQueueInfo();
            SetStatusProcessingProject(StatusProcessingProject.NeedToLoadFiles);

            _filesData = filesData?.ToList() ?? new List<IFileData>();
        }

        /// <summary>
        /// ID идентификатор
        /// </summary>    
        public Guid Id { get; private set; }

        /// <summary>
        /// Подписка на изменение коллекции
        /// </summary>
        public ISubject<FilesChange> FileDataChange => _fileDataChange;

        /// <summary>
        /// Подписка на изменение коллекции
        /// </summary>
        private readonly Subject<FilesChange> _fileDataChange = new Subject<FilesChange>();

        /// <summary>
        /// Данные о конвертируемых файлах
        /// </summary>
        public IReadOnlyList<IFileData> FilesData => _filesData;

        /// <summary>
        /// Пути конвертируемых файлов
        /// </summary>
        public IReadOnlyCollection<string> FilesDataPath => _filesData.Select(file => file.FilePath).
                                                                       ToList().AsReadOnly();

        /// <summary>
        /// Статус выполнения проекта
        /// </summary>
        [Logger]
        public StatusProcessingProject StatusProcessingProject { get; private set; }

        /// <summary>
        /// Информация о количестве файлов в очереди на сервере
        /// </summary>
        public FilesQueueInfo FilesQueueInfo { get; private set; }

        /// <summary>
        /// Сгенерировать идентификатор
        /// </summary>
        public Guid GenerateId()
        {
            Id = Guid.NewGuid();
            return Id;
        }

        /// <summary>
        /// Добавить файл
        /// </summary>
        public void AddFile(IFileData fileData) => AddFiles(new List<IFileData>() { fileData });

        /// <summary>
        /// Добавить файлы
        /// </summary>
        public void AddFiles(IEnumerable<string> files) =>
            files?.Select(f => new FileData(f)).
                   Where(CanFileDataBeAddedToList).
            Void(AddFiles);

        /// <summary>
        /// Добавить файлы
        /// </summary>
        public void AddFiles(IEnumerable<IFileData> filesData)
        {
            var filesDataToAdd = filesData?.Where(CanFileDataBeAddedToList).ToList();
            if (filesDataToAdd == null || filesDataToAdd.Count == 0) return;

            _filesData.AddRange(filesDataToAdd);
            _loggerService.LogByObjects(LoggerLevel.Info, LoggerAction.Add, ReflectionInfo.GetMethodBase(this), filesDataToAdd);

            bool isStatusProcessingProjectChanged = SetStatusProcessingProject(StatusProcessingProject.NeedToStartConverting);
            UpdateFileData(new FilesChange(_filesData, filesDataToAdd, ActionType.Add, isStatusProcessingProjectChanged));
        }

        /// <summary>
        /// Очистить список файлов
        /// </summary>
        public void ClearFiles()
        {
            _filesData.Clear();
            _loggerService.LogByObject<IFileData>(LoggerLevel.Info, LoggerAction.Clear, ReflectionInfo.GetMethodBase(this));

            bool isStatusProcessingProjectChanged = SetStatusProcessingProject(StatusProcessingProject.NeedToLoadFiles);
            UpdateFileData(new FilesChange(_filesData, new List<FileData>(), ActionType.Clear, isStatusProcessingProjectChanged));
        }

        /// <summary>
        /// Удалить файлы
        /// </summary>
        public void RemoveFiles(IEnumerable<IFileData> filesData)
        {
            var filesDataCollection = filesData?.ToList();
            if (filesDataCollection == null || filesDataCollection.Count == 0) return;

            _filesData.RemoveAll(filesDataCollection.Contains);
            _loggerService.LogByObjects(LoggerLevel.Info, LoggerAction.Remove, ReflectionInfo.GetMethodBase(this), filesDataCollection);

            bool isStatusProcessingProjectChanged = (_filesData == null || _filesData.Count == 0)
                                                    ? SetStatusProcessingProject(StatusProcessingProject.NeedToLoadFiles)
                                                    : SetStatusProcessingProject(StatusProcessingProject.NeedToStartConverting);
            UpdateFileData(new FilesChange(_filesData, filesDataCollection, ActionType.Remove, isStatusProcessingProjectChanged));
        }

        /// <summary>
        /// Изменить статус файла и присвоить при необходимости ошибку
        /// </summary>
        public void ChangeFilesStatus(PackageStatus packageStatus)
        {
            if (packageStatus?.IsValid != true) return;

            bool isStatusProcessingProjectChanged = SetStatusProcessingProject(packageStatus.StatusProcessingProject);
            FilesQueueInfo = FilesQueueInfo.GetQueueInfoByStatus(packageStatus.QueueStatus, StatusProcessingProject);

            var filesDataChanged = packageStatus.FileStatus.
                                   Select(fileStatus => _filesData.FirstOrDefault(file => file.FilePath == fileStatus.FilePath)?.
                                                                   ChangeByFileStatus(fileStatus)).
                                   Where(fileData => fileData != null);

            var fileChange = new FilesChange(_filesData, filesDataChanged, ActionType.StatusChange, isStatusProcessingProjectChanged);
            UpdateFileData(fileChange);
        }

        /// <summary>
        /// Изменить статус всех файлов и присвоить ошибку
        /// </summary>
        [Logger]
        public void ChangeAllFilesStatusAndSetError(IErrorCommon errorStatus)
        {
            FilesQueueInfo = new FilesQueueInfo();
           // new ErrorCommon(FileConvertErrorType.AbortOperation, "Операция конвертирования отменена")
            var fileData = _filesData.Select(file => new FileStatus(file.FilePath, StatusProcessing.End, errorStatus));
            var filesStatus = new PackageStatus(fileData, StatusProcessingProject.End);
            ChangeFilesStatus(filesStatus);
        }

        /// <summary>
        /// Обновить список файлов
        /// </summary>
        private void UpdateFileData(FilesChange fileChange) => _fileDataChange?.OnNext(fileChange);

        /// <summary>
        /// Можно ли добавить файл в список для конвертирования
        /// </summary>
        private bool CanFileDataBeAddedToList(IFileData file) => file != null && _filesData.IndexOf(file) == -1;

        /// <summary>
        /// Установить статус конвертирования пакета
        /// </summary>
        private bool SetStatusProcessingProject(StatusProcessingProject statusProcessingProject)
        {
            if (StatusProcessingProject == statusProcessingProject) return false;

            StatusProcessingProject = statusProcessingProject;
            return true;
        }

        #region IDisposable Support
        private bool _disposedValue;

        [Logger]
        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;

            if (disposing)
            {
                _fileDataChange.OnCompleted();
                _fileDataChange.Dispose();
            }

            _disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
