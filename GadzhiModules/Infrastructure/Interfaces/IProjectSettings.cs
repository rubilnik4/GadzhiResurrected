using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiModules.Infrastructure.Implementations
{
    /// <summary>
    /// Параметры приложения
    /// </summary>
    public interface  IProjectSettings
    {
        /// <summary>
        /// Время через которое осуществляются промежуточные запросы к серверу для проверки статуса файлов
        /// </summary>
        int IntervalSecondsToToIntermediateResponse { get; }

        /// <summary>
        /// Имя папка, куда копируются отконвертированные файлы
        /// </summary>
        string DirectoryForSavingConvertedFiles { get; }
    }
}
