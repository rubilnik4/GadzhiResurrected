using GadzhiModules.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Infrastructure.Implementations
{
    /// <summary>
    /// Проверка состояния папок и файлов
    /// </summary>
    public class FileSeach : IFileSeach
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
        /// Получить вложенные файлы
        /// </summary>       
        public IEnumerable<string> GetFiles(string filePath) => Directory.GetFiles(filePath);

        /// <summary>
        /// Представить файл в двоичном виде
        /// </summary>   
        public async Task<byte[]> ConvertFileToByteAndZip(string fileName, string filePath)
        {
            byte[] result = null;

            // Create an archive and store the stream in memory.
            //using (var compressedFileStream = new MemoryStream())
            //using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Create, false))
            //{
            //    //Create a zip entry for each attachment
            //    var zipEntry = zipArchive.CreateEntry(fileName);

            //    //Get the stream of the attachment
            //    using (var fileStream = new FileStream(filePath, FileMode.Open))
            //    using (var zipEntryStream = zipEntry.Open())
            //    {
            //        //Copy the attachment stream to the zip entry stream
            //        await fileStream.CopyToAsync(zipEntryStream);
            //    }
            //    result = compressedFileStream.ToArray();
            //}

            using (FileStream SourceStream = File.Open(filePath, FileMode.Open))
            {
                result = new byte[SourceStream.Length];
                await SourceStream.ReadAsync(result, 0, (int)SourceStream.Length);
            }


            //using (FileStream stream = File.Open(filePath, FileMode.Open))
            //{
            //    using (var compressionStream = new GZipStream(stream, CompressionMode.Compress))
            //    {
            //        result = new byte[stream.Length];
            //        await stream.ReadAsync(result, 0, (int)stream.Length);
            //        await compressionStream.WriteAsync(result, 0, result.Length);
            //    }
            //}
            return result;
        }
    }
}
