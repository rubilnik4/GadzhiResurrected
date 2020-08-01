using System;

namespace GadzhiConverting.Infrastructure.Interfaces
{
    /// <summary>
    /// Дата и время выполнения операций
    /// </summary>
    public interface IAccessService
    {
        /// <summary>
        /// Максимальное время ожидания до следующей операции
        /// </summary>
        int TimeOutLimit { get; }

        /// <summary>
        /// Время последней операции
        /// </summary>
        DateTime LastTimeOperation { get; }

        /// <summary>
        /// Время, прошедшее с последней операции
        /// </summary>
        double MinutesOfLastOperationByNow { get; }

        /// <summary>
        /// Превышение времени ожидания следующей операции
        /// </summary>
        bool IsTimeOut { get; }

        /// <summary>
        /// Установить время последней операции
        /// </summary>
        void SetLastTimeOperationByTime(DateTime dateTimeOperation);

        /// <summary>
        /// Установить время последней операции по текущему времени
        /// </summary>
        void SetLastTimeOperationByNow();
    }
}