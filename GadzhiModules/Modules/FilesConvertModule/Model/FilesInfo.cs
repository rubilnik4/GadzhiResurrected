using GadzhiModules.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Modules.FilesConvertModule.Model
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах
    /// </summary>
    public class FilesInfo
    {
        /// <summary>
        /// Данные о конвертируемых файлах
        /// </summary>
        public List<FileInfo> Files { get; private set; } = new List<FileInfo>();

        /// <summary>
        /// Добавить файл
        /// </summary>
        public void AddFile(FileInfo file)
        {
            Files?.Add(file);
        }

        /// <summary>
        /// Добавить файлы
        /// </summary>
        public void AddFiles(IEnumerable<FileInfo> files)
        {
            Files?.AddRange(files);
        }

        /// <summary>
        /// Добавить файлы
        /// </summary>
        public void AddFiles(IEnumerable<string> files)
        {
            var filesInfo = files?.Where(f => File.Exists(f))
                  .Select(f => new FileInfo(FileHelpers.ExtensionWithoutPoint(Path.GetExtension(f)), 
                                            Path.GetFileNameWithoutExtension(f), f));

            if (filesInfo != null)
            {
                Files?.AddRange(filesInfo);
            }
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
        public void RemoveFiles(IEnumerable<FileInfo> files)
        {
            Files?.RemoveAll(f => files?.Contains(f) == true);
        }
    }
}
