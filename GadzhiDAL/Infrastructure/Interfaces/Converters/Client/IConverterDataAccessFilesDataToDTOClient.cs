using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Entities.FilesConvert.Main;
using GadzhiDAL.Models.Implementations;
using GadzhiDTOClient.TransferModels.FilesConvert;
using System.Threading.Tasks;

namespace GadzhiDAL.Infrastructure.Interfaces.Converters.Client
{
    /// <summary>
    /// Конвертер из модели базы данных в трансферную для клиентской части
    /// </summary>
    public interface IConverterDataAccessFilesDataToDTOClient
    {
        /// <summary>
        /// Конвертировать из модели базы данных в промежуточную
        /// </summary>       
        Task<FilesDataIntermediateResponseClient> ConvertFilesDataAccessToIntermediateResponse(FilesDataEntity filesDataEntity,
                                                                                         FilesQueueInfo filesQueueInfo);

        /// <summary>
        /// Конвертировать из модели базы данных в основной ответ
        /// </summary>          
        Task<FilesDataResponseClient> ConvertFilesDataAccessToResponse(FilesDataEntity filesDataEntity);
    }
}
