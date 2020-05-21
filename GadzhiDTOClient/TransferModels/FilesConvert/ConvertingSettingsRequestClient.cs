using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiDTOClient.TransferModels.FilesConvert
{
    /// <summary>
    /// Параметры конвертации. Трансферная модель
    /// </summary>
    [DataContract]
    public class ConvertingSettingsRequestClient
    {
        /// <summary>
        /// Отдел
        /// </summary>
        [DataMember]
        public string Department { get; set; }
    }
}
