using GadzhiModules.Helpers;
using GadzhiModules.Modules.FilesConvertModule.Model.ReactiveSubjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Modules.FilesConvertModule.Model
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах
    /// </summary>
    public class FilesData
    {
        private List<FileData> _files;

        public FilesData()
        {
            _files = new List<FileData>();
            FileDataChange = new Subject<FileChange>();
        }

        /// <summary>
        /// Подписка на изменение коллекции
        /// </summary>
        public ISubject<FileChange> FileDataChange { get; }

        /// <summary>
        /// Данные о конвертируемых файлах
        /// </summary>
        public IReadOnlyList<FileData> Files => _files;

        /// <summary>
        /// Добавить файл
        /// </summary>
        public void AddFile(FileData file)
        {
            _files?.Add(file);
            UpdateFileData(new FileChange(new List<FileData>() { file },
                                          ActionType.Add));
        }

        /// <summary>
        /// Добавить файлы
        /// </summary>
        public void AddFiles(IEnumerable<FileData> files)
        {
            _files?.AddRange(files);
            UpdateFileData(new FileChange(files, ActionType.Add));
        }

        /// <summary>
        /// Добавить файлы
        /// </summary>
        public void AddFiles(IEnumerable<string> files)
        {
            var filesInfo = files?.Where(f => File.Exists(f))
                  .Select(f => new FileData(FileHelpers.ExtensionWithoutPoint(Path.GetExtension(f)),
                                            Path.GetFileNameWithoutExtension(f), f));

            if (filesInfo != null)
            {
                _files?.AddRange(filesInfo);
            }

            UpdateFileData(new FileChange(filesInfo, ActionType.Add));
        }

        /// <summary>
        /// Очистить список файлов
        /// </summary>
        public void ClearFiles()
        {
            _files?.Clear();
            UpdateFileData(new FileChange(new List<FileData>(), ActionType.Clear));
        }

        /// <summary>
        /// Удалить файлы
        /// </summary>
        public void RemoveFiles(IEnumerable<FileData> files)
        {
            _files?.RemoveAll(f => files?.Contains(f) == true);
            UpdateFileData(new FileChange(files, ActionType.Remove));
        }

        /// <summary>
        /// Обновленить списка файлов
        /// </summary>
        private void UpdateFileData(FileChange fileChange)
        {
            FileDataChange.OnNext(fileChange);
        }
    }
}
