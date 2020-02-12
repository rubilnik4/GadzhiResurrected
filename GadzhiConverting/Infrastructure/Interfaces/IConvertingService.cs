using GadzhiCommon.Enums.FilesConvert;
using GadzhiConverting.Infrastructure.Interfaces.Converters;
using GadzhiConverting.Models.FilesConvert.Implementations;
using GadzhiDAL.Services.Implementations;
using GadzhiDTO.TransferModels.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Interfaces
{
    /// <summary>
    /// Запуск процесса конвертирования
    /// </summary>
    public interface IConvertingService
    {
        /// <summary>
        /// Получить пакет на конвертирование и запустить процесс
        /// </summary>        
        Task ConvertingFirstInQueuePackage();       
    }
}
