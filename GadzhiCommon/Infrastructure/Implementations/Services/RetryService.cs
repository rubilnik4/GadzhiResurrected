using System;

namespace GadzhiCommon.Infrastructure.Implementations.Services
{
    /// <summary>
    /// Параметры повторных обращений к сервису
    /// </summary>
    public class RetryService
    {
        public RetryService()
           : this(0, 0)
        { }

        public RetryService(int allowedRetries)
            : this(1, allowedRetries)
        { }

        protected RetryService(int currentRetry, int allowedRetries)
        {
            if (allowedRetries < 0) throw new ArgumentOutOfRangeException(nameof(allowedRetries));
            if (currentRetry < 0) throw new ArgumentOutOfRangeException(nameof(allowedRetries));

            AllowedRetries = allowedRetries;
            CurrentRetry = currentRetry;
        }

        /// <summary>
        /// Номер текущей попытки
        /// </summary>
        public int CurrentRetry { get; }

        /// <summary>
        /// Допустимое количество попыток
        /// </summary>
        public int AllowedRetries { get; }

        /// <summary>
        /// Пауза при повторном обращении в секундах
        /// </summary>
        public static int RetryDelaySeconds => 15;

        /// <summary>
        /// Является ли текущая попытка последней
        /// </summary> <returns></returns>
        public bool IsRetryLast() => CurrentRetry == AllowedRetries;

        /// <summary>
        /// Допустима ли текущая попытка
        /// </summary>
        public bool IsRetryValid() => CurrentRetry <= AllowedRetries;

        /// <summary>
        /// Получить параметры следующего обращения
        /// </summary>
        public RetryService NextRetry() => new RetryService(CurrentRetry + 1, AllowedRetries);
    }
}