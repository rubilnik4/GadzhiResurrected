using System.Collections.Generic;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiDTOServer.TransferModels.Signatures;

namespace GadzhiConverting.Infrastructure.Interfaces.Converters
{
    /// <summary>
    /// Преобразование подписи в трансферную модель
    /// </summary>
    public interface IConverterDataFileFromDto
    {
        /// <summary>
        /// Преобразовать подписи из трансферной модели и сохранить изображения
        /// </summary>
        public IReadOnlyList<SignatureFile> SignaturesFileFromDto(IList<SignatureDto> signaturesDto, string signatureFolder);
    }
}