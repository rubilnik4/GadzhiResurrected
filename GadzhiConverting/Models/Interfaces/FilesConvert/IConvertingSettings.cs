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
    public interface IConvertingSettings: IConvertingPackageSettings
    {
        /// <summary>
        /// Информация о принтерах
        /// </summary>
        IResultValue<IPrintersInformation> PrintersInformation { get; }
    }
}