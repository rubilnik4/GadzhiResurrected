using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using GadzhiCommon.Enums.FilesConvert;
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
        /// Список файлов для обработки
        /// </summary>
        private readonly List<IFileData> _filesData;

        /// <summary>
        /// Подписка на изменение коллекции
        /// </summary>
        private readonly Subject<FilesChange> _fileDataChange;

        public PackageData()
            : this(new List<IFileData>())
        { }

        public PackageData(IEnumerable<IFileData> filesData)
        {
            Id = Guid.Empty;
            FilesQueueInfo = new FilesQueueInfo();
            StatusProcessingProject = StatusProcessingProject.NeedToLoadFiles;

            _filesData = filesData?.ToList() ?? new List<IFileData>();
            _fileDataChange = new Subject<FilesChange>();
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
        public void AddFile(IFileData fileData)
        {
            AddFiles(new List<IFileData>() { fileData });
        }

        /// <summary>
        /// Добавить файлы
        /// </summary>
        public void AddFiles(IEnumerable<string> files)
        {
            var filesInfo = files?.Select(f => new FileData(f)).
                                   Where(CanFileDataBeAddedToList).
                                   ToList();
            AddFiles(filesInfo ?? new List<FileData>());
        }

        /// <summary>
        /// Добавить файлы
        /// </summary>
        public void AddFiles(IEnumerable<IFileData> filesData)
        {
            var filesInfo = filesData?.Where(CanFileDataBeAddedToList).
                                       ToList();

            if (filesInfo == null) return;

            _filesData.AddRange(filesInfo);
            bool isStatusProcessingProjectChanged = SetStatusProcessingProject(StatusProcessingProject.NeedToStartConverting);
            UpdateFileData(new FilesChange(_filesData, filesInfo, ActionType.Add, isStatusProcessingProjectChanged));
        }

        /// <summary>
        /// Очистить список файлов
        /// </summary>
        public void ClearFiles()
        {
            _filesData.Clear();
            StatusProcessingProject = StatusProcessingProject.NeedToLoadFiles;
            bool isStatusProcessingProjectChanged = SetStatusProcessingProject(StatusProcessingProject.NeedToLoadFiles);
            UpdateFileData(new FilesChange(_filesData, new List<FileData>(), ActionType.Clear, isStatusProcessingProjectChanged));
        }

        /// <summary>
        /// Удалить файлы
        /// </summary>
        public void RemoveFiles(IEnumerable<IFileData> filesData)
        {
            if (filesData == null) return;
            var filesDataCollection = filesData.ToList();

            _filesData.RemoveAll(filesDataCollection.Contains);

            bool isStatusProcessingProjectChanged;
            if (_filesData == null || _filesData.Count == 0)
            {
                isStatusProcessingProjectChanged = SetStatusProcessingProject(StatusProcessingProject.NeedToLoadFiles);
            }
            else
            {
                isStatusProcessingProjectChanged = SetStatusProcessingProject(StatusProcessingProject.NeedToStartConverting);
            }

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
        public void ChangeAllFilesStatusAndMarkError()
        {
            StatusProcessingProject = StatusProcessingProject.Error;
            FilesQueueInfo = new FilesQueueInfo();

            var fileData = _filesData.Select(file => new FileStatus(file.FilePath, StatusProcessing.End, FileConvertErrorType.AbortOperation));
            var filesStatus = new PackageStatus(fileData, StatusProcessingProject.Error);
            ChangeFilesStatus(filesStatus);
        }

        /// <summary>
        /// Обновить список файлов
        /// </summary>
        private void UpdateFileData(FilesChange fileChange) => FileDataChange?.OnNext(fileChange);

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

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            if (disposing)
            {
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
