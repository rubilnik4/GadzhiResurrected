using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Interfaces
{
    /// <summary>
    /// Запуск процесса конвертирования
    /// </summary>
    public interface IConvertingService
    {
        /// <summary>
        /// Получить пакет на конвертирование и запустить процесс
        /// </summary>        
        Task ConvertingFirstInQueuePackage();
    }
}
