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
            if (file != null)
            {
                if (File.Exists(file.FilePath))
                {
                    _files?.Add(file);
                    UpdateFileData(new FileChange(new List<FileData>() { file },
                                                  ActionType.Add));
                }
            }
            else
            {
                throw new ArgumentNullException("Подано пустое значение FileData в AddFile(FileData file)");
            }
        }

        /// <summary>
        /// Добавить файлы
        /// </summary>
        public void AddFiles(IEnumerable<FileData> files)
        {
            if (files != null)
            {
                if (files.All(f => f != null))
                {
                    var filesInfo = files?.Where(f => File.Exists(f.FilePath));
                    if (filesInfo != null)
                    {
                        _files?.AddRange(filesInfo);
                        UpdateFileData(new FileChange(files, ActionType.Add));
                    }
                }
                else
                {
                    throw new ArgumentNullException("Подано пустое значение FileData в AddFiles(IEnumerable<FileData> files)");
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
                var filesInfo = files?.Where(f => File.Exists(f))
                  .Select(f => new FileData(FileHelpers.ExtensionWithoutPoint(Path.GetExtension(f)),
                                            Path.GetFileNameWithoutExtension(f), f));

                if (filesInfo != null)
                {
                    _files?.AddRange(filesInfo);
                }

                UpdateFileData(new FileChange(filesInfo, ActionType.Add));
            }
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
            if (files != null)
            {
                _files?.RemoveAll(f => files?.Contains(f) == true);
                UpdateFileData(new FileChange(files, ActionType.Remove));
            }
        }

        /// <summary>
        /// Обновленить список файлов
        /// </summary>
        private void UpdateFileData(FileChange fileChange)
        {
            FileDataChange?.OnNext(fileChange);
        }
    }
}
