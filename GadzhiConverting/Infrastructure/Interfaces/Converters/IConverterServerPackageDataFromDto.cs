using GadzhiConverting.Models.Implementations.FilesConvert;
using GadzhiConverting.Models.Interfaces.FilesConvert;
using GadzhiDTOServer.TransferModels.FilesConvert;
using System.Threading.Tasks;
using GadzhiCommon.Models.Interfaces.Errors;
using GadzhiConverting.Models.Interfaces.Printers;

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
        Task<IResultValue<IPackageServer>> ToFilesDataServerAndSaveFile(PackageDataRequestServer packageDataRequest);
    }
}
