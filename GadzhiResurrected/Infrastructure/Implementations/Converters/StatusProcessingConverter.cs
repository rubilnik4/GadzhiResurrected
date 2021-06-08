using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiResurrected.Infrastructure.Implementations.Converters
{
    public static class StatusProcessingConverter
    {
        /// <summary>
        /// Словарь статуса обработки в строковом значении
        /// </summary>
        public static IReadOnlyDictionary<StatusProcessing, string> StatusProcessingString =>
            new Dictionary<StatusProcessing, string>
            {
                { StatusProcessing.NotSend, "На старте" },
                { StatusProcessing.Sending, "Отправляем" },
                { StatusProcessing.InQueue, "В очереди" },
                { StatusProcessing.Converting, "Крутим-вертим" },
                { StatusProcessing.ConvertingComplete, "Удачненько" },
                { StatusProcessing.Writing , "Записываем" },
                { StatusProcessing.End, "Готово" },
                { StatusProcessing.Archive, "В архиве" },
            };

        /// <summary>
        /// Преобразовать статус в наименование
        /// </summary>       
        public static string StatusProcessingToString(StatusProcessing statusProcessing)
        {
            var statusProcessingString = String.Empty;
            StatusProcessingString?.TryGetValue(statusProcessing, out statusProcessingString);

            return statusProcessingString;
        }

        /// <summary>
        /// Преобразовать наименование в статус
        /// </summary>       
        public static StatusProcessing StringToStatusProcessing(string statusProcessing) =>
            StatusProcessingString?.FirstOrDefault(status => status.Value == statusProcessing).Key
            ?? StatusProcessing.NotSend;
    }
}
