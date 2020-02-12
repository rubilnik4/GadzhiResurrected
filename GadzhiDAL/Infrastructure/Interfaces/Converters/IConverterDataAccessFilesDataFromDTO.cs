using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Helpers;
using GadzhiCommon.Helpers.FileSystem;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiDAL.Entities.FilesConvert;
using GadzhiDTO.TransferModels.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiDAL.Infrastructure.Interfaces.Converters
{
    /// <summary>
    /// Конвертер из трансферной модели в модель базы данных
    /// </summary>      
    public interface IConverterDataAccessFilesDataFromDTO
    {
        /// <summary>
        /// Конвертер пакета информации из трансферной модели в модель базы данных
        /// </summary>      
        FilesDataEntity ConvertToFilesDataAccess(FilesDataRequest filesDataRequest);

        /// <summary>
        /// Обновить модель базы данных на основе промежуточного ответа
        /// </summary>      
        FilesDataEntity UpdateFilesDataAccessFromIntermediateResponse(FilesDataEntity filesDataEntity,
                                                                      FilesDataIntermediateResponse filesDataIntermediateResponse);

        /// <summary>
        /// Обновить модель базы данных на основе окончательного ответа
        /// </summary>      
        FilesDataEntity UpdateFilesDataAccessFromResponse(FilesDataEntity filesDataEntity,
                                                          FilesDataResponse filesDataResponse);
    }
}
