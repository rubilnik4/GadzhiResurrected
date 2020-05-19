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
    public class FileDataSourceServer : FilePath, IFileDataSourceServer
    {
        public FileDataSourceServer(string filePathServer, string filePathClient)
            : this(filePathServer, filePathClient, "-", "-")
        { }

        public FileDataSourceServer(string filePathServer, string filePathClient, string paperSize, string printerName)
            : base(filePathServer, filePathClient)
        {
            if (String.IsNullOrWhiteSpace(paperSize)) throw new ArgumentNullException(nameof(paperSize));
            if (String.IsNullOrWhiteSpace(printerName)) throw new ArgumentNullException(nameof(printerName));

            PaperSize = paperSize;
            PrinterName = printerName;
        }

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
