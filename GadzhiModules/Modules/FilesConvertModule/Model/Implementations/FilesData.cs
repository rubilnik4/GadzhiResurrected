using GadzhiCommon.Enums.FilesConvert;
using GadzhiModules.Helpers;
using GadzhiModules.Modules.FilesConvertModule.Model.Implementations.ReactiveSubjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Modules.FilesConvertModule.Model.Implementations
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах
    /// </summary>
    public class FilesData : IFilesData
    {
        private List<FileData> _filesInfo;

        public FilesData()
            : this(new List<FileData>())
        {

        }

        public FilesData(List<FileData> files)
        {
            _filesInfo = files;
            FileDataChange = new Subject<FileChange>();
        }

        /// <summary>
        /// Подписка на изменение коллекции
        /// </summary>
        public ISubject<FileChange> FileDataChange { get; }

        /// <summary>
        /// Данные о конвертируемых файлах
        /// </summary>
        public IReadOnlyList<FileData> FilesInfo => _filesInfo;

        /// <summary>
        /// Пути конвертируемых файлов
        /// </summary>
        public IEnumerable<string> FilesInfoPath => _filesInfo.Select(file => file.FilePath);

        /// <summary>
        /// Добавить файл
        /// </summary>
        public void AddFile(FileData file)
        {
            if (CanFileDataBeAddedtoList(file))
            {
                _filesInfo?.Add(file);
                UpdateFileData(new FileChange(_filesInfo,
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
                var filesInfo = files.Where(f => CanFileDataBeAddedtoList(f));
                if (filesInfo != null)
                {
                    _filesInfo?.AddRange(filesInfo);
                    UpdateFileData(new FileChange(_filesInfo, filesInfo, ActionType.Add));
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
                var filesInfo = files?.Select(f => new FileData(f)).
                                       Where(f => CanFileDataBeAddedtoList(f));
                if (filesInfo != null)
                {
                    _filesInfo?.AddRange(filesInfo);
                    UpdateFileData(new FileChange(_filesInfo, filesInfo, ActionType.Add));
                }
            }
        }

        /// <summary>
        /// Очистить список файлов
        /// </summary>
        public void ClearFiles()
        {
            _filesInfo?.Clear();
            UpdateFileData(new FileChange(_filesInfo, new List<FileData>(), ActionType.Clear));
        }

        /// <summary>
        /// Удалить файлы
        /// </summary>
        public void RemoveFiles(IEnumerable<FileData> files)
        {
            if (files != null)
            {
                _filesInfo?.RemoveAll(f => files?.Contains(f) == true);
                UpdateFileData(new FileChange(_filesInfo, files, ActionType.Remove));
            }
        }


        /// <summary>
        /// Измененить статус файла и присвоить при необходимости ошибку
        /// </summary>
        public void ChangeFilesStatusAndMarkError(IEnumerable<FileStatus> filesStatus)
        {           
            if (filesStatus != null)
            {
                //список файлов для изменений
                var filesStatusChanged = _filesInfo?.
                                          Select(file => new
                                          {
                                              File = file,
                                              FileStatus = filesStatus?.FirstOrDefault(fileStatus => fileStatus.FilePath == file.FilePath)
                                          }).Where
                                          (fileComplex => fileComplex.FileStatus != null);
              
                //меняем статус
                foreach (var fileStatus in filesStatusChanged)
                {
                    fileStatus.File.ChangeByFileStatus(fileStatus.FileStatus);
                }

                UpdateFileData(new FileChange(_filesInfo,
                                              filesStatusChanged.Select(file => file.File), 
                                              ActionType.StatusChange));               
            }           
        }

        /// <summary>
        /// Обновленить список файлов
        /// </summary>
        private void UpdateFileData(FileChange fileChange)
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
