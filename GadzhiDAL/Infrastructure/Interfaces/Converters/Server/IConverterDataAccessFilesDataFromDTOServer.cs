using GadzhiDAL.Entities.FilesConvert;
using GadzhiDAL.Entities.FilesConvert.Main;
using GadzhiDTOServer.TransferModels.FilesConvert;
using System.Threading.Tasks;

namespace GadzhiDAL.Infrastructure.Interfaces.Converters.Server
{
    /// <summary>
    /// Конвертер из трансферной модели в модель базы данных
    /// </summary>      
    public interface IConverterDataAccessFilesDataFromDtoServer
    {
        /// <summary>
        /// Обновить модель базы данных на основе промежуточного ответа
        /// </summary>      
        FilesDataEntity UpdateFilesDataAccessFromIntermediateResponse(FilesDataEntity filesDataEntity,
                                                                      PackageDataIntermediateResponseServer packageDataIntermediateResponse);

        /// <summary>
        /// Обновить модель базы данных на основе окончательного ответа
        /// </summary>      
        FilesDataEntity UpdateFilesDataAccessFromResponse(FilesDataEntity filesDataEntity,
                                                          PackageDataResponseServer packageDataResponse);
    }
}
