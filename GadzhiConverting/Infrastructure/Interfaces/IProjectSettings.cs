using System.Threading.Tasks;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Infrastructure.Implementations.Converters;
using GadzhiConverting.Models.Implementations;
using GadzhiConverting.Models.Interfaces.Printers;

namespace GadzhiConverting.Infrastructure.Interfaces
{
    /// <summary>
    /// Параметры приложения
    /// </summary>
    public interface IProjectSettings
    {
        /// <summary>
        /// Пути ресурсов модулей конвертации
        /// </summary>
        public ConvertingResources ConvertingResources { get; }

        /// <summary>
        /// Информация о установленных в системе принтерах
        /// </summary>
        public IResultValue<IPrintersInformation> PrintersInformation { get; }
    }
}
