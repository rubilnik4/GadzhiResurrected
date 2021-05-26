using System.Runtime.Serialization;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Interfaces.Histories;

namespace GadzhiDTOBase.TransferModels.Histories
{
    /// <summary>
    /// Данные истории конвертации ресурсов файла
    /// </summary>
    [DataContract]
    public class HistoryFileDataSourceResponse : IHistoryFileDataSource
    {
        public HistoryFileDataSourceResponse(FileExtensionType fileExtensionType, string printerName, string paperSize)
        {
            FileExtensionType = fileExtensionType;
            PrinterName = printerName;
            PaperSize = paperSize;
        }

        /// <summary>
        /// Типы допустимых расширений
        /// </summary>
        [DataMember]
        public FileExtensionType FileExtensionType { get; private set; }

        /// <summary>
        /// Принтер
        /// </summary>
        [DataMember]
        public string PrinterName { get; private set; }

        /// <summary>
        /// Формат
        /// </summary>
        [DataMember]
        public string PaperSize { get; private set; }
    }
}