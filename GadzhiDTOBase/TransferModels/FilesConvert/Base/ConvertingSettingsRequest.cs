using System.Runtime.Serialization;

namespace GadzhiDTOBase.TransferModels.FilesConvert.Base
{
    /// <summary>
    /// Параметры конвертации. Трансферная модель
    /// </summary>
    [DataContract]
    public class ConvertingSettingsRequest
    {
        /// <summary>
        /// Отдел
        /// </summary>
        [DataMember]
        public string Department { get; set; }
    }
}
