using System.Collections.Generic;
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
        /// Поиск файлов на один уровень ниже и в текущей папке       
        /// </summary>    
        Task<IEnumerable<string>> GetFilesFromDirectoryAndSubDirectory(IEnumerable<string> fileOrDirectoriesPaths);

        /// <summary>
        /// Получить полное имя файла по директории, имени и расширению
        /// </summary>       
        string CombineFilePath(string directoryPath, string fileNameWithoutExtension, string extension);

        /// <summary>
        /// Представить файл в двоичном виде
        /// </summary>        
        Task<byte[]> ConvertFileToByteAndZip(string filePath);

        /// <summary>
        /// Распаковать файл из двоичного вида и сохранить
        /// </summary>   
        Task<bool> UnzipFileAndSave(string filePath, IList<byte> fileBinary);

        /// <summary>
        /// Создать поддиректорию и пррисвоить идентефикатор
        /// </summary>     
        string CreateFolderByGuid(string startingPath);

        /// <summary>
        /// Создать поддиректорию
        /// </summary>     
        string CreateFolderByName(string startingPath, string folderName);
    }
}
