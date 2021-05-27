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
            : this(filePathServer, filePathClient, convertingModeType, StampPaperSizeType.Undefined, "-")
        { }

        public FileDataSourceServer(string filePathServer, string filePathClient, 
                                    ConvertingModeType convertingModeType, StampPaperSizeType paperSize)
       : this(filePathServer, filePathClient, convertingModeType, paperSize, "-")
        { }

        public FileDataSourceServer(string filePathServer, string filePathClient, ConvertingModeType convertingModeType,
                                    StampPaperSizeType paperSize, string printerName)
            : base(filePathServer, filePathClient)
        {
            PaperSize = paperSize;
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
        public StampPaperSizeType PaperSize { get; }

        /// <summary>
        /// Имя принтера
        /// </summary>
        public string PrinterName { get; }

        #region IEnumerable
        public IEnumerator<IFileDataSourceServer> GetEnumerator()
        {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion
    }
}
