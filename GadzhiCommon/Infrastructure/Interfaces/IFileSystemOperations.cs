using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GadzhiCommon.Models.Interfaces.Errors;

namespace GadzhiCommon.Infrastructure.Interfaces
{
    /// <summary>
    /// Проверка состояния папок и файлов
    /// </summary>
    public interface IFileSystemOperations
    {
        /// <summary>
        /// Удалить всю информацию из папки
        /// </summary>      
        IResultCollection<string> DeleteAllDataInDirectory(string directoryPath, DateTime timeNow, int hoursElapsed = -1);

        /// <summary>
        /// Удалить файл
        /// </summary>
        IResultValue<string> DeleteFile(string filePath);

        /// <summary>
        /// Представить файл в двоичном виде
        /// </summary>        
        Task<IResultValue<byte[]>> FileToByteAndZip(string filePath);

        /// <summary>
        /// Распаковать, сохранить файл и вернуть его путь
        /// </summary>
        Task<IResultValue<string>> UnzipFileAndSave(string filepath, byte[] fileByte);

        /// <summary>
        /// Сохранить файл на диск из двоичного кода
        /// </summary>   
        Task<IResultValue<string>> SaveFileFromByte(string filePath, byte[] fileByte);

        /// <summary>
        /// Получить файл в виде двоичного кода
        /// </summary>   
        Task<byte[]> GetFileFromPath(string filePath);

        /// <summary>
        /// Копировать файл
        /// </summary>   
        IResultValue<string> CopyFile(string fileSource, string filePath);

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
