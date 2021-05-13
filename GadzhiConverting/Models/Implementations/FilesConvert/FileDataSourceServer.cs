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
            : this(filePathServer, filePathClient, new List<string> { "-" }, "-")
        { }

        public FileDataSourceServer(string filePathServer, string filePathClient, string paperSize)
           : this(filePathServer, filePathClient, new List<string> { paperSize }, "-")
        { }

        public FileDataSourceServer(string filePathServer, string filePathClient, IEnumerable<string> paperSizes)
       : this(filePathServer, filePathClient, paperSizes, "-")
        { }

        public FileDataSourceServer(string filePathServer, string filePathClient, string paperSize, string printerName)
          : this(filePathServer, filePathClient, new List<string> { paperSize }, printerName)
        { }

        public FileDataSourceServer(string filePathServer, string filePathClient, IEnumerable<string> paperSizes, string printerName)
            : base(filePathServer, filePathClient)
        {
            if (String.IsNullOrWhiteSpace(printerName)) throw new ArgumentNullException(nameof(printerName));

            PaperSizes = paperSizes.ToList();
            PrinterName = printerName;
        }

        /// <summary>
        /// Формат печати
        /// </summary>
        public IReadOnlyCollection<string> PaperSizes { get; }

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
