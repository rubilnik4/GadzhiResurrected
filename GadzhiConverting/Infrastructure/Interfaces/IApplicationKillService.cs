using System;

namespace GadzhiConverting.Infrastructure.Interfaces
{
    /// <summary>
    /// Очистка приложений при их зависании
    /// </summary>
    public interface IApplicationKillService: IDisposable
    {
        /// <summary>
        /// Начать процесс сканирования на зависания
        /// </summary>
        void StartScan();

        /// <summary>
        /// Завершить процесс сканирования на зависания
        /// </summary>
        void StopScan();
    }
}