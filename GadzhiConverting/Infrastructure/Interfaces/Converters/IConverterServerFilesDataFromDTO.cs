using GadzhiConverting.Models.FilesConvert.Implementations;
using GadzhiDTOServer.TransferModels.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Interfaces.Converters
{
    /// <summary>
    /// Конвертер из трансферной модели в серверную
    /// </summary>      
    public interface IConverterServerFilesDataFromDTO
    {
        /// <summary>
        /// Конвертер пакета информации из трансферной модели в класс серверной части
        /// </summary>      
        Task<FilesDataServer> ConvertToFilesDataServerAndSaveFile(FilesDataRequestServer filesDataRequest);        
    }
}
