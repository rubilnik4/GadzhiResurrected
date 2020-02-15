using GadzhiDTOClient.TransferModels.FilesConvert;
using GadzhiModules.Modules.FilesConvertModule.Models.Implementations.Information;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Infrastructure.Interfaces.Converters
{
    /// <summary>
    /// Конвертеры из трансферной модели в локальную
    /// </summary>  
    public interface IConverterClientFilesDataFromDTO
    {
        /// <summary>
        /// Конвертер пакета информации из промежуточной трансферной модели в класс клиентской части
        /// </summary>      
        FilesStatus ConvertToFilesStatusFromIntermediateResponse(FilesDataIntermediateResponseClient filesDataIntermediateResponse);

        /// <summary>
        /// Конвертер пакета информации из трансферной модели в класс клиентской части перед сохранение
        /// </summary>      
        FilesStatus ConvertToFilesStatus(FilesDataResponseClient filesDataResponse);


        /// <summary>
        /// Конвертер пакета информации из трансферной модели в класс клиентской части и сохранение файлов
        /// </summary>      
        Task<FilesStatus> ConvertToFilesStatusAndSaveFiles(FilesDataResponseClient filesDataResponse);
        
    }
}
