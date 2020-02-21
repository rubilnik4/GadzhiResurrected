using GadzhiDAL.Entities.FilesConvert;
using GadzhiDTOClient.TransferModels.FilesConvert;

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
        FilesDataEntity ConvertToFilesDataAccess(FilesDataRequestClient filesDataRequest);
    }
}
