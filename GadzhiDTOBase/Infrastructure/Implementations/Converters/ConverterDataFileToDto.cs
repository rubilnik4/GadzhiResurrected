using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiCommon.Models.Implementations.LibraryData;
using GadzhiCommon.Models.Interfaces.LibraryData;
using GadzhiDTOBase.TransferModels.Signatures;

namespace GadzhiDTOBase.Infrastructure.Implementations.Converters
{
    /// <summary>
    /// Преобразование подписи в трансферную модель
    /// </summary>
    public static class ConverterDataFileToDto
    {
        /// <summary>
        /// Преобразовать подписи в трансферную модель
        /// </summary>
        public static IReadOnlyList<SignatureDto> SignaturesToDto(IReadOnlyList<ISignatureFileData> signaturesLibrary) =>
            signaturesLibrary?.
            Select(SignatureToDto).ToList()
            ?? throw new ArgumentNullException(nameof(signaturesLibrary));

        /// <summary>
        /// Преобразовать подпись в трансферную модель
        /// </summary>
        private static SignatureDto SignatureToDto(ISignatureFileData signatureFileData) =>
            new SignatureDto()
            {
                Id = signatureFileData.PersonId,
                PersonInformation = PersonInformationToDto(signatureFileData.PersonInformation),
                SignatureJpeg = signatureFileData.SignatureFileDataSource,
            };

        /// <summary>
        /// Преобразовать информацию о пользователе в  трансферную модель
        /// </summary>
        private static PersonInformationDto PersonInformationToDto(PersonInformation personInformation) =>
            new PersonInformationDto()
            {
                Surname = personInformation.Surname,
                Name = personInformation.Name,
                Patronymic = personInformation.Patronymic,
                Department = personInformation.Department,
            };
    }
}