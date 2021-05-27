using GadzhiDTOClient.TransferModels.FilesConvert;
using System.Threading.Tasks;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.FileConverting;
using GadzhiModules.Modules.GadzhiConvertingModule.Models.Interfaces.ProjectSettings;

namespace GadzhiModules.Infrastructure.Interfaces.Converters
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
