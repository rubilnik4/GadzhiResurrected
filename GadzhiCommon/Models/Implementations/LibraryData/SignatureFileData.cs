using System;
using GadzhiCommon.Models.Interfaces.LibraryData;

namespace GadzhiCommon.Models.Implementations.LibraryData
{
    /// <summary>
    /// Имя с идентификатором и изображением в байтовом виде
    /// </summary>
    public class SignatureFileData : SignatureLibrary, ISignatureFileData
    {
        public SignatureFileData(string id, PersonInformation personInformation, byte[] signatureFileData)
            : base(id, personInformation)
        {
            SignatureFileDataSource = ValidateSignatureFileDate(signatureFileData)
                ? signatureFileData
                : throw new ArgumentNullException(nameof(signatureFileData));
        }

        /// <summary>
        /// Изображение подписи
        /// </summary>
        public byte[] SignatureFileDataSource { get; }

        /// <summary>
        /// Проверить корректность данных
        /// </summary>
        public static bool ValidateSignatureFileDate(byte[] signatureFileData) => signatureFileData?.Length > 0;
    }
}
