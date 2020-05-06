﻿using GadzhiCommon.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using GadzhiCommon.Extensions.Collection;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Extensions.StringAdditional;

namespace GadzhiCommon.Infrastructure.Implementations
{
    /// <summary>
    /// Проверка состояния папок и файлов, архивация, сохранение
    /// </summary>
    public class FileSystemOperations : IFileSystemOperations
    {
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

        /// <summary>
        /// Существует ли папка
        /// </summary>     
        public bool IsDirectoryExist(string directoryPath) => Directory.Exists(directoryPath);

        /// <summary>
        /// Существует ли файл
        /// </summary>      
        public bool IsFileExist(string filePath) => File.Exists(filePath);

        /// <summary>
        /// Получить вложенные папки
        /// </summary>        
        public IEnumerable<string> GetDirectories(string directoryPath) => Directory.GetDirectories(directoryPath);

        /// <summary>
        /// Получить вложенные файлы
        /// </summary>       
        public IEnumerable<string> GetFiles(string filePath) => Directory.GetFiles(filePath);

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
            directoriesPaths ?? Enumerable.Empty<string>().
            Where(directoryPath => IsDirectory(directoryPath) && IsDirectoryExist(directoryPath)).
            ToList().
            Map(directoriesPath => directoriesPath.Union(directoriesPath.SelectMany(GetDirectories))).
            SelectMany(GetFiles);

        /// <summary>
        /// Удалить всю информацию из папки
        /// </summary>      
        public void DeleteAllDataInDirectory(string directoryPath)
        {
            if (String.IsNullOrWhiteSpace(directoryPath) || !Directory.Exists(directoryPath)) return;

            var directoryInfo = new DirectoryInfo(directoryPath);
            foreach (var file in directoryInfo.EnumerateFiles())
            {
                if (!IsFileLocked(file))
                {
                    file.Delete();
                }
            }
            foreach (var dir in directoryInfo.EnumerateDirectories())
            {
                try
                {
                    dir.Delete(true);
                }
                catch (IOException)
                { }
            }
        }

        /// <summary>
        /// Представить файл в двоичном виде и запаковать
        /// </summary>
        public async Task<(bool Success, byte[] Zip)> FileToByteAndZip(string filePath)
        {
            try
            {
                //using var input = File.Open(filePath, FileMode.Open);
                //using var output = new MemoryStream();
                //using var zip = new GZipStream(output, CompressionMode.Compress);
                //await input.CopyToAsync(zip);

                //return (true, output.ToArray());
                using var input = File.Open(filePath, FileMode.Open);
                using var output = new MemoryStream();
                using (var zip = new GZipStream(output, CompressionMode.Compress))
                {
                    await input.CopyToAsync(zip);
                }
                return (true, output.ToArray());
            }
            catch (IOException)
            { }

            return (false, null);
        }

        /// <summary>
        /// Распаковать файл из двоичного вида и сохранить
        /// </summary>
        public async Task<bool> UnzipFileAndSave(string filePath, IList<byte> fileByte)
        {
            if (String.IsNullOrEmpty(filePath) || fileByte == null) return false;

            try
            {
                using var input = new MemoryStream(fileByte.ToArray());
                using var zip = new GZipStream(input, CompressionMode.Decompress);
                using var output = File.Create(filePath);
                await zip.CopyToAsync(output);

                return true;
            }
            catch (IOException)
            { }

            return false;
        }

        /// <summary>
        /// Распаковать файл из двоичного вида и сохранить
        /// </summary>   
        public async Task<bool> SaveFileFromByte(string filePath, byte[] fileByte)
        {
            if (String.IsNullOrEmpty(filePath) || fileByte == null) return false;

            try
            {
                using var sourceStream = File.Open(filePath, FileMode.OpenOrCreate);
                sourceStream.Seek(0, SeekOrigin.End);
                await sourceStream.WriteAsync(fileByte, 0, fileByte.Length);

                return true;
            }
            catch (IOException)
            { }

            return false;
        }

        /// <summary>
        /// Копировать файл
        /// </summary>   
        public bool CopyFile(string fileSource, string fileDestination)
        {
            if (String.IsNullOrWhiteSpace(fileDestination)) return false;
            if (!IsFileExist(fileSource)) return false;

            try
            {
                File.Copy(fileSource, fileDestination, true);
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        /// <summary>
        /// Создать поддиректорию и присвоить идентификатор
        /// </summary>     
        public string CreateFolderByGuid(string startingPath) => CreateFolderByName(startingPath, Guid.NewGuid().ToString());

        /// <summary>
        /// Создать поддиректорию
        /// </summary>     
        public string CreateFolderByName(string startingPath, string folderName = "") =>
             (startingPath.AddSlashesToPath() + folderName.AddSlashesToPath()).
             Map(Directory.CreateDirectory)?.
             FullName;

        /// <summary>
        /// Проверка, используется ли файл
        /// </summary>
        private static bool IsFileLocked(FileInfo file)
        {
            try
            {
                using var stream = file?.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            return false;
        }
    }
}
