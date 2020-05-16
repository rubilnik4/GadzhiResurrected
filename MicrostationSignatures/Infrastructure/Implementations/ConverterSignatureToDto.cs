using GadzhiDTOServer.TransferModels.Signatures;
using MicrostationSignatures.Models.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MicrostationSignatures.Infrastructure.Implementations
{
    /// <summary>
    /// Преобразование подписи в трансферную модель
    /// </summary>
    public static class ConverterSignatureToDto
    {
        /// <summary>
        /// Преобразовать подписи в трансферную модель
        /// </summary>
        public static IList<SignatureDto> SignaturesToDto(IReadOnlyList<SignatureLibrary> signaturesLibrary) =>
            signaturesLibrary?.
            Select(SignatureToDto).ToList()
            ?? throw new ArgumentNullException(nameof(signaturesLibrary));

        /// <summary>
        /// Преобразовать подпись в трансферную модель
        /// </summary>
        public static SignatureDto SignatureToDto(SignatureLibrary signatureLibrary) =>
            new SignatureDto()
            {
                Id = signatureLibrary.Id,
                FullName = signatureLibrary.Fullname,
                SignatureJpeg = signatureLibrary.SignatureJpeg,
            };

        /// <summary>
        /// Преобразовать подпись Microstation в трансферную модель
        /// </summary>
        public static SignatureMicrostationDto SignatureMicrostationToDto(SignatureLibraryMicrostation signatureLibraryMicrostation) =>
            new SignatureMicrostationDto()
            {
                NameDatabase = signatureLibraryMicrostation.Fullname,
                MicrostationDataBase = signatureLibraryMicrostation.SignatureMicrostation,
            };
    }
}