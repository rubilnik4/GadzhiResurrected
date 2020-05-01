using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Entities.FilesConvert.Main;
using GadzhiDTOClient.TransferModels.FilesConvert;
using System.Threading.Tasks;

namespace GadzhiDAL.Infrastructure.Interfaces.Converters.Client
{
    /// <summary>
    /// Конвертер из трансферной модели в модель базы данных
    /// </summary>      
    public interface IConverterDataAccessFilesDataFromDTOClient
    {
        /// <summary>
        /// Конвертер пакета информации из трансферной модели в модель базы данных
        /// </summary>      
        FilesDataEntity ConvertToFilesDataAccess(PackageDataRequestClient packageDataRequest);
    }
}
