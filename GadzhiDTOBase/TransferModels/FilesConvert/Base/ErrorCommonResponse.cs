using System.Runtime.Serialization;
using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiDTOBase.TransferModels.FilesConvert.Base
{
    /// <summary>
    /// Ошибка конвертации
    /// </summary>
    [DataContract]
    public class ErrorCommonResponse
    {
        public ErrorCommonResponse(ErrorConvertingType errorConvertingType, string errorDescription)
        {
            ErrorConvertingType = errorConvertingType;
            ErrorDescription = errorDescription;
        }

        /// <summary>
        /// Тип ошибки при конвертации файлов
        /// </summary>
        [DataMember]
        public ErrorConvertingType ErrorConvertingType { get; private set; }

        /// <summary>
        /// Описание ошибки
        /// </summary>
        [DataMember]
        public string ErrorDescription { get; private set; }
    }
}