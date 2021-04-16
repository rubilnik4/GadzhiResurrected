using GadzhiConverting.Models.Implementations.FilesConvert;
using GadzhiConverting.Models.Interfaces.FilesConvert;
using GadzhiDTOServer.TransferModels.FilesConvert;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Interfaces.Converters
{
    /// <summary>
    /// Конвертер из серверной модели в трансферную
    /// </summary>
    public interface IConverterServerPackageDataToDto
    {
        /// <summary>
        /// Конвертировать серверную модель в промежуточную
        /// </summary>       
        PackageDataShortResponseServer FilesDataToShortResponse(IPackageServer packageServer);

        /// <summary>
        /// Конвертировать серверную модель в окончательный ответ
        /// </summary>          
        Task<PackageDataResponseServer> FilesDataToResponse(IPackageServer packageServer);

        /// <summary>
        /// Конвертировать файл серверной модели в окончательный ответ
        /// </summary>
        Task<FileDataResponseServer> FileDataToResponse(IFileDataServer fileDataServer);
    }
}