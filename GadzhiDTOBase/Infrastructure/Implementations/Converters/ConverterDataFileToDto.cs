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
        public static IList<SignatureDto> SignaturesToDto(IEnumerable<ISignatureFileData> signaturesLibrary) =>
            signaturesLibrary.Select(SignatureToDto).ToList();

        /// <summary>
        /// Преобразовать подпись в трансферную модель
        /// </summary>
        private static SignatureDto SignatureToDto(ISignatureFileData signatureFileData) =>
            new SignatureDto(signatureFileData.PersonId, PersonInformationToDto(signatureFileData.PersonInformation),
                             signatureFileData.SignatureSource.ToArray());

        /// <summary>
        /// Преобразовать информацию о пользователе в  трансферную модель
        /// </summary>
        private static PersonInformationDto PersonInformationToDto(PersonInformation personInformation) =>
            new PersonInformationDto(personInformation.Surname, personInformation.Name,
                                     personInformation.Patronymic, personInformation.DepartmentType);
    }
}