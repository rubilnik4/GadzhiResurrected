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
    [DataContract]
    public class FileDataRequest
    {
        /// <summary>
        /// Расширение файла
        /// </summary>
        [DataMember]
        public string FileType { get; set; }

        /// <summary>
        /// Имя файла
        /// </summary>
        [DataMember]
        public string FileName { get; set; }

        /// <summary>
        /// Путь файла
        /// </summary>
        [DataMember]
        public string FilePath { get; set; }

        /// <summary>
        /// Цвет печати
        /// </summary>
        [DataMember]
        public ColorPrint ColorPrint { get; set; }

        /// <summary>
        /// Статус обработки файла
        /// </summary>
        [DataMember]
        public StatusProcessing StatusProcessing { get; set; }

        /// <summary>
        /// Файл данных в формате zip
        /// </summary>
        [DataMember]
        public IEnumerable<byte> FileDataSource { get; set; }
    }
}
