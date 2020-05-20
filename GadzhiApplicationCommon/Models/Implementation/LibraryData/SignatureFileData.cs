using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadzhiApplicationCommon.Models.Interfaces.LibraryData;

namespace GadzhiApplicationCommon.Models.Implementation.LibraryData
{
    /// <summary>
    /// Имя с идентификатором и изображением в байтовом виде
    /// </summary>
    public class SignatureFileData : SignatureLibrary, ISignatureFileData
    {
        public SignatureFileData(string id, string personName, byte[] signatureFileData)
            : base(id, personName)
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
