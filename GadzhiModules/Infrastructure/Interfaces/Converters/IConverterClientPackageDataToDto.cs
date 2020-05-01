using GadzhiDTOClient.TransferModels.FilesConvert;
using System.Threading.Tasks;
using GadzhiModules.Modules.FilesConvertModule.Models.Interfaces;

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
        Task<PackageDataRequestClient> ToPackageDataRequest(IPackageData packageData);
    }
}
