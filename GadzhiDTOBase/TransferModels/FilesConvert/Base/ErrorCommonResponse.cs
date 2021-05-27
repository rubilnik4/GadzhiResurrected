using System.Runtime.Serialization;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Interfaces.Errors;

namespace GadzhiDTOBase.TransferModels.FilesConvert.Base
{
    /// <summary>
    /// Ошибка конвертации
    /// </summary>
    [DataContract]
    public class ErrorCommonResponse: IError
    {
        public ErrorCommonResponse(ErrorConvertingType errorConvertingType, string description)
        {
            ErrorConvertingType = errorConvertingType;
            Description = description;
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
        public string Description { get; private set; }
    }
}