using GadzhiCommon.Helpers.Dialogs;
using GadzhiCommon.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiCommon.Infrastructure.Implementations
{
    /// <summary>
    /// Проверка состояния папок и файлов, архивация, сохранение
    /// </summary>
    public class FileSystemOperations : IFileSystemOperations
    {

        /// <summary>
        /// Является ли путь папкой
        /// </summary>    
        public bool IsDirectory(string directoryPath) => !String.IsNullOrEmpty(directoryPath) &&
                                                         String.IsNullOrEmpty(Path.GetExtension(directoryPath));

        /// <summary>
        /// Существует ли папка
        /// </summary>     
        public bool IsDirectoryExist(string directoryPath) => Directory.Exists(directoryPath);

        /// <summary>
        /// Является ли путь файлом
        /// </summary>       
        public bool IsFile(string filePath) => !String.IsNullOrEmpty(filePath) &&
                                               !String.IsNullOrEmpty(Path.GetExtension(filePath));

        /// <summary>
        /// Существует ли файл
        /// </summary>      
        public bool IsFileExist(string filePath) => File.Exists(filePath);

        /// <summary>
        /// Получить вложенные папки
        /// </summary>        
        public IEnumerable<string> GetDirectories(string directoryPath) => Directory.GetDirectories(directoryPath);

        /// <summary>
        /// Получить полное имя файла по директории, имени и расширению
        /// </summary>       
        public string CreateFilePath(string directoryPath, string fileNameWithoutExtension, string extension)
        {
            return directoryPath + "//" + fileNameWithoutExtension + "." + extension;
        }

        /// <summary>
        /// Получить вложенные файлы
        /// </summary>       
        public IEnumerable<string> GetFiles(string filePath) => Directory.GetFiles(filePath);

        /// <summary>
        /// Поиск файлов на один уровень ниже и в текущих папках. Допустимо передавать пути файлов для дальнейшего объединения      
        /// </summary>    
        public async Task<IEnumerable<string>> GetFilesFromDirectoryAndSubDirectory(IEnumerable<string> fileOrDirectoriesPaths)
        {
            var filePaths = fileOrDirectoriesPaths?.Where(f => IsFile(f));
            var directoriesPath = fileOrDirectoriesPaths?.Where(d => IsDirectory(d) &&
                                                                     IsDirectoryExist(d));
            var filesInDirectories = directoriesPath?.Union(directoriesPath?.SelectMany(d => GetDirectories(d)))?
                                                     .SelectMany(d => GetFiles(d));
            var allFilePaths = filePaths?.Union(filesInDirectories)?
                                         .Where(f => DialogFilters.IsInDocAndDgnFileTypes(f) &&
                                                     IsFileExist(f));

            return await Task.FromResult(allFilePaths);
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
            catch (Exception)
            {

            }
            return result;
        }

        /// <summary>
        /// Распаковать файл из двоичного вида и сохранить
        /// </summary>   
        public async Task<bool> UnzipFileAndSave(string filePath, byte[] fileBinary)
        {
            bool succsess = false;

            //продолжаем процесс не смотря на ошибку. Файлы с ошибкой не будут конвертированы
            try
            {
                if (fileBinary != null)
                {
                    using (MemoryStream input = new MemoryStream(fileBinary))
                    using (GZipStream zip = new GZipStream(input, CompressionMode.Decompress))
                    using (FileStream output = File.Create(filePath))
                    {
                        await zip.CopyToAsync(output);                       
                    }

                    succsess = true;
                }
            }
            catch (Exception)
            {

            }
            return succsess;
        }

        /// <summary>
        /// Создать поддиректорию и присвоить идентефикатор
        /// </summary>     
        public (bool isCreated, string path) CreateFolderByGuid(string startingPath)
        {
            bool isCreated = false;
            string createdPath = startingPath + Guid.NewGuid() + "\\";

            if (!String.IsNullOrWhiteSpace(startingPath))
            {
                Directory.CreateDirectory(createdPath);
                isCreated = true;
            }

            return (isCreated, createdPath);
        }
    }
}
