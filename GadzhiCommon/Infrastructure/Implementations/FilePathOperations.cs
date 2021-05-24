using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Extensions.StringAdditional;
using GadzhiCommon.Infrastructure.Interfaces;

namespace GadzhiCommon.Infrastructure.Implementations
{
    /// <summary>
    /// Операции с путями файлов
    /// </summary>
    public class FilePathOperations: IFilePathOperations
    {
        /// <summary>
        /// Существует ли папка
        /// </summary>     
        public bool IsDirectoryExist(string directoryPath) => 
            Directory.Exists(directoryPath);

        /// <summary>
        /// Существует ли файл
        /// </summary>      
        public bool IsFileExist(string filePath) => 
            File.Exists(filePath);

        /// <summary>
        /// Размер файла
        /// </summary>
        public long GetFileSize(string filePath) =>
            new FileInfo(filePath).Length;

        /// <summary>
        /// Получить вложенные папки
        /// </summary>        
        public IEnumerable<string> GetDirectories(string directoryPath) => 
            Directory.GetDirectories(directoryPath);

        /// <summary>
        /// Получить вложенные файлы
        /// </summary>       
        public IEnumerable<string> GetFiles(string filePath) =>
            Directory.GetFiles(filePath);

        /// <summary>
        /// Поиск файлов на один уровень ниже и в текущих папках. Допустимо передавать пути файлов для дальнейшего объединения      
        /// </summary>    
        public IEnumerable<string> GetFilesFromPaths(IEnumerable<string> fileOrDirectoriesPaths) =>
            fileOrDirectoriesPaths.
            Map(paths => paths?.ToList() ?? new List<string>()).
            Map(paths => GetFilesFromDirectory(paths).
                         Union(paths.Where(IsFile)));

        /// <summary>
        /// Получить пути файлов из папок
        /// </summary>
        public IEnumerable<string> GetFilesFromDirectory(IEnumerable<string> directoriesPaths) =>
            directoriesPaths?.
            Where(directoryPath => IsDirectory(directoryPath) && IsDirectoryExist(directoryPath)).
            ToList().
            Map(directoriesPath => directoriesPath.Union(directoriesPath.SelectMany(GetDirectories))).
            SelectMany(GetFiles)
            ?? Enumerable.Empty<string>();

        /// <summary>
        /// Изменить имя в пути файла без изменения расширения
        /// </summary>
        public static string ChangeFilePathNameWithoutExtension(string filePath, string fileName) =>
            Path.GetDirectoryName(filePath) + Path.DirectorySeparatorChar +
            fileName + Path.GetExtension(filePath);

        /// <summary>
        /// Изменить имя в пути файла с изменением расширения
        /// </summary>
        public static string ChangeFileName(string filePath, string fileName, string extension) =>
            Path.GetDirectoryName(filePath) + Path.DirectorySeparatorChar +
            fileName + "." + extension;

        /// <summary>
        /// Убрать точку из расширения файла и привести к нижнему регистру
        /// </summary>      
        public static string ExtensionWithoutPoint(string extension) =>
            extension?.ToLowerCaseCurrentCulture().TrimStart('.');

        /// <summary>
        /// Взять расширение. Убрать точку из расширения файла и привести к нижнему регистру
        /// </summary>      
        public static string ExtensionWithoutPointFromPath(string path) =>
           Path.GetExtension(path).
           Map(ExtensionWithoutPoint);

        /// <summary>
        /// Заменить недопустимые символы в имени файла
        /// </summary>
        public static string GetValidFileName(string fileName)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + ".";
            var regexName = new Regex($"[{Regex.Escape(regexSearch)}]");
            return regexName.Replace(fileName, "_");
        }

        /// <summary>
        /// Заменить недопустимые символы в пути файла
        /// </summary>
        public static string GetValidFilePath(string filePath)
        {
            var regexSearch = new string(Path.GetInvalidPathChars());
            var r = new Regex($"[{Regex.Escape(regexSearch)}]");
            return r.Replace(filePath, "_");
        }

        /// <summary>
        /// Является ли путь папкой
        /// </summary>    
        public static bool IsDirectory(string directoryPath) => !String.IsNullOrEmpty(directoryPath) &&
                                                                String.IsNullOrEmpty(Path.GetExtension(directoryPath));

        /// <summary>
        /// Является ли путь файлом
        /// </summary>       
        public static bool IsFile(string filePath) => !String.IsNullOrEmpty(filePath) &&
                                                      !String.IsNullOrEmpty(Path.GetExtension(filePath));

        /// <summary>
        /// Получить полное имя файла по директории, имени и расширению
        /// </summary>       
        public static string CombineFilePath(string directoryPath, string fileNameWithoutExtension, string extension) =>
             directoryPath.AddSlashesToPath() +
             fileNameWithoutExtension + "." +
             extension?.TrimStart('.');
    }
}