using System;
using GadzhiConvertingLibrary.Infrastructure.Interfaces;

namespace GadzhiConvertingLibrary.Infrastructure.Implementations
{
    /// <summary>
    /// Дата и время выполнения операций
    /// </summary>
    public class AccessService: IAccessService
    {
        public AccessService(int timeOutLimit)
            :this(DateTime.Now, timeOutLimit)
        { }

        public AccessService(DateTime dateTimeNow, int timeOutLimit)
        {
            LastTimeOperation = dateTimeNow;
            TimeOutLimit = timeOutLimit;
        }

        /// <summary>
        /// Максимальное время ожидания до следующей операции
        /// </summary>
        public int TimeOutLimit{ get; }

        /// <summary>
        /// Время последней операции
        /// </summary>
        public DateTime LastTimeOperation { get; private set; }

        /// <summary>
        /// Время, прошедшее с последней операции
        /// </summary>
        public double MinutesOfLastOperationByNow => (DateTime.Now - LastTimeOperation).TotalMinutes;

        /// <summary>
        /// Превышение времени ожидания следующей операции
        /// </summary>
        public bool IsTimeOut => MinutesOfLastOperationByNow > TimeOutLimit;

        /// <summary>
        /// Установить время последней операции
        /// </summary>
        public void SetLastTimeOperationByTime(DateTime dateTimeOperation)
        {
            if (dateTimeOperation < LastTimeOperation) throw new ArgumentOutOfRangeException(nameof(dateTimeOperation));

            LastTimeOperation = dateTimeOperation;
        }

        /// <summary>
        /// Установить время последней операции по текущему времени
        /// </summary>
        public void SetLastTimeOperationByNow() => SetLastTimeOperationByTime(DateTime.Now);
    }
}