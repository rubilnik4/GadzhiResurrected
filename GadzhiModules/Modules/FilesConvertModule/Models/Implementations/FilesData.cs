using GadzhiCommon.Enums.FilesConvert;
using GadzhiModules.Infrastructure.Implementations.Information;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations.Information;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations.ReactiveSubjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;

namespace GadzhiModules.Modules.FilesConvertModule.Models.Implementations
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах
    /// </summary>
    public class FilesData : IFilesData, IDisposable
    {
        /// <summary>
        /// Список файлов для обработки
        /// </summary>
        private List<FileData> _fileDatas;

        /// <summary>
        /// Подписка на изменение коллекции
        /// </summary>
        private readonly Subject<FilesChange> _fileDataChange;

        public FilesData()
            : this(new List<FileData>())
        {

        }

        public FilesData(List<FileData> fileDatas)
        {
            Id = Guid.NewGuid();
            FilesQueueInfo = new FilesQueueInfo();
            StatusProcessingProject = StatusProcessingProject.NeedToLoadFiles;

            _fileDatas = fileDatas;
            _fileDataChange = new Subject<FilesChange>();
        }

        /// <summary>
        /// ID идентефикатор
        /// </summary>    
        public Guid Id { get; }

        /// <summary>
        /// Подписка на изменение коллекции
        /// </summary>
        public ISubject<FilesChange> FileDataChange => _fileDataChange;

        /// <summary>
        /// Данные о конвертируемых файлах
        /// </summary>
        public IReadOnlyList<FileData> FileDatas => _fileDatas;

        /// <summary>
        /// Пути конвертируемых файлов
        /// </summary>
        public IEnumerable<string> FileDatasPath => _fileDatas.Select(file => file.FilePath);

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
            if (files != null)
            {
                //ToList обязателен. Иначе данные зачищаются из списка
                var filesInfo = files?.Select(f => new FileData(f)).
                                       Where(f => CanFileDataBeAddedtoList(f)).ToList();
                if (filesInfo != null)
                {
                    AddFiles(filesInfo);
                }
            }
        }

        /// <summary>
        /// Добавить файлы
        /// </summary>
        public void AddFiles(IEnumerable<FileData> fileDatas)
        {
            if (fileDatas != null)
            {
                //ToList обязателен. Иначе данные зачищаются из списка
                var filesInfo = fileDatas.Where(f => CanFileDataBeAddedtoList(f)).ToList();
                if (filesInfo != null)
                {
                    _fileDatas?.AddRange(filesInfo);
                    bool isStatusProcessingProjectChanged = SetStatusProcessingProject(StatusProcessingProject.NeedToStartConverting);
                    UpdateFileData(new FilesChange(_fileDatas,
                                                   filesInfo,
                                                   ActionType.Add,
                                                   isStatusProcessingProjectChanged));
                }
            }
        }

        /// <summary>
        /// Очистить список файлов
        /// </summary>
        public void ClearFiles()
        {
            _fileDatas?.Clear();
            StatusProcessingProject = StatusProcessingProject.NeedToLoadFiles;
            bool isStatusProcessingProjectChanged = SetStatusProcessingProject(StatusProcessingProject.NeedToLoadFiles);
            UpdateFileData(new FilesChange(_fileDatas,
                                           new List<FileData>(),
                                           ActionType.Clear,
                                           isStatusProcessingProjectChanged));
        }

        /// <summary>
        /// Удалить файлы
        /// </summary>
        public void RemoveFiles(IEnumerable<FileData> files)
        {
            if (files != null)
            {
                _fileDatas?.RemoveAll(f => files?.Contains(f) == true);

                bool isStatusProcessingProjectChanged = false;
                if (_fileDatas == null || _fileDatas.Count == 0)
                {
                    isStatusProcessingProjectChanged = SetStatusProcessingProject(StatusProcessingProject.NeedToLoadFiles);
                }
                else
                {
                    isStatusProcessingProjectChanged = SetStatusProcessingProject(StatusProcessingProject.NeedToStartConverting);
                }

                UpdateFileData(new FilesChange(_fileDatas,
                                               files,
                                               ActionType.Remove,
                                               isStatusProcessingProjectChanged));
            }
        }

        /// <summary>
        /// Измененить статус файла и присвоить при необходимости ошибку
        /// </summary>
        public void ChangeFilesStatus(FilesStatus filesStatus)
        {
            if (filesStatus?.IsValid == true)
            {
                bool isStatusProcessingProjectChanged = SetStatusProcessingProject(filesStatus.StatusProcessingProject);
                FilesQueueInfo.ChangeByFileQueueStatus(filesStatus.FilesQueueStatus, StatusProcessingProject);

                //список файлов для изменений c откорректированным статусом
                var filesDataChanged = filesStatus?.FileStatus.
                                       Select(fileStatus =>
                                       {
                                           var fileData = _fileDatas?.FirstOrDefault(file => file.FilePath == fileStatus.FilePath);
                                           fileData.ChangeByFileStatus(fileStatus);
                                           return fileData;
                                       });

                //формируем данные для отправки изменений
                var fileChange = new FilesChange(_fileDatas,
                                                 filesDataChanged,
                                                 ActionType.StatusChange,
                                                 isStatusProcessingProjectChanged);
                UpdateFileData(fileChange);
            }
        }

        /// <summary>
        /// Измененить статус всех файлов и присвоить ошибку
        /// </summary>
        public void ChangeAllFilesStatusAndMarkError()
        {
            StatusProcessingProject = StatusProcessingProject.Error;
            FilesQueueInfo = new FilesQueueInfo();

            var filesStatus = new FilesStatus(_fileDatas?.
                                              Select(fileData => new FileStatus(fileData.FilePath,
                                                                                StatusProcessing.End,
                                                                                FileConvertErrorType.AbortOperation)),
                                              StatusProcessingProject.Error);

            ChangeFilesStatus(filesStatus);
        }

        /// <summary>
        /// Обновить список файлов
        /// </summary>
        private void UpdateFileData(FilesChange fileChange)
        {
            FileDataChange?.OnNext(fileChange);
        }

        /// <summary>
        /// Можно ли добавить файл в список для конвертирования
        /// </summary>
        private bool CanFileDataBeAddedtoList(FileData file)
        {
            return file != null &&
                   _fileDatas?.Any(f => f.Equals(file)) == false;
        }

        private bool SetStatusProcessingProject(StatusProcessingProject statusProcessingProject)
        {
            bool isChanged = false;
            if (StatusProcessingProject != statusProcessingProject)
            {
                StatusProcessingProject = statusProcessingProject;
                isChanged = true;
            }
            return isChanged;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _fileDataChange.Dispose();
                }

                _fileDatas = null;

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
