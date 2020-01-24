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
        private List<FileData> _filesData;

        public FilesData()
            : this(new List<FileData>())
        {

        }

        public FilesData(List<FileData> files)
        {
            _filesData = files;
            FileDataChange = new Subject<FileChange>();
        }

        /// <summary>
        /// Подписка на изменение коллекции
        /// </summary>
        public ISubject<FileChange> FileDataChange { get; private set; }

        /// <summary>
        /// Данные о конвертируемых файлах
        /// </summary>
        public IReadOnlyList<FileData> Files => _filesData;

        /// <summary>
        /// Добавить файл
        /// </summary>
        public void AddFile(FileData file)
        {
            if (CanFileDataBeAddedtoList(file))
            {
                _filesData?.Add(file);
                UpdateFileData(new FileChange(_filesData,
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
                    _filesData?.AddRange(files);
                    UpdateFileData(new FileChange(_filesData, files, ActionType.Add));
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
                    _filesData?.AddRange(filesInfo);
                    UpdateFileData(new FileChange(_filesData, filesInfo, ActionType.Add));
                }
            }
        }

        /// <summary>
        /// Очистить список файлов
        /// </summary>
        public void ClearFiles()
        {
            _filesData?.Clear();
            UpdateFileData(new FileChange(_filesData, new List<FileData>(), ActionType.Clear));
        }

        /// <summary>
        /// Удалить файлы
        /// </summary>
        public void RemoveFiles(IEnumerable<FileData> files)
        {
            if (files != null)
            {
                _filesData?.RemoveAll(f => files?.Contains(f) == true);
                UpdateFileData(new FileChange(_filesData, files, ActionType.Remove));
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
            if (file == null)
            {
                throw new ArgumentNullException("Пустое значение FileData в AddFile(FileData file)");
            }
            return file != null &&
                   _filesData?.Contains(file) == false;
        }
    }
}
