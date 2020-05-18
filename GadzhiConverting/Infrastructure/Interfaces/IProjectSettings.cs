using System.Threading.Tasks;
using GadzhiConverting.Models.Implementations;
using GadzhiConverting.Models.Interfaces.Printers;

namespace GadzhiConverting.Infrastructure.Interfaces
{
    public interface IProjectSettings
    {
        /// <summary>
        /// Пути ресурсов модулей конвертации
        /// </summary>
        public Task<ConvertingResources> ConvertingResources { get; }
    }
}
