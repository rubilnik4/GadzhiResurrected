using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Helpers.Converters
{
    public static class StatusProcessingConverter
    {
        /// <summary>
        /// Словарь статуса обработки в строком значении
        /// </summary>
        public static IReadOnlyDictionary<StatusProcessing, string> StatusProcessingToString =
            new Dictionary<StatusProcessing, string>
            {
                { StatusProcessing.NotSend, "На старте" },
                { StatusProcessing.Sending, "Отправляем" },
                { StatusProcessing.InQueue, "В очереди" },
                { StatusProcessing.Converting, "Крутим-вертим" },
                { StatusProcessing.Completed, "Удачненько" },
                { StatusProcessing.Writing , "Записываем" },
                { StatusProcessing.End, "Готово" },
                { StatusProcessing.Error, "Ошибочка" },
            };      

        /// <summary>
        /// Преобразовать статус в наименование
        /// </summary>       
        public static string ConvertStatusProcessingToString(StatusProcessing statusProcessing)
        {
            string statusProcessingString = String.Empty;
            StatusProcessingToString?.TryGetValue(statusProcessing, out statusProcessingString);

            return statusProcessingString;
        }

        /// <summary>
        /// Преобразовать наименование в статус
        /// </summary>       
        public static StatusProcessing ConvertStringToStatusProcessing(string statusProcessing)
        {
            StatusProcessing statusProcessingOut = StatusProcessingToString?.
                                                        FirstOrDefault(status => status.Value == statusProcessing).Key ??
                                                        StatusProcessing.NotSend;

            return statusProcessingOut;
        }
    }
}
