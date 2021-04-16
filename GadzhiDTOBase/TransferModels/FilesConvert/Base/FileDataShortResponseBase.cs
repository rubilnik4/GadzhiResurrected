using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Interfaces.Errors;

namespace GadzhiDTOBase.TransferModels.FilesConvert.Base
{
    /// <summary>
    /// Класс содержащий промежуточные данные о конвертируемом файле
    /// </summary>
    [DataContract]
    public abstract class FileDataShortResponseBase : IFormattable
    {       
        /// <summary>
        /// Путь файла
        /// </summary>
        [DataMember]
        public string FilePath { get; set; }

        /// <summary>
        /// Статус обработки файла
        /// </summary>
        [DataMember]
        public StatusProcessing StatusProcessing { get; set; }        

        /// <summary>
        /// Тип ошибки при конвертации файла
        /// </summary>
        [DataMember]
        public IList<ErrorCommonResponse> FileErrors { get; set; }

        #region IFormattable Support
        public override string ToString() => ToString(String.Empty, CultureInfo.CurrentCulture);

        public string ToString(string format, IFormatProvider formatProvider) => FilePath;
        #endregion
    }
}
