using GadzhiCommon.Extentions.Collection;
using GadzhiCommon.Extentions.Functional;
using GadzhiCommon.Extentions.StringAdditional;
using GadzhiCommon.Helpers.Dialogs;
using GadzhiCommon.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

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
            extension?.ToLower(CultureInfo.CurrentCulture).TrimStart('.');

        /// <summary>
        /// Взять расширение. Убрать точку из расширения файла и привести к нижнему регистру
        /// </summary>      
        public static string ExtensionWithoutPointFromPath(string path) =>
           Path.GetExtension(path).
           Map(extension => ExtensionWithoutPoint(extension));

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
        public IEnumerable<string> GetFilesFromDirectoryAndSubDirectory(IEnumerable<string> fileOrDirectoriesPaths) =>
            fileOrDirectoriesPaths?.
            Where(directoryPath => IsDirectory(directoryPath) && IsDirectoryExist(directoryPath)).
            Map(directoriesPath => directoriesPath.UnionNotNull(
                                   directoriesPath?.SelectMany(d => GetDirectories(d)))).
            SelectMany(directoryPath => GetFiles(directoryPath)).
            Map(filesPath => filesPath.UnionNotNull(
                             fileOrDirectoriesPaths?.Where(f => IsFile(f))));

        /// <summary>
        /// Удалить всю информацию из папки
        /// </summary>      
        public void DeleteAllDataInDirectory(string directoryPath)
        {
            if (!String.IsNullOrWhiteSpace(directoryPath) && Directory.Exists(directoryPath))
            {
                var directoryInfo = new DirectoryInfo(directoryPath);
                foreach (FileInfo file in directoryInfo.EnumerateFiles())
                {
                    if(!IsFileLocked(file))
                    {
                        file.Delete();
                    }                  
                }
                foreach (DirectoryInfo dir in directoryInfo.EnumerateDirectories())
                {
                    try
                    {
                        dir.Delete(true);
                    }
                    catch (IOException)
                    {

                    }
                }
            }
        }
     
        /// <summary>
        /// Представить файл в двоичном виде и запаковать
        /// </summary>   
        public async Task<byte[]> ConvertFileToByteAndZip(string filePath)
        {
            byte[] result = null;

            //продолжаем процесс не смотря на ошибку. Файлы с ошибкой не войдут в отправку
            try
            {
                using (FileStream input = File.Open(filePath, FileMode.Open))
                using (MemoryStream output = new MemoryStream())
                {
                    using (GZipStream zip = new GZipStream(output, CompressionMode.Compress))
                    {
                        await input.CopyToAsync(zip);
                    }
                    result = output.ToArray();
                }
            }
            finally
            { }
            return result;
        }

        /// <summary>
        /// Распаковать файл из двоичного вида и сохранить
        /// </summary>   
        public async Task<bool> UnzipFileAndSave(string filePath, IList<byte> fileBinary)
        {
            bool success = false;

            //продолжаем процесс не смотря на ошибку. Файлы с ошибкой не будут конвертированы
            try
            {
                if (fileBinary != null)
                {
                    using (MemoryStream input = new MemoryStream(fileBinary.ToArray()))
                    using (GZipStream zip = new GZipStream(input, CompressionMode.Decompress))
                    using (FileStream output = File.Create(filePath))
                    {
                        await zip.CopyToAsync(output);
                    }

                    success = true;
                }
            }
            finally
            { }
            return success;
        }

        /// <summary>
        /// Распаковать файл из двоичного вида и сохранить
        /// </summary>   
        public async Task<bool> SaveFileFromByte(string filePath, byte[] fileByte)
        {
            bool success = false;

            //продолжаем процесс не смотря на ошибку. Файлы с ошибкой не будут конвертированы
            try
            {
                if (!String.IsNullOrEmpty(filePath) && fileByte != null)
                {
                    using (FileStream sourceStream = File.Open(filePath, FileMode.OpenOrCreate))
                    {
                        sourceStream.Seek(0, SeekOrigin.End);
                        await sourceStream.WriteAsync(fileByte, 0, fileByte.Length);
                    }

                    success = true;
                }
            }
            finally
            { }
            return success;
        }

        /// <summary>
        /// Создать поддиректорию и присвоить идентефикатор
        /// </summary>     
        public string CreateFolderByGuid(string startingPath) => CreateFolderByName(startingPath, Guid.NewGuid().ToString());

        /// <summary>
        /// Создать поддиректорию
        /// </summary>     
        public string CreateFolderByName(string startingPath, string folderName = "") =>
             (startingPath.AddSlashesToPath() + folderName.AddSlashesToPath())?.
             Map(createdPath => Directory.CreateDirectory(createdPath))?.
             FullName;

        /// <summary>
        /// Проверка, используется ли файл
        /// </summary>
        private bool IsFileLocked(FileInfo file)
        {
            try
            {
                using (FileStream stream = file?.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                return true;
            }
            return false;
        }


    }
}
