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
    /// Класс содержащий промежуточные данные о конвертируемых файлах
    /// <summary>
    public class FilesDataIntermediateResponse
    {
        /// <summary>
        /// Завершена ли обработка
        /// </summary>
        [DataMember]
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Статус выполнения проекта
        /// </summary>
        [DataMember]
        public StatusProcessingProject StatusProcessingProject { get; set; }

        /// <summary>
        /// Промежуточные данные о конвертируемых файлах
        /// </summary>
        [DataMember]
        public IEnumerable<FileDataIntermediateResponse> FilesData { get; set; }
    }
}
