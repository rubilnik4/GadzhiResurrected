using GadzhiModules.Modules.FilesConvertModule.Model.Enums;
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
    /// Класс содержащий данные о конвертируемых файлах
    /// </summary>
    [MessageContract]
    public class FileDataRequest
    {
        /// <summary>
        /// Расширение файла
        /// </summary>
        [MessageHeader]
        public string FileType { get; set; }

        /// <summary>
        /// Имя файла
        /// </summary>
        [MessageHeader]
        public string FileName { get; set; }

        /// <summary>
        /// Путь файла
        /// </summary>
        [MessageHeader]
        public string FilePath { get; set; }

        /// <summary>
        /// Цвет печати
        /// </summary>
        [MessageHeader]
        public ColorPrint ColorPrint { get; set; }

        /// <summary>
        /// Статус обработки файла
        /// </summary>
        [MessageHeader]
        public StatusProcessing StatusProcessing { get; set; }

        /// <summary>
        /// Статус обработки файла
        /// </summary>
        [MessageBodyMember]
        public Stream FileData { get; set; }
    }
}
