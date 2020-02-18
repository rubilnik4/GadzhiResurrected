using GadzhiMicrostation.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Infrastructure.Implementations
{
    /// <summary>
    /// Проверка состояния папок и файлов, архивация, сохранение
    /// </summary>
    public class FileSystemOperationsMicrostation : IFileSystemOperationsMicrostation
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
        public string CombineFilePath(string directoryPath, string fileNameWithoutExtension, string extension)
        {
            extension = extension.TrimStart('.');

            return directoryPath + fileNameWithoutExtension + "." + extension;
        }

        /// <summary>
        /// Получить вложенные файлы
        /// </summary>       
        public IEnumerable<string> GetFiles(string filePath) => Directory.GetFiles(filePath);

        /// <summary>
        /// Создать поддиректорию
        /// </summary>     
        public string CreateFolderByName(string startingPath, string folderName)
        {           
            if (!startingPath.EndsWith("\\"))
            {
                startingPath += "\\";
            }
            string createdPath = startingPath + folderName + "\\";

            if (!String.IsNullOrEmpty(startingPath))
            {
                Directory.CreateDirectory(createdPath);               
            }

            return createdPath;
        }
    }
}
