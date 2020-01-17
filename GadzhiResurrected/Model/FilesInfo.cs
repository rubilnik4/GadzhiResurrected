using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gadzhie.Model
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах
    /// </summary>
    public class FilesInfo
    {
        /// <summary>
        /// Данные о конвертируемых файлах
        /// </summary>
        public List<FilesInfo> Files { get; private set; } = new List<FilesInfo>();

        /// <summary>
        /// Добавить файл
        /// </summary>
        public void AddFile(FilesInfo file)
        {
            Files?.Add(file);
        }

        /// <summary>
        /// Добавить файлы
        /// </summary>
        public void AddFiles(IEnumerable<FilesInfo> files)
        {
            Files?.AddRange(files);
        }

        /// <summary>
        /// Очистить список файлов
        /// </summary>
        public void ClearFiles()
        {
            Files?.Clear();
        }

        /// <summary>
        /// Удалить файлы
        /// </summary>
        public void RemoveFiles(IEnumerable<FilesInfo> files)
        {
            Files?.RemoveAll(f => files?.Contains(f) == true);
        }
    }
}
