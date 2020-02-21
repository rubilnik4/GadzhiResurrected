using GadzhiDTOClient.TransferModels.FilesConvert;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations;
using System.Threading.Tasks;

namespace GadzhiModules.Infrastructure.Interfaces.Converters
{
    /// <summary>
    /// Конвертеры из локальной модели в трансферную
    /// </summary>  
    public interface IConverterClientFilesDataToDTO
    {
        /// <summary>
        /// Конвертер пакета информации о файле из локальной модели в трансферную
        /// </summary>      
        Task<FilesDataRequestClient> ConvertToFilesDataRequest(IFilesData filesData);
    }
}
