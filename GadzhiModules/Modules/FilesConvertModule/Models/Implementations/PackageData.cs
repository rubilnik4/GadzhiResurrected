﻿using GadzhiCommon.Enums.FilesConvert;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations.Information;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations.ReactiveSubjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using GadzhiModules.Modules.FilesConvertModule.Models.Interfaces;

namespace GadzhiModules.Modules.FilesConvertModule.Models.Implementations
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах
    /// </summary>
    public class PackageData : IPackageData
    {
        /// <summary>
        /// Список файлов для обработки
        /// </summary>
        private List<FileData> _filesData;

        /// <summary>
        /// Подписка на изменение коллекции
        /// </summary>
        private readonly Subject<FilesChange> _fileDataChange;

        public PackageData()
            : this(new List<FileData>())
        {

        }

        public PackageData(List<FileData> filesData)
        {
            Id = Guid.NewGuid();
            FilesQueueInfo = new FilesQueueInfo();
            StatusProcessingProject = StatusProcessingProject.NeedToLoadFiles;

            _filesData = filesData;
            _fileDataChange = new Subject<FilesChange>();
        }

        /// <summary>
        /// ID идентификатор
        /// </summary>    
        public Guid Id { get; }

        /// <summary>
        /// Подписка на изменение коллекции
        /// </summary>
        public ISubject<FilesChange> FileDataChange => _fileDataChange;

        /// <summary>
        /// Данные о конвертируемых файлах
        /// </summary>
        public IReadOnlyList<FileData> FilesData => _filesData;

        /// <summary>
        /// Пути конвертируемых файлов
        /// </summary>
        public IEnumerable<string> FilesDataPath => _filesData.Select(file => file.FilePath);

        /// <summary>
        /// Статус выполнения проекта
        /// </summary>
        public StatusProcessingProject StatusProcessingProject { get; private set; }

        /// <summary>
        /// Информация о количестве файлов в очереди на сервере
        /// </summary>
        public FilesQueueInfo FilesQueueInfo { get; private set; }

        /// <summary>
        /// Добавить файл
        /// </summary>
        public void AddFile(FileData fileData)
        {
            AddFiles(new List<FileData>() { fileData });
        }

        /// <summary>
        /// Добавить файлы
        /// </summary>
        public void AddFiles(IEnumerable<string> files)
        {
            var filesInfo = files?.Select(f => new FileData(f)).
                                   Where(CanFileDataBeAddedToList).
                                   ToList();
            if (filesInfo != null)
            {
                AddFiles(filesInfo);
            }
        }

        /// <summary>
        /// Добавить файлы
        /// </summary>
        public void AddFiles(IEnumerable<FileData> filesData)
        {
            var filesInfo = filesData?.
                            Where(CanFileDataBeAddedToList).
                            ToList();

            if (filesInfo == null) return;

            _filesData?.AddRange(filesInfo);
            bool isStatusProcessingProjectChanged = SetStatusProcessingProject(StatusProcessingProject.NeedToStartConverting);
            UpdateFileData(new FilesChange(_filesData, filesInfo, ActionType.Add, isStatusProcessingProjectChanged));
        }

        /// <summary>
        /// Очистить список файлов
        /// </summary>
        public void ClearFiles()
        {
            _filesData?.Clear();
            StatusProcessingProject = StatusProcessingProject.NeedToLoadFiles;
            bool isStatusProcessingProjectChanged = SetStatusProcessingProject(StatusProcessingProject.NeedToLoadFiles);
            UpdateFileData(new FilesChange(_filesData, new List<FileData>(), ActionType.Clear, isStatusProcessingProjectChanged));
        }

        /// <summary>
        /// Удалить файлы
        /// </summary>
        public void RemoveFiles(IEnumerable<FileData> filesData)
        {
            if (filesData == null) return;
            var filesDataCollection = filesData.ToList();

            _filesData?.RemoveAll(filesDataCollection.Contains);

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
            FilesQueueInfo.ChangeByQueueStatus(packageStatus.QueueStatus, StatusProcessingProject);

            //список файлов для изменений c откорректированным статусом
            var filesDataChanged =
                packageStatus.FileStatus.
                Select(fileStatus =>
                {
                    var fileData = _filesData?.FirstOrDefault(file => file.FilePath == fileStatus.FilePath);
                    fileData?.ChangeByFileStatus(fileStatus);
                    return fileData;
                });

            //формируем данные для отправки изменений
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

            var fileData = _filesData?.Select(file => new FileStatus(file.FilePath,
                                                                     StatusProcessing.End,
                                                                     FileConvertErrorType.AbortOperation));
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
        private bool CanFileDataBeAddedToList(FileData file) =>
            file != null && _filesData?.Any(f => f.Equals(file)) == false;

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

            _filesData = null;

            _disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
