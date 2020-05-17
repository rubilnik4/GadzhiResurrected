using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiDTOServer.TransferModels.Signatures;

namespace GadzhiConverting.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Преобразование подписи в трансферную модель
    /// </summary>
    public static class ConverterDataFileFromDto
    {
        /// <summary>
        /// Преобразовать подписи в трансферную модель
        /// </summary>
        public static IReadOnlyList<SignatureLibrary> SignaturesFromDto(IList<SignatureDto> signaturesDto, bool signatureLoad) =>
            signaturesDto?.
            Select(signature => SignatureFromDto(signature, signatureLoad)).ToList()
            ?? throw new ArgumentNullException(nameof(signaturesDto));

        /// <summary>
        /// Преобразовать подпись Microstation в трансферную модель
        /// </summary>
        public static MicrostationDataFile MicrostationDataFileFromDto(MicrostationDataFileDto microstationDataFileDto) =>
            (microstationDataFileDto != null)
            ? new MicrostationDataFile(microstationDataFileDto.NameDatabase, microstationDataFileDto.MicrostationDataBase)
            : throw new ArgumentNullException(nameof(microstationDataFileDto));

        /// <summary>
        /// Преобразовать подпись в трансферную модель
        /// </summary>
        private static SignatureLibrary SignatureFromDto(SignatureDto signatureDto, bool signatureLoad) =>
            (signatureDto != null)
                ? signatureLoad
                    ? new SignatureLibrary(signatureDto.Id, signatureDto.FullName, signatureDto.SignatureJpeg)
                    : new SignatureLibrary(signatureDto.Id, signatureDto.FullName)
                : throw new ArgumentNullException(nameof(signatureDto));
    }
}