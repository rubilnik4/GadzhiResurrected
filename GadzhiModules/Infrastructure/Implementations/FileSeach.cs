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
          
            using (FileStream input = File.Open(filePath, FileMode.Open))
            using (MemoryStream output = new MemoryStream())
            {
                using (GZipStream zip = new GZipStream(output, CompressionMode.Compress))
                {
                    await input.CopyToAsync(zip);
                }
                result = output.ToArray();
            }

            return result;
        }
    }
}
