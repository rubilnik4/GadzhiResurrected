using GadzhiConverting.Models.Implementations.FilesConvert;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Interfaces
{
    public interface IConvertingFileData
    {
        /// <summary>
        /// Запустить конвертирование файла
        /// </summary>
        Task<FileDataServer> Converting(FileDataServer fileDataServer);
    }
}
