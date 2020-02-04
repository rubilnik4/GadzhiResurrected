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
        public FileDataIntermediateResponse()
        {
            StatusProcessing = StatusProcessing.NotSend;           
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
        /// Тип ошибки при конвертации файла
        /// </summary>
        [DataMember]
        public IEnumerable<FileConvertErrorType> FileConvertErrorType { get; set; }
    }
}
