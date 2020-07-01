using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Enums.FilesConvert;

namespace GadzhiModules.Infrastructure.Implementations.Converters
{
    public static class StatusProcessingProjectConverter
    {
        /// <summary>
        /// Словарь статуса обработки проекта в строковом значении
        /// </summary>
        public static IReadOnlyDictionary<StatusProcessingProject, string> StatusProcessingProjectString =>
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
        public static IReadOnlyList<StatusProcessingProject> StatusProcessingProjectIsConverting =>
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
        public static string StatusProcessingProjectToString(StatusProcessingProject statusProcessingProject)
        {
            var statusProcessingProjectString = String.Empty;
            StatusProcessingProjectString?.TryGetValue(statusProcessingProject, out statusProcessingProjectString);

            return statusProcessingProjectString;
        }

        /// <summary>
        /// Преобразовать наименование в статус
        /// </summary>       
        public static StatusProcessingProject StringToStatusProcessingProject(string statusProcessingProject) =>
             StatusProcessingProjectString?.
             FirstOrDefault(status => status.Value == statusProcessingProject).Key 
             ?? StatusProcessingProject.NeedToLoadFiles;
    }
}
