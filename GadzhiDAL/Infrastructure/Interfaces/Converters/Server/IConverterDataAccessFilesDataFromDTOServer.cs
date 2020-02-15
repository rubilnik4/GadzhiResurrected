using GadzhiDAL.Entities.FilesConvert;
using GadzhiDTOServer.TransferModels.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiDAL.Infrastructure.Interfaces.Converters.Server
{
    /// <summary>
    /// Конвертер из трансферной модели в модель базы данных
    /// </summary>      
    public interface IConverterDataAccessFilesDataFromDTOServer
    {       
        /// <summary>
        /// Обновить модель базы данных на основе промежуточного ответа
        /// </summary>      
        FilesDataEntity UpdateFilesDataAccessFromIntermediateResponse(FilesDataEntity filesDataEntity,
                                                                      FilesDataIntermediateResponseServer filesDataIntermediateResponse);

        /// <summary>
        /// Обновить модель базы данных на основе окончательного ответа
        /// </summary>      
        FilesDataEntity UpdateFilesDataAccessFromResponse(FilesDataEntity filesDataEntity,
                                                          FilesDataResponseServer filesDataResponse);
    }
}
