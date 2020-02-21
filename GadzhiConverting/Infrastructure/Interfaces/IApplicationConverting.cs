using System;

namespace GadzhiConverting.Infrastructure.Interfaces
{
    /// <summary>
    /// Инфраструктура для конвертирования файлов
    /// </summary>
    public interface IApplicationConverting : IDisposable
    {
        /// <summary>
        /// Запустить процесс конвертирования
        /// </summary>      
        void StartConverting();
    }
}
