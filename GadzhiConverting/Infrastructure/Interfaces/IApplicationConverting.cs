using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Interfaces
{
    /// <summary>
    /// Инфраструктура для конвертирования файлов
    /// </summary>
    public interface IApplicationConverting: IDisposable
    {
        /// <summary>
        /// Запустить процесс конвертирования
        /// </summary>      
        void StartConverting();
    }
}
