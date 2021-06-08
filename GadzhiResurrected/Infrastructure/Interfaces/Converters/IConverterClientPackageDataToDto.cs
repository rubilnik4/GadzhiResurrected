using System.Threading.Tasks;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiDTOClient.TransferModels.FilesConvert;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Interfaces.FileConverting;
using GadzhiResurrected.Modules.GadzhiConvertingModule.Models.Interfaces.ProjectSettings;

namespace GadzhiResurrected.Infrastructure.Interfaces.Converters
{
    /// <summary>
    /// Конвертеры из локальной модели в трансферную
    /// </summary>  
    public interface IConverterClientPackageDataToDto
    {
        /// <summary>
        /// Конвертер пакета информации о файле из локальной модели в трансферную
        /// </summary>      
        Task<IResultValue<PackageDataRequestClient>> ToPackageDataRequest(IPackageData packageData, IConvertingSettings convertingSetting);
    }
}
