using ConvertingModels.Models.Interfaces.FilesConvert;
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
        Task<IFileDataServerConverting> Converting(IFileDataServerConverting fileDataServer);
    }
}
