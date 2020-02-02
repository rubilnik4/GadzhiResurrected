using GadzhiCommon.Enums.FilesConvert;
using GadzhiModules.Helpers;
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
    public class FilesData : IFilesData
    {
        /// <summary>
        /// Список файлов для обработки
        /// </summary>
        private List<FileData> _filesInfo;

        public FilesData()
            : this(new List<FileData>())
        {

        }

        public FilesData(List<FileData> files)
        {
            ID = Guid.NewGuid();
            StatusProcessingProject = StatusProcessingProject.NeedToLoadFiles;

            _filesInfo = files;
            FileDataChange = new Subject<FilesChange>();
        }

        /// <summary>
        /// ID идентефикатор
        /// </summary>    
        public Guid ID { get; }

        /// <summary>
        /// Подписка на изменение коллекции
        /// </summary>
        public ISubject<FilesChange> FileDataChange { get; }

        /// <summary>
        /// Данные о конвертируемых файлах
        /// </summary>
        public IReadOnlyList<FileData> FilesInfo => _filesInfo;

        /// <summary>
        /// Пути конвертируемых файлов
        /// </summary>
        public IEnumerable<string> FilesInfoPath => _filesInfo.Select(file => file.FilePath);

        /// <summary>
        /// Статус выполнения проекта
        /// </summary>
        public StatusProcessingProject StatusProcessingProject { get; private set; }

        /// <summary>
        /// Добавить файл
        /// </summary>
        public void AddFile(FileData file)
        {
            if (CanFileDataBeAddedtoList(file))
            {
                _filesInfo?.Add(file);
                UpdateFileData(new FilesChange(_filesInfo,
                                              new List<FileData>() { file },
                                              ActionType.Add));
            }
        }

        /// <summary>
        /// Добавить файлы
        /// </summary>
        public void AddFiles(IEnumerable<FileData> files)
        {
            if (files != null)
            {
                //ToList обязателен. Иначе данные зачищаются из списка
                var filesInfo = files.Where(f => CanFileDataBeAddedtoList(f)).ToList();
                if (filesInfo != null)
                {
                    _filesInfo?.AddRange(filesInfo);
                    UpdateFileData(new FilesChange(_filesInfo, filesInfo, ActionType.Add));
                }
            }
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
                    _filesInfo?.AddRange(filesInfo);
                    UpdateFileData(new FilesChange(_filesInfo, filesInfo, ActionType.Add));
                }
            }
        }

        /// <summary>
        /// Очистить список файлов
        /// </summary>
        public void ClearFiles()
        {
            _filesInfo?.Clear();
            UpdateFileData(new FilesChange(_filesInfo, new List<FileData>(), ActionType.Clear));
        }

        /// <summary>
        /// Удалить файлы
        /// </summary>
        public void RemoveFiles(IEnumerable<FileData> files)
        {
            if (files != null)
            {
                _filesInfo?.RemoveAll(f => files?.Contains(f) == true);
                UpdateFileData(new FilesChange(_filesInfo, files, ActionType.Remove));
            }
        }

        /// <summary>
        /// Измененить статус файла и присвоить при необходимости ошибку
        /// </summary>
        public void ChangeFilesStatusAndMarkError(FilesStatus filesStatus)
        {
            if (filesStatus.IsValid)
            {
                //меняем статус проекта
                bool isStatusProjectChanged = filesStatus.StatusProcessingProject != StatusProcessingProject;
                StatusProcessingProject = filesStatus.StatusProcessingProject;

                //список файлов для изменений c откорректированным статусом
                var filesDataChanged = filesStatus?.FileStatus.
                                       Select(fileStatus =>
                                       {
                                           var fileData = _filesInfo?.FirstOrDefault(file => file.FilePath == fileStatus.FilePath);
                                           fileData.ChangeByFileStatus(fileStatus);
                                           return fileData;
                                       });

                //формируем данные для отправки изменений
                var fileChange = new FilesChange(_filesInfo,
                                                 filesDataChanged,
                                                 ActionType.StatusChange,
                                                 filesStatus.IsConvertingChanged,
                                                 isStatusProjectChanged);
                UpdateFileData(fileChange);
            }
        }

        /// <summary>
        /// Измененить статус всех файлов и присвоить ошибку
        /// </summary>
        public void ChangeAllFilesStatusAndMarkError()
        {
            var filesStatus = new FilesStatus(_filesInfo?.
                                              Select(fileData => new FileStatus(fileData.FilePath,
                                                                                StatusProcessing.Error)),
                                              StatusProcessingProject.Error,
                                              isConvertingChanged: true);

            ChangeFilesStatusAndMarkError(filesStatus);
        }

        /// <summary>
        /// Обновленить список файлов
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
                   _filesInfo?.Any(f => f.Equals(file)) == false;
        }
    }
}
