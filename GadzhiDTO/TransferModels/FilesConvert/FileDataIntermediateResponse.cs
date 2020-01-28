using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiDTO.TransferModels.FilesConvert
{
    /// <summary>
    /// Класс содержащий промежуточные данные о конвертируемом файле
    /// <summary>
    [DataContract]
    public class FileDataIntermediateResponse
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
        public StatusProcessing StatusProcessing { get; set; } = StatusProcessing.NotSend;

        /// <summary>
        /// Тип ошибки при конвертации файла
        /// </summary>
        public FileConvertErrorType FileConvertErrorType { get; } = FileConvertErrorType.NoError;
    }
}
