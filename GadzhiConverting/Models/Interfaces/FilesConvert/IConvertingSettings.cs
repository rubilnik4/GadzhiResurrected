using GadzhiCommon.Enums.ConvertingSettings;
using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiCommon.Models.Interfaces.LibraryData;
using GadzhiConverting.Models.Interfaces.Printers;

namespace GadzhiConverting.Models.Interfaces.FilesConvert
{
    /// <summary>
    /// Параметры конвертации
    /// </summary>
    public interface IConvertingSettings
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
        ConvertingModeType ConvertingModeType { get; }
        /// <summary>
        /// Использовать подпись по умолчанию
        /// </summary>
        bool UseDefaultSignature { get; }

        /// <summary>
        /// Информация о принтерах
        /// </summary>
        IResultValue<IPrintersInformation> PrintersInformation { get; }
    }
}