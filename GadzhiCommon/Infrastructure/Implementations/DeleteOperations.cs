using System;
using System.IO;
using System.Linq;
using GadzhiCommon.Extensions.Functional.Result;
using GadzhiCommon.Models.Interfaces.Errors;

namespace GadzhiCommon.Infrastructure.Implementations
{
    /// <summary>
    /// Операции удаления
    /// </summary>
    public static class DeleteOperations
    {
        /// <summary>
        /// Удалить файлы
        /// </summary>
        public static IResultCollection<string> DeleteFiles(DirectoryInfo directoryInfo, DateTime timeNow, int hoursElapsed = -1) =>
             directoryInfo.EnumerateFiles().
                Where(fileInfo => (timeNow - fileInfo.LastWriteTime).Ticks >= hoursElapsed).
                Select(fileInfo => ExecuteResultHandler.ExecuteBindResultValue(() => { fileInfo.Delete(); return fileInfo.FullName; })).
                ToResultCollection();

        /// <summary>
        /// Удалить папки
        /// </summary>
        public static IResultCollection<string> DeleteDirectories(DirectoryInfo directoryInfo, DateTime timeNow, int hoursElapsed = -1) =>
           directoryInfo.EnumerateDirectories().
           Where(directory => (timeNow - directory.LastWriteTime).Ticks >= hoursElapsed).
           Select(directory => ExecuteResultHandler.ExecuteBindResultValue(() => { directory.Delete(true); return directory.FullName; })).
           ToResultCollection();

        /// <summary>
        /// Проверка, используется ли файл
        /// </summary>
        private static IResultError IsFileLocked(FileInfo file) =>
            ExecuteResultHandler.ExecuteBindResultValue(() =>
            {
                using var stream = file?.Open(FileMode.Open, FileAccess.Read, FileShare.None);
                return file;
            }).ToResult();
    }
}