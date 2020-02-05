using GadzhiCommon.Enums.FilesConvert;
using GadzhiCommon.Infrastructure.Interfaces;
using GadzhiDTO.TransferModels.FilesConvert;
using GadzhiWcfHost.Infrastructure.Implementations;
using GadzhiWcfHost.Infrastructure.Implementations.Information;
using GadzhiWcfHost.Models.FilesConvert.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GadzhiWcfHost.Infrastructure.Interfaces.Converters
{
    /// <summary>
    /// Конвертер из серверной модели в трансферную
    /// </summary>
    public interface IConverterServerFilesDataToDTO
    {
        /// <summary>
        /// Конвертировать серверную модель в промежуточную
        /// </summary>       
        FilesDataIntermediateResponse ConvertFilesToIntermediateResponse(FilesDataServer filesDataServer);
        
        /// <summary>
        /// Конвертировать серверную модель в окончательный ответ
        /// </summary>          
        Task<FilesDataResponse> ConvertFilesToResponse(FilesDataServer filesDataServer);       
    }
}