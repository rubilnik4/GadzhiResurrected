using GadzhiCommon.Enums.FilesConvert;
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
        /// Статус обработки проекта
        /// </summary>
        StatusProcessingProject StatusProcessingProject { get; }

        /// <summary>
        /// Вычислить процент отконвертированных файлов
        /// </summary>
        int CalculatePercentageOfComplete();

        /// <summary>
        /// Получить статус обработки проекта c процентом выполнения
        /// </summary>       
        string GetStatusProcessingProjectName();
    }
}
