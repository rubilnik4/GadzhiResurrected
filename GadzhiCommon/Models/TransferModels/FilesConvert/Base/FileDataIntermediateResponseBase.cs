using GadzhiCommon.Enums.FilesConvert;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GadzhiCommon.Models.TransferModels.FilesConvert.Base
{
    /// <summary>
    /// Класс содержащий промежуточные данные о конвертируемом файле
    /// <summary>
    [DataContract]
    public abstract class FileDataIntermediateResponseBase
    {
        public FileDataIntermediateResponseBase()
        {
            FileConvertErrorType = new List<FileConvertErrorType>();
        }

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
        /// Завершена ли обработка файла
        /// </summary>
        [DataMember]
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Тип ошибки при конвертации файла
        /// </summary>
        [DataMember]
        public IEnumerable<FileConvertErrorType> FileConvertErrorType { get; set; }
    }
}
