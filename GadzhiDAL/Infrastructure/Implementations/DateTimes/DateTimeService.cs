using System;

namespace GadzhiDAL.Infrastructure.Implementations.DateTimes
{
    /// <summary>
    /// Сервис определения текущего времени
    /// </summary>
    public static class DateTimeService
    {
        /// <summary>
        /// Получить текущее время
        /// </summary>
        public static DateTime GetDateTimeNow () => 
            DateTime.Now;
    }
}