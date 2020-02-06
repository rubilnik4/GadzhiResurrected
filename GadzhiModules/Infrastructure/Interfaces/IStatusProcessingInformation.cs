using GadzhiCommon.Enums.FilesConvert;
using GadzhiModules.Infrastructure.Implementations.Information;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Infrastructure.Interfaces
{
    /// <summary>
    /// Класс для получения информации о текущем статусе конвертирования
    /// </summary>
    public interface IStatusProcessingInformation
    { 
        /// <summary>
        /// Индикатор конвертирования файлов
        /// </summary 
        bool IsConverting { get; }

        /// <summary>
        /// Изменился ли статус запуска/остановки процесса конвертирования
        /// </summary>
        bool IsConvertingChanged { get; }

        /// <summary>
        /// Статус выполнения проекта
        /// </summary>
        StatusProcessingProject StatusProcessingProject { get; }

        /// <summary>
        /// Процент выполнения пакета конвертирования
        /// </summary>
        int PercentageOfComplete { get; }

        /// <summary>
        /// Есть ли у текущего процесса процент выполнения
        /// </summary>
        bool HasStatusProcessPercentage { get; }

        /// <summary>
        /// Получить статус обработки проекта c процентом выполнения
        /// </summary>       
        string GetStatusProcessingProjectName();
    }
}
