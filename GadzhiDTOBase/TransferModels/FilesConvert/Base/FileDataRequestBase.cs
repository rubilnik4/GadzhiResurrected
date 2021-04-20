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
        /// <summary>
        /// Путь файла
        /// </summary>
        [DataMember]
        public string FilePath { get; set; }

        /// <summary>
        /// Цвет печати
        /// </summary>
        [DataMember]
        public ColorPrintType ColorPrintType { get; set; }

        /// <summary>
        /// Статус обработки файла
        /// </summary>
        [DataMember]
        public StatusProcessing StatusProcessing { get; set; }

        /// <summary>
        /// Файл данных в формате zip GZipStream
        /// </summary>
        [DataMember]
        public byte[] FileDataSource { get; set; }

        #region IFormattable Support
        public override string ToString() => ToString(String.Empty, CultureInfo.CurrentCulture);

        public string ToString(string format, IFormatProvider formatProvider) => FilePath;
        #endregion
    }
}
