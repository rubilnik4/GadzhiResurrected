using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiDAL.Entities.FilesConvert;
using GadzhiDTO.TransferModels.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GadzhiDAL.Infrastructure.Interfaces.Converters
{
    /// <summary>
    /// Конвертер из модели базы данных в трансферную
    /// </summary>
    public interface IConverterDataAccessFilesDataToDTO
    {
        /// <summary>
        /// Конвертировать из модели базы данных в промежуточную
        /// </summary>       
        FilesDataIntermediateResponse ConvertFilesDataAccessToIntermediateResponse(FilesDataEntity filesDataEntity);
       
    }
}