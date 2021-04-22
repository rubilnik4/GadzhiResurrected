using System;
using GadzhiCommon.Models.Enums.ServerStates;
using GadzhiDAL.Infrastructure.Implementations.DateTimes;
using GadzhiDTOBase.TransferModels.ServerStates;

namespace GadzhiWcfHost.Infrastructure.Implementations.ServerStates
{
    /// <summary>
    /// Фабрика создания ответа ою информации о сервере
    /// </summary>
    public static class ServerDetailFactory
    {
        /// <summary>
        /// Время активности сервера в часах
        /// </summary>
        public const int LAST_ACTIVITY_TIMEOUT = 2;

        /// <summary>
        /// Сервер не инициализирован
        /// </summary>
        public static ServerDetailResponse CreateNotInitialize(string serverName) =>
            new ServerDetailResponse(serverName, null,
                                     new ServerDetailQueueResponse(String.Empty, String.Empty, String.Empty, 0, 0, 0),
                                     ServerActivityType.NotAvailable);

        /// <summary>
        /// Получить статус сервера
        /// </summary>
        public static ServerActivityType GetServerActivityType(DateTime lastAccess, string currentPackage) =>
            !String.IsNullOrWhiteSpace(currentPackage)
                ? ServerActivityType.InProgress
                : (lastAccess - DateTimeService.GetDateTimeNow()).TotalHours <= LAST_ACTIVITY_TIMEOUT
                    ? ServerActivityType.Empty
                    : ServerActivityType.NotAvailable;
    }
}