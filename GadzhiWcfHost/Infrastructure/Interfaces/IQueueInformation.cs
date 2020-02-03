using GadzhiWcfHost.Infrastructure.Implementations.Information;
using GadzhiWcfHost.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GadzhiWcfHost.Infrastructure.Implementations
{
    /// <summary>
    /// Информация о статусе конвертируемых файлов
    /// </summary>
    public interface IQueueInformation
    {
        /// <summary>
        /// Получить информацию о количестве конвертируемых файлов в очереди
        /// </summary>        
        FilesQueueInfo GetQueueInfo();

        /// <summary>
        /// Получить информацию о количестве конвертируемых файлов в очереди до опеределенного пакета
        /// </summary>        
        FilesQueueInfo GetQueueInfoUpToIdPackage(Guid id);


    }
}