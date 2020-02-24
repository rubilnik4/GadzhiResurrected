using GadzhiMicrostation.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

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
            if (directoryPath?.EndsWith("\\", StringComparison.Ordinal) == false)
            {
                directoryPath += "\\";
            }

            extension = extension?.TrimStart('.');

            return directoryPath + fileNameWithoutExtension + "." + extension;
        }

        /// <summary>
        /// Получить вложенные файлы
        /// </summary>       
        public IEnumerable<string> GetFiles(string filePath) => Directory.GetFiles(filePath);

        /// <summary>
        /// Создать поддиректорию
        /// </summary>     
        public string CreateFolderByName(string startingPath, string folderName = "")
        {
            if (!startingPath?.EndsWith("\\", StringComparison.Ordinal) == true)
            {
                startingPath += "\\";
            }
            if (!String.IsNullOrEmpty(folderName) && !folderName.EndsWith("\\", StringComparison.Ordinal))
            {
                folderName += "\\";
            }
            string createdPath = startingPath + folderName;

            if (!String.IsNullOrEmpty(startingPath))
            {
                Directory.CreateDirectory(createdPath);
            }

            return createdPath;
        }

        /// <summary>
        /// Распаковать файл из двоичного вида и сохранить
        /// </summary>   
        public bool SaveFileFromByte(string filePath, byte[] fileByte)
        {
            bool succsess = false;

            //продолжаем процесс не смотря на ошибку. Файлы с ошибкой не будут конвертированы
            try
            {
                if (!String.IsNullOrEmpty(filePath) && fileByte != null)
                {
                    File.WriteAllBytes(filePath, fileByte);

                    succsess = true;
                }
            }
            finally
            {

            }
            return succsess;
        }
    }
}
