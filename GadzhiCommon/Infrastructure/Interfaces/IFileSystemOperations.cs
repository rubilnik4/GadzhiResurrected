using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiCommon.Infrastructure.Interfaces
{
    /// <summary>
    /// Проверка состояния папок и файлов
    /// </summary>
    public interface IFileSystemOperations
    {
        /// <summary>
        /// Является ли путь папкой
        /// </summary>       
        bool IsDirectory(string directoryPath);

        /// <summary>
        /// Является ли путь файлом
        /// </summary>       
        bool IsFile(string filePath);

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
        /// Получить полное имя файла по директории, имени и расширению
        /// </summary>       
        string CreateFilePath(string directoryPath, string fileNameWithoutExtension, string extension);

        /// <summary>
        /// Представить файл в двоичном виде
        /// </summary>        
        Task<byte[]> ConvertFileToByteAndZip(string fileName, string filePath);

        /// <summary>
        /// Распаковать файл из двоичного вида и сохранить
        /// </summary>   
        Task<bool> UnzipFileAndSave(string filePath, byte[] fileBinary);

        /// <summary>
        /// Создать поддиректорию и пррисвоить идентефикатор
        /// </summary>     
        (bool isCreated, string path) CreateFolderByGuid(string startingPath);
    }
}
