using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiDTOBase.TransferModels.FilesConvert.Base
{
    /// <summary>
    /// Базовый класс содержащий данные о конвертируемом файле
    /// </summary>
    [DataContract]
    public abstract class FileDataRequestBase: IFormattable
    {
        protected FileDataRequestBase(string filePath, ColorPrintType colorPrintType, StatusProcessing statusProcessing,
                                      byte[] fileDataSource, string fileExtensionAdditional, byte[] fileDataSourceAdditional)
        {
            FilePath = filePath;
            ColorPrintType = colorPrintType;
            StatusProcessing = statusProcessing;
            FileDataSource = fileDataSource;
            FileExtensionAdditional = fileExtensionAdditional;
            FileDataSourceAdditional = fileDataSourceAdditional;
        }

        /// <summary>
        /// Путь файла
        /// </summary>
        [DataMember]
        public string FilePath { get; private set; }

        /// <summary>
        /// Цвет печати
        /// </summary>
        [DataMember]
        public ColorPrintType ColorPrintType { get; private set; }

        /// <summary>
        /// Статус обработки файла
        /// </summary>
        [DataMember]
        public StatusProcessing StatusProcessing { get; private set; }

        /// <summary>
        /// Файл данных в формате zip GZipStream
        /// </summary>
        [DataMember]
        public byte[] FileDataSource { get; private set; }

        /// <summary>
        /// Тип расширения дополнительного файла
        /// </summary>
        [DataMember]
        public string FileExtensionAdditional { get; private set; }

        /// <summary>
        /// Файл данных в формате zip GZipStream
        /// </summary>
        [DataMember]
        public byte[] FileDataSourceAdditional { get; private set; }

        #region IFormattable Support
        public override string ToString() => ToString(String.Empty, CultureInfo.CurrentCulture);

        public string ToString(string format, IFormatProvider formatProvider) => FilePath;
        #endregion
    }
}
