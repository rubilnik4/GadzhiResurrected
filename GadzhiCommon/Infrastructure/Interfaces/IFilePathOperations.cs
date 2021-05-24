using System.Collections.Generic;

namespace GadzhiCommon.Infrastructure.Interfaces
{
    /// <summary>
    /// Операции с путями файлов
    /// </summary>
    public interface IFilePathOperations
    {
        /// <summary>
        /// Существует ли папка
        /// </summary>       
        bool IsDirectoryExist(string directoryPath);

        /// <summary>
        /// Существует ли файл
        /// </summary>       
        bool IsFileExist(string filePath);

        /// <summary>
        /// Размер файла
        /// </summary>
        long GetFileSize(string filePath);

        /// <summary>
        /// Получить вложенные папки
        /// </summary>        
        IEnumerable<string> GetDirectories(string directoryPath);

        /// <summary>
        /// Получить вложенные файлы
        /// </summary>        
        IEnumerable<string> GetFiles(string filePath);

        /// <summary>
        /// Поиск файлов на один уровень ниже и в текущей папке       
        /// </summary>    
        IEnumerable<string> GetFilesFromPaths(IEnumerable<string> fileOrDirectoriesPaths);

        /// <summary>
        /// Получить пути файлов из папок
        /// </summary>
        public IEnumerable<string> GetFilesFromDirectory(IEnumerable<string> directoriesPaths);
    }
}