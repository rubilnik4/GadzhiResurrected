using GadzhiDAL.Entities.FilesConvert;
using GadzhiDTOServer.TransferModels.FilesConvert;

namespace GadzhiDAL.Infrastructure.Interfaces.Converters.Server
{
    /// <summary>
    /// Конвертер из трансферной модели в модель базы данных
    /// </summary>      
    public interface IConverterDataAccessFilesDataToDTOServer
    {
        /// <summary>
        /// Конвертировать из модели базы данных в запрос
        /// </summary>          
        FilesDataRequestServer ConvertFilesDataAccessToRequest(FilesDataEntity filesDataEntity);
    }
}
