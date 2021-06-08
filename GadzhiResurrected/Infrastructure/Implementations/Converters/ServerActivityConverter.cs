using System.Collections.Generic;
using GadzhiCommon.Models.Enums.ServerStates;

namespace GadzhiResurrected.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Преобразования статуса сервера
    /// </summary>
    public static class ServerActivityConverter
    {
        /// <summary>
        /// Словарь статуса обработки в строковом значении
        /// </summary>
        public static IReadOnlyDictionary<ServerActivityType, string> ServerActivityString =>
            new Dictionary<ServerActivityType, string>
            {
                { ServerActivityType.Empty, "Свободен" },
                { ServerActivityType.InProgress, "В работе" },
                { ServerActivityType.NotAvailable, "Не доступен" },
                { ServerActivityType.NotInitialize, "Не инициализирован" },
            };
    }
}