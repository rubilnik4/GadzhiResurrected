﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace GadzhiCommon.Infrastructure.Interfaces
{
    /// <summary>
    /// Проверка состояния папок и файлов
    /// </summary>
    public interface IFileSystemOperations
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

        /// <summary>
        /// Удалить всю информацию из папки
        /// </summary>      
        void DeleteAllDataInDirectory(string directoryPath);      

        /// <summary>
        /// Представить файл в двоичном виде
        /// </summary>        
        Task<(bool Success, byte[] Zip)> FileToByteAndZip(string filePath);

        /// <summary>
        /// Распаковать файл из двоичного вида и сохранить
        /// </summary>   
        Task<bool> UnzipFileAndSave(string filePath, IList<byte> fileByte);

        /// <summary>
        /// Распаковать файл из двоичного вида и сохранить
        /// </summary>   
        Task<bool> SaveFileFromByte(string filePath, byte[] fileByte);

        /// <summary>
        /// Копировать файл
        /// </summary>   
        bool CopyFile(string fileSource, string fileDestination);

        /// <summary>
        /// Создать поддиректорию и присвоить идентификатор
        /// </summary>     
        string CreateFolderByGuid(string startingPath);

        /// <summary>
        /// Создать поддиректорию
        /// </summary>     
        string CreateFolderByName(string startingPath, string folderName = "");     
    }
}
