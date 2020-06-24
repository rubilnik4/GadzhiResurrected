using System.Threading.Tasks;
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
    }
}
