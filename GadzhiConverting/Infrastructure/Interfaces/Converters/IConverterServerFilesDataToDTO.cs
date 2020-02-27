using GadzhiConverting.Models.Implementations.FilesConvert;
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
        FilesDataIntermediateResponseServer ConvertFilesToIntermediateResponse(FilesDataServer filesDataServer);

        /// <summary>
        /// Конвертировать серверную модель в окончательный ответ
        /// </summary>          
        Task<FilesDataResponseServer> ConvertFilesToResponse(FilesDataServer filesDataServer);
    }
}