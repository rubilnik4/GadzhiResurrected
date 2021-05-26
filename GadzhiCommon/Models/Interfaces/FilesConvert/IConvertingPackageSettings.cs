using System.Collections.Generic;
using GadzhiCommon.Enums.ConvertingSettings;
using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiCommon.Models.Interfaces.FilesConvert
{
    public interface IConvertingPackageSettings
    {
        /// <summary>
        /// Идентификатор личной подпись
        /// </summary>
        string PersonId { get; }

        /// <summary>
        /// Принцип именование PDF
        /// </summary>
        PdfNamingType PdfNamingType { get; }

        /// <summary>
        /// Тип конвертации
        /// </summary>
        IReadOnlyCollection<ConvertingModeType> ConvertingModeTypes { get; }

        /// <summary>
        /// Использовать подпись по умолчанию
        /// </summary>
        bool UseDefaultSignature { get; }
    }
}