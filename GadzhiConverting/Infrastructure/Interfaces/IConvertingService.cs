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
