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
    public static class ConverterDataFileToDto
    {
        /// <summary>
        /// Преобразовать подписи в трансферную модель
        /// </summary>
        public static IList<SignatureDto> SignaturesToDto(IReadOnlyList<SignatureFileData> signaturesLibrary) =>
            signaturesLibrary?.
            Select(SignatureToDto).ToList()
            ?? throw new ArgumentNullException(nameof(signaturesLibrary));

        /// <summary>
        /// Преобразовать подпись Microstation в трансферную модель
        /// </summary>
        public static MicrostationDataFileDto MicrostationDataFileToDto(MicrostationDataFile microstationDataFile) =>
            new MicrostationDataFileDto()
            {
                NameDatabase = microstationDataFile.NameDatabase,
                MicrostationDataBase = microstationDataFile.MicrostationDataBase,
            };

        /// <summary>
        /// Преобразовать подпись в трансферную модель
        /// </summary>
        private static SignatureDto SignatureToDto(SignatureFileData signatureFileData) =>
            new SignatureDto()
            {
                Id = signatureFileData.Id,
                FullName = signatureFileData.Fullname,
                SignatureJpeg = signatureFileData.SignatureFileDataSource,
            };

    }
}