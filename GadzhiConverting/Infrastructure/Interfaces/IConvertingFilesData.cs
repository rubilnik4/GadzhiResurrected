using GadzhiConverting.Models.Implementations.FilesConvert;
using GadzhiConverting.Models.Interfaces.FilesConvert;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Interfaces
{
    public interface IConvertingFileData
    {
        /// <summary>
        /// Запустить конвертирование файла
        /// </summary>
        Task<IFileDataServer> Converting(IFileDataServer fileDataServer);
    }
}
