using GadzhiConverting.Models.Implementations.FilesConvert;
using GadzhiConverting.Models.Interfaces.FilesConvert;
using GadzhiDTOServer.TransferModels.FilesConvert;
using System.Threading.Tasks;

namespace GadzhiConverting.Infrastructure.Interfaces.Converters
{
    /// <summary>
    /// Конвертер из серверной модели в трансферную
    /// </summary>
    public interface IConverterServerFilesDataToDTO
    {
        /// <summary>
        /// Конвертировать серверную модель в промежуточную
        /// </summary>       
        FilesDataIntermediateResponseServer ConvertFilesToIntermediateResponse(IFilesDataServerConverting filesDataServer);

        /// <summary>
        /// Конвертировать серверную модель в окончательный ответ
        /// </summary>          
        Task<FilesDataResponseServer> ConvertFilesToResponse(IFilesDataServerConverting filesDataServer);
    }
}