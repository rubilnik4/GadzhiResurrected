using GadzhiCommon.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Extensions.Functional;
using GadzhiCommon.Extensions.Functional.Result;
using GadzhiCommon.Extensions.StringAdditional;
using GadzhiCommon.Models.Implementations.Errors;
using GadzhiCommon.Models.Interfaces.Errors;

namespace GadzhiCommon.Infrastructure.Implementations
{
    /// <summary>
    /// Проверка состояния папок и файлов, архивация, сохранение
    /// </summary>
    public class FileSystemOperations : IFileSystemOperations
    {
        /// <summary>
        /// Удалить всю информацию из папки
        /// </summary>      
        public void DeleteAllDataInDirectory(string directoryPath, DateTime timeNow, int hoursElapsed = -1)
        {
            if (String.IsNullOrWhiteSpace(directoryPath) || !Directory.Exists(directoryPath)) return;
            var directoryInfo = new DirectoryInfo(directoryPath);
            foreach (var fileInfo in directoryInfo.EnumerateFiles())
            {
                if ((timeNow - fileInfo.LastWriteTime).Ticks >= hoursElapsed && !IsFileLocked(fileInfo).OkStatus)
                {
                    fileInfo.Delete();
                }
            }
            foreach (var dir in directoryInfo.EnumerateDirectories())
            {
                if ((timeNow - dir.LastWriteTime).Ticks >= hoursElapsed)
                {
                    dir.Delete(true);
                }
            }
        }

        /// <summary>
        /// Удалить файл
        /// </summary>
        public void DeleteFile(string filePath) =>
            File.Delete(filePath);

        /// <summary>
        /// Представить файл в двоичном виде и запаковать
        /// </summary>
        public async Task<IResultValue<byte[]>> FileToByteAndZip(string filePath) =>
            await ExecuteTaskResultHandler.ExecuteBindResultValueAsync(() => FileToByteZip(filePath));

        /// <summary>
        /// Распаковать файл из двоичного вида и сохранить
        /// </summary>
        public async Task<IResultValue<string>> UnzipFileAndSave(string filePath, byte[] fileByte) =>
            await filePath.WhereContinueAsyncBind(_ => !String.IsNullOrEmpty(filePath) && fileByte != null,
                okFunc: _ => ExecuteTaskResultHandler.ExecuteBindResultValueAsync(() => FileFromByteZip(filePath, fileByte)),
                badFunc: _ => new ResultValue<string>(new ErrorCommon(ErrorConvertingType.FileNotSaved, $"Невозможно сохранить файл {filePath}")).
                              Map(result => Task.FromResult((IResultValue<string>)result)));

        /// <summary>
        /// Сохранить файл на диск из двоичного кода
        /// </summary>   
        public async Task<IResultValue<string>> SaveFileFromByte(string filePath, byte[] fileByte) =>
            await ExecuteTaskResultHandler.ExecuteBindResultValueAsync(() => FileToByte(filePath, fileByte));

        /// <summary>
        /// Получить файл в виде двоичного кода
        /// </summary>   
        public async Task<byte[]> GetFileFromPath(string filePath)
        {
            using var input = File.Open(filePath, FileMode.Open);
            using var output = new MemoryStream();
            await input.CopyToAsync(output);
            return output.ToArray();
        }

        /// <summary>
        /// Копировать файл
        /// </summary>   
        public IResultValue<string> CopyFile(string fileSource, string fileDestination) =>
            fileSource.WhereContinue(_ => !String.IsNullOrEmpty(fileSource) && !String.IsNullOrEmpty(fileDestination),
                okFunc: _ => ExecuteResultHandler.ExecuteBindResultValue(() =>
                        {
                            File.Copy(fileSource, fileDestination, true);
                            return fileDestination;
                        }),
                badFunc: _ => new ResultValue<string>(new ErrorCommon(ErrorConvertingType.FileNotSaved,
                                                                      $"Невозможно скопировать файл {fileSource}")));

        /// <summary>
        /// Создать поддиректорию и присвоить идентификатор
        /// </summary>     
        public string CreateFolderByGuid(string startingPath) => 
            CreateFolderByName(startingPath, Guid.NewGuid().ToString());

        /// <summary>
        /// Создать поддиректорию
        /// </summary>     
        public string CreateFolderByName(string startingPath, string folderName = "") =>
             (startingPath.AddSlashesToPath() + folderName.AddSlashesToPath()).
             Map(Directory.CreateDirectory)?.
             FullName;

        /// <summary>
        /// Запаковать файл
        /// </summary>
        public static async Task<byte[]> FileToByteZip(string filePath)
        {
            using var input = File.Open(filePath, FileMode.Open);
            using var output = new MemoryStream();
            using (var zip = new GZipStream(output, CompressionMode.Compress))
            {
                await input.CopyToAsync(zip);
            }
            return output.ToArray();
        }

        /// <summary>
        /// Распаковать файл
        /// </summary>
        private static async Task<string> FileFromByteZip(string filePath, IList<byte> fileByte)
        {
            using var input = new MemoryStream(fileByte.ToArray());
            using var zip = new GZipStream(input, CompressionMode.Decompress);
            using var output = File.Create(filePath);
            await zip.CopyToAsync(output);
            return filePath;
        }

        /// <summary>
        ///  Преобразовать файл в байтовый массив
        /// </summary>
        private static async Task<string> FileToByte(string filePath, byte[] fileByte)
        {
            using var sourceStream = File.Create(filePath);
            sourceStream.Seek(0, SeekOrigin.End);
            await sourceStream.WriteAsync(fileByte, 0, fileByte.Length);
            return filePath;
        }

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
