using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Entities.FilesConvert.Main;
using GadzhiDTOServer.TransferModels.FilesConvert;
using System.Threading.Tasks;

namespace GadzhiDAL.Infrastructure.Interfaces.Converters.Server
{
    /// <summary>
    /// Конвертер из трансферной модели в модель базы данных
    /// </summary>      
    public interface IConverterDataAccessFilesDataToDtoServer
    {
        /// <summary>
        /// Конвертировать из модели базы данных в запрос
        /// </summary>          
        Task<PackageDataRequestServer> ConvertFilesDataAccessToRequest(FilesDataEntity filesDataEntity);
    }
}
