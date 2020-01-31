using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiDTO.TransferModels.FilesConvert
{
    /// <summary>
    /// Класс содержащий данные о конвертируемом файле
    /// </summary>
    [DataContract]
    public class FileDataResponse
    {
        public FileDataResponse()
        {
            FileConvertErrorType = new List<FileConvertErrorType>();
        }

        /// <summary>
        /// Путь файла
        /// </summary>
        [DataMember]
        public string FilePath { get; set; }
        
        /// <summary>
        /// Файл данных в формате zip GZipStream
        /// </summary>
        [DataMember]
        public byte[] FileDataSource { get; set; }

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
