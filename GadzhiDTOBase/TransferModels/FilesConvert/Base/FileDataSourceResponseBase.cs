using System.Collections.Generic;
using System.Runtime.Serialization;
using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiDTOBase.TransferModels.FilesConvert.Base
{
    /// <summary>
    /// Информация об отконвертированных файлах
    /// </summary>
    [DataContract]
    public abstract class FileDataSourceResponseBase
    {
        /// <summary>
        /// Путь файла
        /// </summary>
        [DataMember]
        public string FileName { get; set; }

        /// <summary>
        /// Тип расширения
        /// </summary>
        [DataMember]
        public FileExtensionType FileExtensionType { get; set; }

        /// <summary>
        /// Формат печати
        /// </summary>
        [DataMember]
        public IList<string> PaperSizes { get; set; }

        /// <summary>
        /// Имя принтера
        /// </summary>
        [DataMember]
        public string PrinterName { get; set; }

        /// <summary>
        /// Файл данных в формате zip GZipStream
        /// </summary>
        [DataMember]
        public byte[] FileDataSource { get; set; }
    }
}
