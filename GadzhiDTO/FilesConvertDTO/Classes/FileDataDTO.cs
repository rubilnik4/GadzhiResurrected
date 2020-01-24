using GadzhiModules.Modules.FilesConvertModule.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiDTO.FilesConvertDTO.Classes
{
    /// <summary>
    /// Класс содержащий данные о конвертируемых файлах
    /// </summary>
    [DataContract]
    public class FileDataDTO
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
    }
}
