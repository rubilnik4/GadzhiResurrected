using GadzhiCommon.Enums.ConvertingSettings;
using GadzhiCommon.Models.Interfaces.LibraryData;

namespace GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.ConvertingSettings
{
    /// <summary>
    /// Параметры конвертации, отображение
    /// </summary>
    public interface IConvertingSettings
    {
        /// <summary>
        /// Отдел
        /// </summary>
        string Department { get; set; }

        /// <summary>
        /// Личная подпись
        /// </summary>
        ISignatureLibrary PersonSignature { get; set; }

        /// <summary>
        /// Принцип именования PDF
        /// </summary>
        PdfNamingType PdfNamingType { get; set; }
    }
}