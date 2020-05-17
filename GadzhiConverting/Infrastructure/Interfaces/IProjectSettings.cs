using System.Threading.Tasks;
using GadzhiConverting.Models.Implementations;
using GadzhiConverting.Models.Interfaces.Printers;

namespace GadzhiConverting.Infrastructure.Interfaces
{
    public interface IProjectSettings
    {
        /// <summary>
        /// Папка для конвертирования файлов
        /// </summary>
        string ConvertingDirectory { get; }

        /// <summary>
        /// Информация о установленных в системе принтерах
        /// </summary>
        IPrintersInformation PrintersInformation { get; }

        /// <summary>
        /// Время через которое осуществляется проверка пакетов на сервере
        /// </summary>
        int IntervalSecondsToServer { get; }

        /// <summary>
        /// Время через которое осуществляется удаление ненужных пакетов на сервере
        /// </summary>
        int IntervalHoursToDeleteUnusedPackages { get; }

        /// <summary>
        /// Получить имя компьютера
        /// </summary>
        string NetworkName { get; }

        /// <summary>
        /// Пути ресурсов модулей конвертации
        /// </summary>
        public Task<ConvertingResources> ConvertingResources { get; }
    }
}
