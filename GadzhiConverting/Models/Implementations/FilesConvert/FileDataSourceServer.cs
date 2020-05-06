using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Implementations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiConverting.Models.Interfaces.FilesConvert;

namespace GadzhiConverting.Models.Implementations.FilesConvert
{
    /// <summary>
    /// Обработанный файл серверной части в базовом варианте
    /// </summary>
    public class FileDataSourceServer : IFileDataSourceServer
    {
        public FileDataSourceServer(string filePath)
            : this(filePath, "-", "-")
        { }

        public FileDataSourceServer(string filePath, string paperSize, string printerName)
        {
            if (String.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));
            string fileType = FileSystemOperations.ExtensionWithoutPointFromPath(filePath);
            if (!ValidFileExtensions.ContainsInFileTypesValid(fileType))
            {
                throw new KeyNotFoundException(nameof(filePath));
            }

            FilePath = filePath;
            FileExtension = ValidFileExtensions.GetFileTypesValid(fileType);
            PaperSize = paperSize;
            PrinterName = printerName;
        }

        /// <summary>
        /// Путь файла
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// Тип расширения файла
        /// </summary>
        public FileExtension FileExtension { get; }

        /// <summary>
        /// Формат печати
        /// </summary>
        public string PaperSize { get; }

        /// <summary>
        /// Имя принтера
        /// </summary>
        public string PrinterName { get; }

        /// <summary>
        /// Реализация перечисления
        /// </summary>  
        public IEnumerator<IFileDataSourceServer> GetEnumerator()
        {
            yield return this;
        }

        /// <summary>
        /// Реализация перечисления
        /// </summary>  
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
