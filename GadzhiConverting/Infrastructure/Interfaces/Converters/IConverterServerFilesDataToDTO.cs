using GadzhiConverting.Models.Implementations.FilesConvert;
using GadzhiConverting.Models.Interfaces.FilesConvert;
using GadzhiDTOServer.TransferModels.FilesConvert;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Interfaces.Converters
{
    /// <summary>
    /// Конвертер из серверной модели в трансферную
    /// </summary>
    public interface IConverterServerFilesDataToDto
    {
        /// <summary>
        /// Конвертировать серверную модель в промежуточную
        /// </summary>       
        PackageDataIntermediateResponseServer FilesDataToIntermediateResponse(IPackageServer packageServer);

        /// <summary>
        /// Конвертировать серверную модель в окончательный ответ
        /// </summary>          
        Task<PackageDataResponseServer> FilesDataToResponse(IPackageServer packageServer);
    }
}