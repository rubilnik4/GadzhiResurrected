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
        /// Подпись
        /// </summary>
        ISignatureLibrary PersonSignature { get; set; }
    }
}