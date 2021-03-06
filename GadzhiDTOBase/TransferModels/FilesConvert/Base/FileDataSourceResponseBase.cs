﻿using System.Collections.Generic;
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
        protected FileDataSourceResponseBase(string fileName, FileExtensionType fileExtensionType, string paperSize,
                                             string printerName, byte[] fileDataSource)
        {
            FileName = fileName;
            FileExtensionType = fileExtensionType;
            PaperSize = paperSize;
            PrinterName = printerName;
            FileDataSource = fileDataSource;
        }

        /// <summary>
        /// Путь файла
        /// </summary>
        [DataMember]
        public string FileName { get; private set; }

        /// <summary>
        /// Тип расширения
        /// </summary>
        [DataMember]
        public FileExtensionType FileExtensionType { get; private set; }

        /// <summary>
        /// Формат печати
        /// </summary>
        [DataMember]
        public string PaperSize { get; private set; }

        /// <summary>
        /// Имя принтера
        /// </summary>
        [DataMember]
        public string PrinterName { get; private set; }

        /// <summary>
        /// Файл данных в формате zip GZipStream
        /// </summary>
        [DataMember]
        public byte[] FileDataSource { get; private set; }
    }
}
