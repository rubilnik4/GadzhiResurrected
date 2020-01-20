using GadzhiModules.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Modules.FilesConvertModule.Model
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах
    /// </summary>
    public class FilesData
    {
        public event EventHandler FilesDataUpdatedEvent;

        private List<FileData> _files;

        public FilesData()
        {
            _files = new List<FileData>();
        }

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
            UpdateFileData();
        }

        /// <summary>
        /// Добавить файлы
        /// </summary>
        public void AddFiles(IEnumerable<FileData> files)
        {
            _files?.AddRange(files);
            UpdateFileData();
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

            UpdateFileData();
        }

        /// <summary>
        /// Очистить список файлов
        /// </summary>
        public void ClearFiles()
        {
            _files?.Clear();
            UpdateFileData();
        }

        /// <summary>
        /// Удалить файлы
        /// </summary>
        public void RemoveFiles(IEnumerable<FileData> files)
        {
            _files?.RemoveAll(f => files?.Contains(f) == true);
            UpdateFileData();
        }

        private void UpdateFileData()
        {
            FilesDataUpdatedEvent?.Invoke(this, new EventArgs());
        }
    }
}
