﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Infrastructure.Implementations;
using GadzhiCommon.Infrastructure.Implementations.Logger;
using GadzhiCommon.Infrastructure.Implementations.Reflection;
using GadzhiCommon.Infrastructure.Interfaces.Logger;
using GadzhiCommon.Models.Enums;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting.Information;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting.ReactiveSubjects;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Interfaces.FileConverting;

namespace GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Implementations.FileConverting
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах
    /// </summary>
    public class PackageData : IPackageData
    {
        public PackageData()
            : this(new List<IFileData>())
        { }

        public PackageData(IEnumerable<IFileData> filesData)
        {
            Id = Guid.Empty;
            FilesQueueInfo = new FilesQueueInfo();
            SetStatusProcessingProject(StatusProcessingProject.NeedToLoadFiles);
            _filesData = filesData.ToList();
        }

        /// <summary>
        /// Журнал системных сообщений
        /// </summary>
        private static readonly ILoggerService _loggerService = LoggerFactory.GetFileLogger();

        /// <summary>
        /// Список файлов для обработки
        /// </summary>
        private readonly List<IFileData> _filesData;

        /// <summary>
        /// ID идентификатор
        /// </summary>    
        public Guid Id { get; private set; }

        /// <summary>
        /// Подписка на изменение коллекции
        /// </summary>
        public ISubject<FilesChange> FileDataChange { get; } = 
            new Subject<FilesChange>();

        /// <summary>
        /// Данные о конвертируемых файлах
        /// </summary>
        public IReadOnlyList<IFileData> FilesData =>
            _filesData;

        /// <summary>
        /// Пути конвертируемых файлов
        /// </summary>
        public IReadOnlyCollection<string> FilesDataPath =>
            _filesData.Select(file => file.FilePath).
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
        public void AddFile(IFileData fileData) =>
            AddFiles(new List<IFileData> { fileData });

        /// <summary>
        /// Добавить файлы
        /// </summary>
        public void AddFiles(IEnumerable<string> files, ColorPrintType colorPrintType) =>
            files?.Select(f => new FileData(f, colorPrintType)).Where(CanFileDataBeAddedToList).
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
        /// Получить список файлов для загрузки из базы
        /// </summary>
        public IReadOnlyCollection<string> GetFilesToDownload(PackageStatus packageStatus) =>
            packageStatus.FileStatus.
            Where(fileStatus => CheckStatusProcessing.CompletedStatusProcessing.Contains(fileStatus.StatusProcessing)).
            Select(fileStatus => fileStatus.FilePath).
            Intersect(_filesData.Where(fileData => !CheckStatusProcessing.CompletedStatusProcessing.Contains(fileData.StatusProcessing)).
                                 Select(fileData => fileData.FilePath)).
            ToList();

        /// <summary>
        /// Изменить статус файла и присвоить при необходимости ошибку
        /// </summary>
        public void ChangeFileStatus(StatusProcessingProject statusProcessingProject, FileStatus fileStatus)
        {
            bool isStatusProjectChanged = SetStatusProcessingProject(statusProcessingProject);
            var filesDataChanged = _filesData.First(file => file.FilePath == fileStatus.FilePath).
                                   ChangeByFileStatus(fileStatus);

            var fileChange = new FilesChange(_filesData, filesDataChanged, ActionType.StatusChange, isStatusProjectChanged);
            UpdateFileData(fileChange);
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
            var fileData = _filesData.Select(file => new FileStatus(file.FilePath, StatusProcessing.End, errorStatus));
            var filesStatus = new PackageStatus(fileData, StatusProcessingProject.End);
            ChangeFilesStatus(filesStatus);
        }

        /// <summary>
        /// Установить статус конвертирования пакета
        /// </summary>
        private bool SetStatusProcessingProject(StatusProcessingProject statusProcessingProject)
        {
            if (StatusProcessingProject == statusProcessingProject) return false;

            StatusProcessingProject = statusProcessingProject;
            return true;
        }

        /// <summary>
        /// Обновить список файлов
        /// </summary>
        private void UpdateFileData(FilesChange fileChange) =>
            FileDataChange.OnNext(fileChange);

        /// <summary>
        /// Можно ли добавить файл в список для конвертирования
        /// </summary>
        private bool CanFileDataBeAddedToList(IFileData file) =>
            file != null && _filesData.IndexOf(file) == -1;

        #region IDisposable Support
        private bool _disposedValue;

        [Logger]
        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;

            if (disposing)
            {
                FileDataChange.OnCompleted();
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
