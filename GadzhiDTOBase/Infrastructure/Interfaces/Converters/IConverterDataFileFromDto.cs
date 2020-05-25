using System.Collections.Generic;
using System.Threading.Tasks;
using GadzhiCommon.Models.Interfaces.LibraryData;
using GadzhiDTOBase.TransferModels.Signatures;

namespace GadzhiDTOBase.Infrastructure.Interfaces.Converters
{
    /// <summary>
    /// Преобразование подписи в трансферную модель
    /// </summary>
    public interface IConverterDataFileFromDto
    {
        /// <summary>
        /// Преобразовать подписи из трансферной модели и сохранить изображения
        /// </summary>
        IList<ISignatureFile> SignaturesFileFromDto(IList<SignatureDto> signaturesDto, string signatureFolder);

        /// <summary>
        /// Преобразовать подписи из трансферной модели и сохранить изображения
        /// </summary>
        Task<IList<ISignatureFile>> SignaturesFileFromDtoAsync(IList<SignatureDto> signaturesDto, string signatureFolder);
    }
}