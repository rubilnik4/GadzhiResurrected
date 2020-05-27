using System;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Interfaces
{
    /// <summary>
    /// Запуск процесса конвертирования
    /// </summary>
    public interface IConvertingService: IDisposable
    {
        /// <summary>
        /// Запустить процесс конвертирования
        /// </summary>      
        Task StartConverting();
    }
}
