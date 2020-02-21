using GadzhiCommon.Enums.FilesConvert;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GadzhiModules.Helpers.Converters
{
    public static class StatusProcessingProjectConverter
    {
        /// <summary>
        /// Словарь статуса обработки проекта в строком значении
        /// </summary>
        public static IReadOnlyDictionary<StatusProcessingProject, string> StatusProcessingProjectToString =
            new Dictionary<StatusProcessingProject, string>
            {
                { StatusProcessingProject.NeedToLoadFiles, "Перетащите барахлишко на экран" },
                { StatusProcessingProject.NeedToStartConverting, "Жмите конвертировать" },
                { StatusProcessingProject.Sending, "Отправляем файлики" },
                { StatusProcessingProject.InQueue, "Ожидаем своей очереди" },
                { StatusProcessingProject.Converting, "Процесс пошел" },
                { StatusProcessingProject.Receiving, "Возвращаем блудных сынов" },
                { StatusProcessingProject.Writing, "Пишем в папочки" },
                { StatusProcessingProject.End, "Финиш" },
                { StatusProcessingProject.Error, "Непредвиденные ошибочки" },
            };

        /// <summary>
        /// Список статусов, находящихся в процессе конвертирования
        /// </summary>
        public static IReadOnlyList<StatusProcessingProject> ConvertingStatusProcessingProject =>
             new List<StatusProcessingProject>()
             {
                StatusProcessingProject.Converting,
                StatusProcessingProject.InQueue,
                StatusProcessingProject.Receiving,
                StatusProcessingProject.Sending,
                StatusProcessingProject.Writing,
              };

        /// <summary>
        /// Преобразовать статус в наименование
        /// </summary>       
        public static string ConvertStatusProcessingProjectToString(StatusProcessingProject statusProcessingProject)
        {
            string statusProcessingProjectString = String.Empty;
            StatusProcessingProjectToString?.TryGetValue(statusProcessingProject, out statusProcessingProjectString);

            return statusProcessingProjectString;
        }

        /// <summary>
        /// Преобразовать наименование в статус
        /// </summary>       
        public static StatusProcessingProject ConvertStringToStatusProcessingProject(string statusProcessingProject)
        {
            StatusProcessingProject statusProcessingProjectOut = StatusProcessingProjectToString?.
                                                        FirstOrDefault(status => status.Value == statusProcessingProject).Key ??
                                                        StatusProcessingProject.NeedToLoadFiles;

            return statusProcessingProjectOut;
        }
    }
}
