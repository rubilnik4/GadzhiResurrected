using System;
using System.Collections.Generic;
using System.Linq;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;
using GadzhiCommon.Models.Interfaces.LibraryData;
using GadzhiConvertingLibrary.Models.Converters;

namespace GadzhiConvertingLibrary.Extensions
{
    public static class SignatureExtension
    {
        /// <summary>
        /// Преобразовать имя с идентификатором и подписью в класс модуля конвертации
        /// </summary>
        public static ISignatureLibraryApp ToApplication(this ISignatureLibrary signatureFile) =>
            SignatureLibraryConverter.ToSignatureLibraryApplication(signatureFile);

        /// <summary>
        /// Преобразовать имена с идентификаторами и подписями в класс модуля конвертации
        /// </summary>
        public static IEnumerable<ISignatureLibraryApp> ToApplication(this IEnumerable<ISignatureLibrary> signatureLibraries) =>
            signatureLibraries.Select(SignatureLibraryConverter.ToSignatureLibraryApplication);

        /// <summary>
        /// Преобразовать имя с идентификатором и подписью в класс модуля конвертации
        /// </summary>
        public static ISignatureFileApp ToApplication(this ISignatureFile signatureFileBase) =>
            SignatureLibraryConverter.ToSignatureFileApplication(signatureFileBase);

        /// <summary>
        /// Преобразовать имена с идентификаторами и подписями в класс модуля конвертации
        /// </summary>
        public static IEnumerable<ISignatureFileApp> ToApplication(this IEnumerable<ISignatureFile> signatureFiles) =>
            signatureFiles.Select(SignatureLibraryConverter.ToSignatureFileApplication);
    }
}