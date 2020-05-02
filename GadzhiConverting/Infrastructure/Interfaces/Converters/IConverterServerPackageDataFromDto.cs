using GadzhiConverting.Models.Implementations.FilesConvert;
using GadzhiConverting.Models.Interfaces.FilesConvert;
using GadzhiDTOServer.TransferModels.FilesConvert;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Interfaces.Converters
{
    /// <summary>
    /// Конвертер из трансферной модели в серверную
    /// </summary>      
    public interface IConverterServerPackageDataFromDto
    {
        /// <summary>
        /// Конвертер пакета информации из трансферной модели в класс серверной части
        /// </summary>      
        Task<IPackageServer> ToFilesDataServerAndSaveFile(PackageDataRequestServer packageDataRequest);
    }
}
