using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GadzhiApplicationCommon.Models.Enums.StampCollections;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiConverting.Models.Interfaces.FilesConvert;

namespace GadzhiConverting.Models.Implementations.FilesConvert
{
    /// <summary>
    /// Обработанный файл серверной части в базовом варианте
    /// </summary>
    public class FileDataSourceServer : FilePath, IFileDataSourceServer
    {
        public FileDataSourceServer(string filePathServer, string filePathClient, ConvertingModeType convertingModeType)
            : this(filePathServer, filePathClient, convertingModeType, 
                   new List<StampPaperSizeType> {StampPaperSizeType.Undefined}, "-")
        { }

        public FileDataSourceServer(string filePathServer, string filePathClient, ConvertingModeType convertingModeType,
                                    StampPaperSizeType paperSize)
           : this(filePathServer, filePathClient, convertingModeType, new List<StampPaperSizeType> { paperSize }, "-")
        { }

        public FileDataSourceServer(string filePathServer, string filePathClient, 
                                    ConvertingModeType convertingModeType, IEnumerable<StampPaperSizeType> paperSizes)
       : this(filePathServer, filePathClient, convertingModeType, paperSizes, "-")
        { }

        public FileDataSourceServer(string filePathServer, string filePathClient, ConvertingModeType convertingModeType,
                                    StampPaperSizeType paperSize, string printerName)
          : this(filePathServer, filePathClient, convertingModeType, new List<StampPaperSizeType> { paperSize }, printerName)
        { }

        public FileDataSourceServer(string filePathServer, string filePathClient,
                                    ConvertingModeType convertingModeType, IEnumerable<StampPaperSizeType> paperSizes, string printerName)
            : base(filePathServer, filePathClient)
        {
            if (String.IsNullOrWhiteSpace(printerName)) throw new ArgumentNullException(nameof(printerName));

            PaperSizes = paperSizes.Distinct().OrderBy(paperSize => paperSize).ToList();
            PrinterName = printerName;
            ConvertingModeType = convertingModeType;
        }

        /// <summary>
        /// Тип конвертации
        /// </summary>
        public ConvertingModeType ConvertingModeType { get; }

        /// <summary>
        /// Формат печати
        /// </summary>
        public IReadOnlyCollection<StampPaperSizeType> PaperSizes { get; }

        /// <summary>
        /// Имя принтера
        /// </summary>
        public string PrinterName { get; }

        #region IEnumerable
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
        #endregion
    }
}
