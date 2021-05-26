using System;
using GadzhiApplicationCommon.Models.Implementation.LibraryData;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiCommon.Models.Implementations.LibraryData;
using GadzhiCommon.Models.Interfaces.LibraryData;
using GadzhiConvertingLibrary.Extensions;

namespace GadzhiConvertingLibrary.Models.Converters
{
    public static class SignatureLibraryConverter
    {
        /// <summary>
        /// Преобразовать имя с идентификатором в класс модуля конвертации
        /// </summary>
        public static ISignatureLibraryApp ToSignatureLibraryApplication(ISignatureLibrary signatureLibraryBase) =>
            (signatureLibraryBase != null)
                ? new SignatureLibraryApp(signatureLibraryBase.PersonId, ToPersonInformationApp(signatureLibraryBase.PersonInformation))
                : throw new ArgumentNullException(nameof(signatureLibraryBase));

        /// <summary>
        /// Преобразовать имя с идентификатором и подписью в класс модуля конвертации
        /// </summary>
        public static ISignatureFileApp ToSignatureFileApplication(ISignatureFile signatureFileBase) =>
            (signatureFileBase != null)
                ? new SignatureFileApp(signatureFileBase.PersonId, ToPersonInformationApp(signatureFileBase.PersonInformation),
                                       signatureFileBase.SignatureFilePath, signatureFileBase.IsVerticalImage)
                : throw new ArgumentNullException(nameof(signatureFileBase));

        /// <summary>
        /// Преобразовать информацию о пользователе в класс модуля конвертации
        /// </summary>
        public static PersonInformationApp ToPersonInformationApp(PersonInformation personInformation) =>
            new PersonInformationApp(personInformation.Surname, personInformation.Name, personInformation.Patronymic,
                                     personInformation.DepartmentType.ToApplication(), DepartmentToApplicationConverter.GetDepartmentTypeFunc());
    }
}